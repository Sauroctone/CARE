﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private float hinput;
	private float vinput;
	Vector3 movement;
    Vector3 direction;
   // public float rotLerp;

	public LayerMask layer;
	public float checkDist;

    public Animator anim;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		hinput = Input.GetAxisRaw ("Horizontal");
		vinput = Input.GetAxisRaw ("Vertical");
        direction = new Vector3(hinput, 0, vinput);
        movement = direction * speed;

        if (direction != Vector3.zero)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotLerp);

            if (rb.velocity != Vector3.zero)
            {
                if (!anim.GetBool("isMoving"))
                    anim.SetBool("isMoving", true);
            }
        }
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z); 

		//Debug.DrawRay (transform.position, rb.velocity.normalized * checkDist, Color.red, .1f);

		if (Physics.Raycast(transform.position, rb.velocity.normalized, checkDist, layer))
		{
			rb.velocity = Vector3.zero;
		}

        if (rb.velocity == Vector3.zero)
        {
            if (anim.GetBool("isMoving"))
                anim.SetBool("isMoving", false);
        }
    }
}