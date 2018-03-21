using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneController : MonoBehaviour {

	GamepadController player;
//	DashController dash;
	Rigidbody rb;

	public GameObject lockZone;
	public Transform projector;

	float scale;
	float originalScale;
	public float scaleIncrement;
	public float speedDecrement;
	public float heightIncrement;
	float originalHeight;

	public float execSpeed;
	public bool isExecuting;
	public float execInterval;

	TrailSpawner trailSpawn;
	LockZoneCollision lockList;

	VibrationManager vibration;
	float leftPower;
	float rightPower;

	ScreenShakeGenerator shake;
	StateManager stateMan;

	public float initTimer;
	bool isLocked;

	public Material zoneMat;
	public Color initColor;
	public Color lockedColor;

	Coroutine initCoroutine;

    public GameObject trail;
    GameObject trailInstance;

	//	Mesh lockZoneMesh;

	[Header ("--- SOUND EFFECTS ---")]
	public AudioSource lockzoneLoad;
	public AudioSource lockzoneHit;

	void Start()
	{
		player = GetComponent <GamepadController> ();
		//dash = GetComponent <DashController> ();
		rb = GetComponent<Rigidbody> ();
		lockList = lockZone.GetComponent<LockZoneCollision> ();
		trailSpawn = GetComponent<TrailSpawner> ();
		vibration = GetComponent<VibrationManager> ();
		shake = Camera.main.GetComponent<ScreenShakeGenerator> ();
		stateMan = Camera.main.GetComponent<StateManager> ();

		//lockZoneMesh = lockZone.GetComponent<MeshFilter>().mesh;

		originalScale = lockZone.transform.localScale.x;
		scale = originalScale;
		originalHeight = projector.position.y;
	}

	void Update ()
	{
		PlayerInput ();
		DashCancelling ();
	}

	void PlayerInput()
	{

        //Pressing the lockzone button 
		if (stateMan.playerTwoState == PlayerTwoStates.Normal && player.gamepad.GetButtonDown("X")) 
		{
			lockZone.SetActive (true);
			zoneMat.color = initColor;			
			isLocked = false;
			initCoroutine = StartCoroutine (LockZoneInitiation ());

			stateMan.playerTwoState = PlayerTwoStates.Locking;

			leftPower = 0.2f;
			rightPower = 0.2f;
		}

        //While it's pressed
		if (lockZone.activeSelf && player.gamepad.GetButton ("X"))
		{
			scale += scaleIncrement * Time.deltaTime;
			lockZone.transform.localScale = new Vector3 (scale, 1, scale);

			projector.position = new Vector3 (projector.position.x, projector.position.y + heightIncrement * Time.deltaTime, projector.position.z);

			if (player.speed > 0)
				player.speed -= speedDecrement * Time.deltaTime;

			leftPower += 0.1f * Time.deltaTime;
			rightPower = leftPower;
			vibration.Vibrate (leftPower, rightPower);
		}

        //When it's released
		if (lockZone.activeSelf && !player.gamepad.GetButton ("X")) 
		{
			CleanZone ();

			vibration.Vibrate (0, 0);


			if (lockList.lockedEnemies.Count > 0) 
			{
				if (isLocked)
					StartCoroutine (LockZoneExecution ());
				else 
				{
					CleanEnemies ();
					player.speed = player.originalSpeed;
					stateMan.playerTwoState = PlayerTwoStates.Normal;
					StopCoroutine (initCoroutine);
				}

			}

			else
			{
				player.speed = player.originalSpeed;
				stateMan.playerTwoState = PlayerTwoStates.Normal;
				isLocked = false;
				StopCoroutine (initCoroutine);
			}
		}

	}

	void DashCancelling ()
	{
		if (stateMan.playerTwoState == PlayerTwoStates.Dashing) 
		{
			if (isExecuting) 
			{
				StopCoroutine (LockZoneExecution ());
				isExecuting = false;
			} 

			else if (lockZone.activeSelf) 
			{
				CleanZone ();
			}

			isLocked = false;
			if (initCoroutine != null)
				StopCoroutine (initCoroutine);
			CleanEnemies ();

			vibration.Vibrate (0, 0);
		}
	}

	void CleanZone()
	{
		lockzoneLoad.Stop ();
		lockZone.transform.localScale = new Vector3 (1, 1, 1);
		scale = originalScale;
		projector.position = new Vector3 (projector.position.x, originalHeight, projector.position.z);

		lockZone.SetActive (false);
	}

	void CleanEnemies()
	{
		foreach (Transform enemy in lockList.lockedEnemies) 
		{
            enemy.Find("LockFeedback").gameObject.SetActive(false);
        }

		lockList.lockedEnemies.Clear ();
	}

	IEnumerator LockZoneInitiation()
	{
		SoundFunctions.PlaySound (lockzoneLoad, true, 1.0f, 1.05f, false);
		yield return new WaitForSeconds (initTimer);
		isLocked = true;
		zoneMat.color = lockedColor;
	}

	IEnumerator LockZoneExecution()
	{
		isExecuting = true;

        if (trailInstance == null)
            trailInstance = Instantiate(trail, transform) as GameObject;

        Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"));

		while (lockList.lockedEnemies.Count > 0) 
		{
			Transform target = lockList.lockedEnemies [0];
			rb.MovePosition (target.position);
			target.GetComponent<EnemyHealthManager> ().hitPoints = 0;
			lockList.lockedEnemies.RemoveAt (0);
			shake.ShakeScreen (.2f, .15f);

			//if (lockList.lockedEnemies.Count > 0)
			//	lockList.lockedEnemies [0].GetComponent<Renderer> ().material = lockList.darkRed;
			
			yield return new WaitForSeconds (execInterval);
		}

		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"), false);

		player.speed = player.originalSpeed;
		isExecuting = false;
		isLocked = false;
		stateMan.playerTwoState = PlayerTwoStates.Normal;

		//Interrompre cette coroutine en cas de collision avec un élément d'enviro (ex : rocher)
	}
}
