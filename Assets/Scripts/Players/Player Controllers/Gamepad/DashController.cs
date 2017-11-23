using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour {

	GamepadController player;

	//private Rigidbody rb;

	public bool isDashing;
	bool canDash = true;
	public float dashStrength;
	public float dashTime;
	public float dashFreeze;
	public float dashCooldown;

	public enum PlayerTwoStates {Normal, Dashing};

	void Start () 
	{
	//	rb = GetComponent<Rigidbody> ();
		player = GetComponent<GamepadController> ();
	}

	void Update () 
	{
		if (canDash && !isDashing && player.gamepad.GetButtonDown ("A")) 
		{
			StartCoroutine (Dash ());
		}
	}

	IEnumerator Dash()
	{
		canDash = false;
		isDashing = true;
		//rb.AddForce(player.lastDirection * dashStrength, ForceMode.Impulse);
		player.speed = dashStrength;
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"));

		yield return new WaitForSeconds(dashTime);
	
		player.speed = 0;

		yield return new WaitForSeconds (dashFreeze);

		isDashing = false;
		player.speed = player.originalSpeed;
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"), false);
		//S'IL FINIT SON DASH DANS UN ENNEMI ?

		yield return new WaitForSeconds (dashCooldown);

		canDash = true;
	}
}
