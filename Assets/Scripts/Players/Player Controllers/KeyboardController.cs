using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private float hinput;
	private float vinput;
	Vector3 movement;

	public LayerMask layer;
	public float checkDist;

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

		//Debug.DrawRay (transform.position, rb.velocity.normalized * checkDist, Color.red, .1f);

		if (Physics.Raycast(transform.position, rb.velocity.normalized, checkDist, layer))
		{
			rb.velocity = Vector3.zero;
		}
	}
}