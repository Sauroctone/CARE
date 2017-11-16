using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollision : MonoBehaviour {

	public List<Vector3> trailPositions = new List<Vector3>();
	float timer = 0.1f;

	void Update () 
	{
		timer -= Time.deltaTime;

		if (timer <= 0) 
		{
			trailPositions.Add (transform.position);
			timer = 0.1f;
		}			
	}
}