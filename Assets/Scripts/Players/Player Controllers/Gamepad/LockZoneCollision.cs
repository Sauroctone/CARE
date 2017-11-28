using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneCollision : MonoBehaviour {

	public List<Transform> lockedEnemies = new List<Transform>();
	public Material darkRed;
	public Material darkerRed;
	public Material black;


	void OnTriggerEnter(Collider col)
	{
		lockedEnemies.Add (col.transform);
		col.GetComponent<Renderer>().material = darkerRed;
	}

	void OnTriggerExit(Collider col)
	{
		if (lockedEnemies.Contains (col.transform)) 
		{
			lockedEnemies.Remove (col.transform);
			col.GetComponent<Renderer>().material = black;
		}
	}

	void Update ()
	{
		if (lockedEnemies.Count > 0) 
		{
			Renderer rend = lockedEnemies [0].GetComponent<Renderer> ();
			if (rend.material != darkRed)
				rend.material = darkRed;
		}
	}
}
