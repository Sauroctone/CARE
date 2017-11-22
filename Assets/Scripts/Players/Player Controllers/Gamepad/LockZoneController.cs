using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneController : MonoBehaviour {

	GamepadController player;
	DashController dash;

	public GameObject lockZone;
	public ListManager lists;

	float scale;
	float originalScale;
	public float scaleIncrement;
	public float lockZoneWidth;

	Mesh lockZoneMesh;

	void Start()
	{
		player = GetComponent <GamepadController> ();
		dash = GetComponent <DashController> ();
		lockZoneMesh = lockZone.GetComponent<MeshFilter>().mesh;

		originalScale = lockZone.transform.localScale.x;
		scale = originalScale;
	}

	void Update ()
	{
		if (!dash.isDashing && player.gamepad.GetButtonDown("B")) 
		{
			lockZone.SetActive (true);
		}

		if (lockZone.activeSelf && player.gamepad.GetButton ("B"))
		{
			scale += scaleIncrement * Time.deltaTime;
			lockZone.transform.localScale = new Vector3 (scale, 1, scale);
		}

		if (lockZone.activeSelf && !player.gamepad.GetButton ("B")) 
		{
			lockZone.transform.localScale = new Vector3 (1, 1, 1);
			scale = originalScale;

			//lockZoneWidth = lockZoneMesh.bounds.size.x * lockZone.transform.localScale.x;

			lockZone.SetActive (false);
			/*
			for (int i = 0; i < lists.enemyDatabase.Count; i++) 
			{
				if (Vector3.Distance (lists.enemyDatabase [i].transform.position, transform.position) <= lockZoneWidth)
					print ("in radius");
			}*/

			StartCoroutine (LockZoneExecution ());
		}
	}

	IEnumerator LockZoneExecution()
	{
		LockZoneCollision lockList = lockZone.GetComponent<LockZoneCollision> ();
		yield return null; //en attendant

		//Foncer dans chaque ennemi de la liste un par un et l'éliminer
		//Clean la liste d'ennemis lockés au fur et à mesure de l'élimination, OU clean entièrement en cas d'interruption
		//Pouvoir interrompre cette coroutine en dashant
		//Interrompre cette coroutine en cas de collision avec un élément d'enviro (ex : rocher)
	}
}
