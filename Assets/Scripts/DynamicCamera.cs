using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {

	public Camera cam;

	public Transform player1;
	public Transform player2;
	public float smoothTime;
	private Vector3 targetPos;
	private Vector3 velocity = Vector3.zero;

	private Vector3 viewPosP1;
	private Vector3 viewPosP2;
	private Vector3 screenCenter;
	private float minCameraHeight;
	public float maxCameraHeight;
	public float cameraDezoom;
	public float cameraZoom;
	public float dezoomLerp;
	public float zoomLerp;
	public AnimationCurve dezoomCurve;

	public float[] cases;
	private float closestToEdge;

	void Start ()
	{
		minCameraHeight = cam.transform.position.y;
	}

	void Update ()
	{
		CameraHeight ();
		SmoothFollow ();
	}

	void CameraHeight()
	{
		viewPosP1 = cam.WorldToViewportPoint (player1.position);
		viewPosP2 = cam.WorldToViewportPoint (player2.position);

		cases[0] = viewPosP1.x - 0.2f;
		cases[1] = 0.8f - viewPosP1.x;
		cases[2] = viewPosP1.y - 0.2f;
		cases[3] = 0.8f - viewPosP1.y;
		cases[4] = viewPosP2.x - 0.2f;
		cases[5] = 0.8f - viewPosP2.x;
		cases[6] = viewPosP2.y - 0.2f;
		cases[7] = 0.8f - viewPosP2.y;
		closestToEdge = 0;

		for (int i = 0; i < 8; i++)
		{
			if (cases[i] < closestToEdge) 
			{
				closestToEdge = cases [i];
			}
		}

		if (closestToEdge < 0) 
		{
			Dezoom (Mathf.Abs (closestToEdge));
		} 

		else if (cam.transform.localPosition.y > minCameraHeight)
		{
			for (int i = 0; i < 8; i++)
			{
				if (cases[i] > 0.2f && cases[i] > closestToEdge) 
				{
					closestToEdge = cases [i];
				}
			}

			if (closestToEdge > 0.2f) 
			{
				Zoom ();
			}
		}
	}

	void SmoothFollow ()
	{
		screenCenter = cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, cam.nearClipPlane));
		targetPos = new Vector3 ((player1.position.x + player2.position.x + screenCenter.x)/3, transform.position.y, (player1.position.z + player2.position.z + screenCenter.y)/3);
		transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, smoothTime); 
	}

	void Dezoom (float distanceToEdge)
	{
		dezoomLerp = dezoomCurve.Evaluate (distanceToEdge * 5);
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y + cameraDezoom, cam.transform.localPosition.z - cameraDezoom), dezoomLerp);
	}

	void Zoom ()
	{
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y - cameraZoom, cam.transform.localPosition.z + cameraZoom), zoomLerp);
	}
}
