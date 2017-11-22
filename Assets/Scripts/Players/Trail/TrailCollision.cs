using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollision : MonoBehaviour {

	TrailRenderer trail;

	public List<Vector3> trailPositions = new List<Vector3>();
	public float sampleRate;
	public LayerMask layer;

	void Start ()
	{
		trail = GetComponent<TrailRenderer> ();
		StartCoroutine (GetVertexPosition ());
		StartCoroutine (TimeDelay ());
	}

	IEnumerator TimeDelay()
	{
		yield return new WaitForSeconds (trail.time);
		StartCoroutine (RemoveVertexPosition ());
	}
		
	IEnumerator GetVertexPosition()
	{
		yield return new WaitForSeconds (sampleRate);
		trailPositions.Add (transform.position);
		StartCoroutine (GetVertexPosition ());
	}

	IEnumerator RemoveVertexPosition()
	{
		yield return new WaitForSeconds (sampleRate);
		trailPositions.RemoveAt(0);
		StartCoroutine (RemoveVertexPosition ());
	}

	void Update ()
	{
		for (int i = 0; i < trailPositions.Count-2; i++) 
		{
			if (Physics.Linecast (trailPositions [i], trailPositions [i + 1], layer))
			{
				//print ("touché");
			}

			//Debug
			Debug.DrawLine (trailPositions [i], trailPositions [i + 1], Random.ColorHSV());
		}
	}
}