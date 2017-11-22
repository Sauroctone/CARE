using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadController : MonoBehaviour {

	public float speed;
	Rigidbody rb;
	float hinput;
	float vinput;

	public Vector3 movement;
	Vector3 direction;
	public Vector3 lastDirection;

	public x360_Gamepad gamepad;
	GamepadManager manager;
	public int player;

	private DashController dash;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		dash = GetComponent<DashController> ();

		manager = GamepadManager.Instance;
		gamepad = manager.GetGamepad (player);
	}

	void Update()
	{
		hinput = gamepad.GetStick_L ().X;
		vinput = gamepad.GetStick_L ().Y;

		direction = new Vector3 (hinput, 0, vinput).normalized;

		if (direction != Vector3.zero)
			lastDirection = direction;

		transform.rotation = Quaternion.LookRotation (lastDirection);

		movement = direction * speed;
	}

	void FixedUpdate ()
	{
		if (!dash.isDashing)
			rb.velocity = new Vector3 (movement.x, rb.velocity.y, movement.z); 
	}
}
