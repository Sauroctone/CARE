using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthpackUsage : MonoBehaviour {

	GamepadController player;
	DashController dash;
	StateManager stateMan;

	public bool hasPack;
	public bool isInRange;
	public Transform playerOne;
	public float distanceToPlayer;
	float inputLoad;
	public float loadTime;

	public GameObject AButton;
	public HealthManager allyHealthMan;
    public ParticleSystem healParticles;

	public Image loader;


	void Start()
	{
		player = GetComponent<GamepadController> ();
		dash = GetComponent<DashController> ();
		stateMan = Camera.main.GetComponent<StateManager> ();
	}

	public void GetHealthpack()
	{
		hasPack = true;
		//feedback
	}

	void Update()
	{
		if (hasPack) 
		{
			CheckDistance ();
			PlayerInput ();
		}
	}

	void CheckDistance()
	{
		if (Vector3.Distance (playerOne.position, transform.position) <= distanceToPlayer &&  stateMan.playerTwoState == PlayerTwoStates.Normal) 
		{
				if (!isInRange)
					isInRange = true;
				if (!AButton.activeSelf)
					AButton.SetActive (true);
		}

		else 
		{
			if (isInRange)
				isInRange = false;
			if (AButton.activeSelf)
				AButton.SetActive (false);

			inputLoad = 0;
			loader.fillAmount = 0;
		}
	}

	void PlayerInput()
	{
		//Charger l'input A
		if (isInRange
			&& stateMan.playerTwoState == PlayerTwoStates.Normal
			&& player.gamepad.GetButton ("A")) 
		{
			inputLoad += Time.deltaTime;
			loader.fillAmount = inputLoad / loadTime;
		}

		//Input chargé, on donne le healthpack
		if (stateMan.playerTwoState == PlayerTwoStates.Normal && inputLoad >= loadTime) 
		{
			StartCoroutine (GiveHealthpack ());
			stateMan.playerTwoState = PlayerTwoStates.GivingHealth;
			hasPack = false;
			AButton.SetActive (false);
		}

		//Si le joueur lâche A avant la fin il dashe
		if (inputLoad > 0 && !player.gamepad.GetButton ("A")) 
		{
			inputLoad = 0;
			loader.fillAmount = 0;
			dash.TestDash ();
		}
	}

	IEnumerator GiveHealthpack()
	{
		yield return null;
		allyHealthMan.health = allyHealthMan.maxHealth;
		allyHealthMan.UpdateHealthBar ();
        healParticles.Play();
		inputLoad = 0;
		loader.fillAmount = 0;
		stateMan.playerTwoState = PlayerTwoStates.Normal;
        isInRange = false;
	}
}