using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthpackBehaviour : MonoBehaviour {

	Vector3 originPos;
	HealthSpawnManager spawnMan;
	HealthpackUsage hpUsage;

	void Start()
	{
		originPos = transform.position;
		spawnMan = Camera.main.GetComponent<HealthSpawnManager> ();
	}

	void OnTriggerEnter (Collider col)
	{
		if (hpUsage == null)
			hpUsage = col.GetComponent<HealthpackUsage> ();

		if (!hpUsage.hasPack) 
		{
			hpUsage.GetHealthpack ();
			transform.position = originPos;
			spawnMan.LaunchRespawnTimer (gameObject);
		}
	}
}