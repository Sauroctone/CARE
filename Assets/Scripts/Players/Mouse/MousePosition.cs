using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour {

	/*
	Vector3 mousePosition;
	public Transform mouseTransform;

	// Update is called once per frame
	void Update () 
	{

		Vector3 temp = new Vector3 (-Input.mousePosition.x, 0, -Input.mousePosition.y);

		mousePosition = Camera.main.ScreenToWorldPoint (temp);
		mousePosition.y = 0;

		mouseTransform.position = mousePosition;


		print ("x : " + mousePosition.x);
		print ("y : " + mousePosition.y);
		print ("z : " + mousePosition.z);
	}*/

	public RaycastHit hit;
	Ray ray;
	public Transform mouseTransform;
	int layerMask;

	void Start()
	{
		layerMask = LayerMask.GetMask ("Floor");
	}

	void Update() 
	{
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
		{
			mouseTransform.position = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
		}
	}
}