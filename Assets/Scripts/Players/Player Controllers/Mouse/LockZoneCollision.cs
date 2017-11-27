using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneCollision : MonoBehaviour {

	public List<Transform> lockedEnemies = new List<Transform>();

	void OnTriggerEnter(Collider col)
	{
		lockedEnemies.Add (col.transform);
	}

	void OnTriggerExit(Collider col)
	{
		if (lockedEnemies.Contains (col.transform)) 
		{
			lockedEnemies.Remove (col.transform);
		}
	}
}
