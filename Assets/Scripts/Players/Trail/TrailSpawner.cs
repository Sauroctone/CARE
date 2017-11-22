using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpawner : MonoBehaviour {

	public GameObject trail;
	GameObject trailInstance;

	public GamepadController player;

	void Update ()
	{
		if (trailInstance == null && player.movement != Vector3.zero) 
		{
			trailInstance = Instantiate (trail, transform) as GameObject;
		}
	}
}
