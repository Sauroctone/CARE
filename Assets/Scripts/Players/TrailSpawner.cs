using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpawner : MonoBehaviour {

	Rigidbody rb;
	public GameObject trail;
	GameObject trailInstance;

	public PlayerController player;

	void Start ()
	{ 
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		if (trailInstance == null && player.movement != Vector3.zero) 
		{
			trailInstance = Instantiate (trail, transform) as GameObject;
		}
	}
}
