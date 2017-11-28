using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneController : MonoBehaviour {

	GamepadController player;
	DashController dash;
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



//	Mesh lockZoneMesh;

	void Start()
	{
		player = GetComponent <GamepadController> ();
		dash = GetComponent <DashController> ();
		rb = GetComponent<Rigidbody> ();
		lockList = lockZone.GetComponent<LockZoneCollision> ();
		trailSpawn = GetComponent<TrailSpawner> ();
		vibration = GetComponent<VibrationManager> ();
		shake = Camera.main.GetComponent<ScreenShakeGenerator> ();

		//lockZoneMesh = lockZone.GetComponent<MeshFilter>().mesh;

		originalScale = lockZone.transform.localScale.x;
		scale = originalScale;
		originalHeight = projector.position.y;
	}

	void Update ()
	{
		if (!dash.isDashing && player.gamepad.GetButtonDown("B")) 
		{
			lockZone.SetActive (true);
			leftPower = 0.2f;
			rightPower = 0.2f;
		}

		if (lockZone.activeSelf && player.gamepad.GetButton ("B"))
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

		if (lockZone.activeSelf && !player.gamepad.GetButton ("B")) 
		{
			lockZone.transform.localScale = new Vector3 (1, 1, 1);
			scale = originalScale;
			projector.position = new Vector3 (projector.position.x, originalHeight, projector.position.z);
			vibration.Vibrate (0, 0);

			lockZone.SetActive (false);

			if (lockList.lockedEnemies.Count > 0)
				StartCoroutine (LockZoneExecution ());
			else
				player.speed = player.originalSpeed;
		}

		if (dash.isDashing && isExecuting) 
		{
			StopCoroutine (LockZoneExecution ());
			foreach (Transform enemy in lockList.lockedEnemies) 
			{
				enemy.GetComponent<Renderer> ().material = lockList.black;
			}

			lockList.lockedEnemies.Clear ();
			isExecuting = false;

			vibration.Vibrate (0, 0);
		}
	}

	IEnumerator LockZoneExecution()
	{
		isExecuting = true;

		if (trailSpawn.trailInstance == null) 
			trailSpawn.trailInstance = Instantiate (trailSpawn.trail, transform) as GameObject;

		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"));

		while (lockList.lockedEnemies.Count > 0) 
		{
			Transform target = lockList.lockedEnemies [0];
			rb.MovePosition (target.position);
			target.GetComponent<EnemyHealthManager> ().hitPoints = 0;
			lockList.lockedEnemies.RemoveAt (0);
			shake.ShakeScreen (.2f, .15f);

			if (lockList.lockedEnemies.Count > 0)
				lockList.lockedEnemies [0].GetComponent<Renderer> ().material = lockList.darkRed;
			
			yield return new WaitForSeconds (execInterval);
		}

		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"), false);

		player.speed = player.originalSpeed;
		isExecuting = false;

		//Interrompre cette coroutine en cas de collision avec un élément d'enviro (ex : rocher)
	}
}
