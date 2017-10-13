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
	public float portHighestDiff;
	public float portLowestDiff;
	public float minPortPos;
	public float maxPortPos;
	public float distanceToPortLimit;

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
		viewPosP1 = cam.WorldToViewportPoint (player1.position);	//Translating players' world position in viewport (0 to 1 for x and y)
		viewPosP2 = cam.WorldToViewportPoint (player2.position);

		cases[0] = Mathf.Round((viewPosP1.x - minPortPos) * 100) / 100;		//Calculating every case : the difference between players' viewport position and limits triggering the dezoom
		cases[1] = Mathf.Round((viewPosP1.y - minPortPos) * 100) / 100;
		cases[2] = Mathf.Round((maxPortPos - viewPosP1.x) * 100) / 100;
		cases[3] = Mathf.Round((maxPortPos - viewPosP1.y) * 100) / 100;
		cases[4] = Mathf.Round((viewPosP2.x - minPortPos) * 100) / 100;
		cases[5] = Mathf.Round((viewPosP2.y - minPortPos) * 100) / 100;
		cases[6] = Mathf.Round((maxPortPos - viewPosP2.x) * 100) / 100;
		cases[7] = Mathf.Round((maxPortPos - viewPosP2.y) * 100) / 100;

		portHighestDiff = 0;					//Initializing the biggest difference to zero

		for (int i = 0; i < 8; i++)
		{
			if (cases[i] < portHighestDiff) 	//Comparing every difference to the biggest negative difference (--> the player is beyond the limit)
			{
				portHighestDiff = cases [i];		//If that's the case, this case becomes the new biggest negative difference
			}
		}

		if (portHighestDiff < 0) 		//After checking every case, we check if one of the players was indeed beyond a limit
		{
		//	Dezoom (Mathf.Abs (portHighestDiff));	//We trigger a dezoom this frame, sending in the difference (for the lerp curve)
		} 

		else if (cam.transform.localPosition.y > minCameraHeight) //If the players are inside the limits, and the camera is dezoomed from default position
		{
			portLowestDiff = 0.5f - minPortPos;
			for (int i = 0; i < 8; i++)
			{
				if (cases[i] > distanceToPortLimit && cases[i] < portLowestDiff) //
				{
					portLowestDiff = cases [i];
				}
			}

			if (portLowestDiff > distanceToPortLimit) 
			{
			//	Zoom ();
			}
		}

		//print (portHighestDiff);
	}
		
	void Dezoom (float distanceToEdge)
	{
		dezoomLerp = dezoomCurve.Evaluate (distanceToEdge);	//We give the difference as the X parameter of the curve determining the lerp (bigger lerp if the player is closer to the viewport border)
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y + cameraDezoom, cam.transform.localPosition.z - cameraDezoom), dezoomLerp);
		print ("dezoom");
	}

	void Zoom ()
	{
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y - cameraZoom, cam.transform.localPosition.z + cameraZoom), zoomLerp);
		print ("zoom");
	}

	void SmoothFollow ()
	{
		screenCenter = cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, cam.nearClipPlane));
		targetPos = new Vector3 ((player1.position.x + player2.position.x + screenCenter.x)/3, transform.position.y, (player1.position.z + player2.position.z + screenCenter.y)/3);
		transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, smoothTime); 
	}
}
