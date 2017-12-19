using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	bool isRegen;

	public Slider[] sliders;
	public Image[] batteries;
	public Image[] fills;

	public Color emptyColor;
	public Color fullColor;

	void Start () 
	{
	//	rb = GetComponent<Rigidbody> ();
		player = GetComponent<GamepadController> ();

		for (int i = 0; i < dashCount; i++) 
		{
			batteries [i].color = fullColor;
			fills [i].color = fullColor;
		}
	}

	void Update () 
	{
		if (dashCount > 0 && canDash && !isDashing && player.gamepad.GetButtonDown ("A")) 
		{
			StartCoroutine (Dash ());
		}

		if (dashCount < 3) 
		{
			sliders [dashCount].value += Time.deltaTime / dashRegen;
		}
	}

	IEnumerator Dash()
	{
		canDash = false;
		isDashing = true;

		dashCount -= 1;

		if (!isRegen)
			StartCoroutine (DashRegen ());

		if (dashCount < 2) 
		{
			sliders [dashCount].value = sliders [dashCount+1].value;
			sliders [dashCount+1].value = 0;
			batteries [dashCount].color = emptyColor;
			fills [dashCount].color = emptyColor;
			batteries [dashCount + 1].color = emptyColor;
		}

		else
		{
			sliders [dashCount].value = 0;
			batteries [dashCount].color = emptyColor;
			fills [dashCount].color = emptyColor;
		}

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
		isRegen = true;
		batteries [dashCount].color = emptyColor;
		fills [dashCount].color = emptyColor;

		yield return new WaitForSeconds (dashRegen);
		batteries [dashCount].color = fullColor;
		fills [dashCount].color = fullColor;
		dashCount += 1;

		if (dashCount < 3) 
		{
			StartCoroutine (DashRegen ());
		} 

		else
			isRegen = false;
	}
}
