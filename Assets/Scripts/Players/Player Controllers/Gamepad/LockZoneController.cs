using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneController : MonoBehaviour {

	GamepadController player;
	DashController dash;
	Rigidbody rb;

	public GameObject lockZone;

	float scale;
	float originalScale;
	public float scaleIncrement;
	public float lockZoneWidth;
	public float speedDecrement;

	public float execSpeed;
	public bool isExecuting;

	TrailSpawner trailSpawn;
	LockZoneCollision lockList;

	VibrationManager vibration;
	public float leftPower;
	public float rightPower;
	public float timer;

//	Mesh lockZoneMesh;

	void Start()
	{
		player = GetComponent <GamepadController> ();
		dash = GetComponent <DashController> ();
		rb = GetComponent<Rigidbody> ();
		lockList = lockZone.GetComponent<LockZoneCollision> ();
		trailSpawn = GetComponent<TrailSpawner> ();
		vibration = GetComponent<VibrationManager> ();

		//lockZoneMesh = lockZone.GetComponent<MeshFilter>().mesh;

		originalScale = lockZone.transform.localScale.x;
		scale = originalScale;
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

			lockZone.SetActive (false);

			if (lockList.lockedEnemies.Count > 0)
				StartCoroutine (LockZoneExecution ());
			else
				player.speed = player.originalSpeed;
		}

		if (dash.isDashing && isExecuting) 
		{
			StopCoroutine (LockZoneExecution ());
			lockList.lockedEnemies.Clear ();
			isExecuting = false;
		}
	}

	IEnumerator LockZoneExecution()
	{
		isExecuting = true;

		if (trailSpawn.trailInstance == null) 
			trailSpawn.trailInstance = Instantiate (trailSpawn.trail, transform) as GameObject;

		while (lockList.lockedEnemies.Count > 0) 
		{
			Transform target = lockList.lockedEnemies [0];
			rb.MovePosition (target.position);
			Destroy (target.gameObject);
			lockList.lockedEnemies.RemoveAt (0);
			yield return new WaitForSeconds (0.1f);
		}

		player.speed = player.originalSpeed;
		isExecuting = false;

		//Interrompre cette coroutine en cas de collision avec un élément d'enviro (ex : rocher)
	}
}
