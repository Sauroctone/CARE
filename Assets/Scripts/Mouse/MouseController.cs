using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController: MonoBehaviour {

	public GameObject healZone;
	public GameObject ccZone;

	bool timerIsTicking;
	public float ccTimer;

	Vector3 mouseScreenPos;

	void Update()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			healZone.SetActive (true);
			ccZone.SetActive (false);
		}

		if (Input.GetMouseButtonUp (1)) 
		{
			healZone.SetActive (false);
		}
			

		if (Input.GetMouseButtonDown (0)) 
		{
			if (timerIsTicking) 
			{
				ccZone.SetActive (false);
				Pull ();
			} 

			else 
			{
				healZone.SetActive (false);
				ccZone.SetActive (true);

				StartCoroutine ("CrowdControlTimer");
			}

		}
	}

	void Pull ()
	{

	}

	IEnumerator CrowdControlTimer ()
	{
		timerIsTicking = true;
		yield return new WaitForSeconds (ccTimer);
		timerIsTicking = false;
	}
}
