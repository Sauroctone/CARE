using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private float hinput;
	private float vinput;

	Vector3 movement;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		hinput = Input.GetAxisRaw ("Horizontal");
		vinput = Input.GetAxisRaw ("Vertical");
		movement = new Vector3 (hinput, 0, vinput).normalized * speed;
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z); 
	}
}
