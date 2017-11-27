using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControlCollision : MonoBehaviour {

	public List<Transform> ccEnemies = new List<Transform>();

	void OnTriggerEnter(Collider col)
	{
		ccEnemies.Add (col.transform);
	}
		
	void OnTriggerExit(Collider col)
	{
		if (ccEnemies.Contains (col.transform)) 
		{
			ccEnemies.Remove (col.transform);
		}
	}

	void OnDestroy ()
	{
		ccEnemies.Clear ();
	}
}
