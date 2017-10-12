using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private float hinput;
	private float vinput;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update()
	{
		hinput = Input.GetAxisRaw ("Horizontal"); 
		vinput = Input.GetAxisRaw ("Vertical");
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector3 (hinput * speed, rb.velocity.y, vinput * speed); 
	}
}
