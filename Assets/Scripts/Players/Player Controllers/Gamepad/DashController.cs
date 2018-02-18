﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashController : MonoBehaviour {

	GamepadController player;
	StateManager stateMan;
	HealthpackUsage hpUse;

	//private Rigidbody rb;

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

    public Animator anim;

    public GameObject trail;
    GameObject trailInstance;

	[Header ("--- SOUND EFFECTS ---")]
	public AudioSource dash;
	public AudioSource dashRegain;

	void Start () 
	{
	//	rb = GetComponent<Rigidbody> ();
		player = GetComponent<GamepadController> ();
		stateMan = Camera.main.GetComponent<StateManager> ();
		hpUse = GetComponent<HealthpackUsage> ();

		for (int i = 0; i < dashCount; i++) 
		{
			batteries [i].color = fullColor;
			fills [i].color = fullColor;
		}
	}

	void Update () 
	{
		if (stateMan.playerTwoState == PlayerTwoStates.Normal ||
		    stateMan.playerTwoState == PlayerTwoStates.Locking) 
		{
			if (dashCount > 0
			    && canDash
			    && player.gamepad.GetButtonDown ("A")) 
			{
				if (!hpUse.isInRange) 
				{
					SoundFunctions.PlaySound (dash, true, 1.0f, 1.05f, false);
					StartCoroutine (Dash ());
				}
			}
		}

		if (dashCount < 3) 
		{
			sliders [dashCount].value += Time.deltaTime / dashRegen;
		}
	}

	public void TestDash()
	{
		if (stateMan.playerTwoState == PlayerTwoStates.Normal || 
			stateMan.playerTwoState == PlayerTwoStates.Locking)
		{
			if (dashCount > 0 && canDash) 
			{
				StartCoroutine (Dash ());
			}
		}
	}

	IEnumerator Dash()
	{
        anim.SetTrigger("dashes");
        canDash = false;
		stateMan.playerTwoState = PlayerTwoStates.Dashing; 

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
        Vector3 lastPos = transform.position;

		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Enemy"), LayerMask.NameToLayer ("PlayerTwo"));
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("DashEnemy"), LayerMask.NameToLayer ("PlayerTwo"), false);

        if (trailInstance == null)
            trailInstance = Instantiate(trail, transform) as GameObject;

        yield return new WaitForSeconds(dashTime);
	
		player.speed = 0;
  //      print((lastPos - transform.position).magnitude);

		yield return new WaitForSeconds (dashFreeze);

		stateMan.playerTwoState = PlayerTwoStates.Normal; 
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
		SoundFunctions.PlaySound (dashRegain, false, 1.0f, 1.0f, false);
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
