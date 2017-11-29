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
	public float dashInterval;
	public int dashCount;
	public float dashRegen;

	void Start () 
	{
	//	rb = GetComponent<Rigidbody> ();
		player = GetComponent<GamepadController> ();
	}

	void Update () 
	{
		if (dashCount > 0 && canDash && !isDashing && player.gamepad.GetButtonDown ("A")) 
		{
			StartCoroutine (Dash ());
		}
	}

	IEnumerator Dash()
	{
		canDash = false;
		isDashing = true;

		if (dashCount == 3)
			StartCoroutine (DashRegen ());
		
		dashCount -= 1;

		player.speed = dashStrength;
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"));
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("DashEnemy"), LayerMask.NameToLayer ("PlayerTwo"), false);

		yield return new WaitForSeconds(dashTime);
	
		player.speed = 0;

		yield return new WaitForSeconds (dashFreeze);

		isDashing = false;
		player.speed = player.originalSpeed;
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"), false);
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("DashEnemy"), LayerMask.NameToLayer ("PlayerTwo"));
		//S'IL FINIT SON DASH DANS UN ENNEMI ?

		yield return new WaitForSeconds (dashInterval);

		canDash = true;
	}

	IEnumerator DashRegen()
	{
		yield return new WaitForSeconds (dashRegen);
		dashCount += 1;

		if (dashCount < 3) 
		{
			StartCoroutine (DashRegen ());
		}
	}
}
