using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	private Rigidbody rb;
	private float hinput;
	private float vinput;

	public Vector3 movement;

	private x360_Gamepad gamepad;
	private GamepadManager manager;
	public int player;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		manager = GamepadManager.Instance;
		gamepad = manager.GetGamepad (player);
	}

	void Update()
	{
		hinput = gamepad.GetStick_L ().X;
		vinput = gamepad.GetStick_L ().Y;
		movement = new Vector3 (hinput, 0, vinput).normalized * speed;

		//print (hinput);
		//print (vinput);
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z); 
	}
}
