using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public Transform target;
	Vector3 targetScreenPos;
	public Vector3 offset;
	RectTransform rect;

	void Start ()
	{
		rect = GetComponent<RectTransform> ();
	}

	void Update ()
	{
		targetScreenPos = Camera.main.WorldToScreenPoint (target.position);
		rect.position = targetScreenPos + Camera.main.ViewportToScreenPoint(offset);
	}
}
