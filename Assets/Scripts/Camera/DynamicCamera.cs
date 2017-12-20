using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {

	public Camera cam;

	public Transform player1;
	public Transform player2;
	public float smoothTime;
	Vector3 targetPos;
	Vector3 velocity = Vector3.zero;

	Vector3 viewPosP1;
	Vector3 viewPosP2;
	Vector3 screenCenter;
	float minCameraHeight;
	public float maxCameraHeight;
	public float cameraDezoom;
	public float cameraZoom;
	public float zDivider;
	float dezoomLerp;
	public float zoomLerp;
	public AnimationCurve dezoomCurve;

	public float[] dezoomCases;
	public float portHighestDiff;
	public float minPortXPos;
	public float minPortYPos;
	public float maxPortXPos;
	public float maxPortYPos;
	public float distXToCenter;	
	public float distYToCenter;


	public Texture debugTexture;
	Vector3 minScreenPos;

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
		viewPosP2 = cam.WorldToViewportPoint (player2.position);

		dezoomCases[0] = Mathf.Round((viewPosP2.x - minPortXPos) * 100) / 100;		//Calculating every case : the difference between players' viewport position and limits triggering the dezoom
		dezoomCases[1] = Mathf.Round((maxPortYPos - viewPosP2.y) * 100) / 100;
		dezoomCases[2] = Mathf.Round((maxPortXPos - viewPosP2.x) * 100) / 100;
		dezoomCases[3] = Mathf.Round((viewPosP2.y - minPortYPos) * 100) / 100;

		portHighestDiff = 0;					//Initializing the biggest difference to zero

		for (int i = 0; i < dezoomCases.Length; i++)
		{
			if (dezoomCases[i] < portHighestDiff) 	//Comparing every difference to the biggest negative difference (--> the player is beyond the limit)
			{
				portHighestDiff = dezoomCases [i];		//If that's the case, this case becomes the new biggest negative difference
			}
		}

		if (portHighestDiff < 0 && cam.transform.localPosition.y < maxCameraHeight) 		//After checking every case, we check if one of the players was indeed beyond a limit
		{
			Dezoom (Mathf.Abs (portHighestDiff));	//We trigger a dezoom this frame, sending in the difference (for the lerp curve)
		}

		else if (cam.transform.localPosition.y > minCameraHeight) //If the players are inside the limits, and the camera is dezoomed from default position
		{
			if (viewPosP2.x > distXToCenter && viewPosP2.x < 0.5 + distXToCenter && viewPosP2.y > distYToCenter && viewPosP2.y < 0.3 + distYToCenter) 
			{
				Zoom ();
			}
		}
	}
		
	void Dezoom (float distanceToEdge)
	{
		dezoomLerp = dezoomCurve.Evaluate (distanceToEdge);	//We give the difference as the X parameter of the curve determining the lerp (bigger lerp if the player is closer to the viewport border)
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y + cameraDezoom, cam.transform.localPosition.z - cameraDezoom / zDivider), dezoomLerp);
		//cam.transform.localPosition = Vector3.SmoothDamp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y + cameraDezoom, cam.transform.localPosition.z - cameraDezoom / zDivider), ref velocity, dezoomLerp);
		//print ("dezoom");
	}

	void Zoom ()
	{
		cam.transform.localPosition = Vector3.Lerp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y - cameraZoom, cam.transform.localPosition.z + cameraZoom / zDivider), zoomLerp);
		//cam.transform.localPosition = Vector3.SmoothDamp (cam.transform.localPosition, new Vector3 (cam.transform.localPosition.x, cam.transform.localPosition.y - cameraZoom, cam.transform.localPosition.z + cameraZoom / zDivider), ref velocity, zoomLerp);
		//print ("zoom");
	}

	void SmoothFollow ()
	{
		//screenCenter = cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, cam.nearClipPlane));
		//targetPos = new Vector3 ((player1.position.x + player2.position.x + screenCenter.x)/3, transform.position.y, (player1.position.z + player2.position.z + screenCenter.y)/3);
		targetPos = player1.position + screenCenter;
		transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, smoothTime); 
	}
	/*
	//Debug camera zones
	void OnGUI()
	{
		minScreenPos = cam.ViewportToScreenPoint (new Vector3 (minPortXPos, minPortYPos, 0));
		Vector3 zoomRectOrigin = cam.ViewportToScreenPoint (new Vector3 (distXToCenter, distYToCenter, 0));

		GUI.DrawTexture (new Rect (minScreenPos.x, minScreenPos.y, Screen.width - minScreenPos.x * 2, Screen.height - minScreenPos.y * 2), debugTexture);
		GUI.DrawTexture (new Rect (zoomRectOrigin.x, zoomRectOrigin.y, Screen.width - zoomRectOrigin.x * 2, Screen.height - zoomRectOrigin.y * 2), debugTexture);
	}*/	
}
