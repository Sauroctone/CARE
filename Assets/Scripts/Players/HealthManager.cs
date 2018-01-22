using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	public float maxHealth;
	public float health;
	public Slider healthBar;
	bool isRecovering;
	public float recovery;

	void Start()
	{
		UpdateHealthBar ();
	}

	public void TakeDamage(float _damage)
	{
		if (!isRecovering)
		{
			health -= _damage;

			if (health < 0)
				health = 0;

			UpdateHealthBar ();

			if (health == 0)
				Die ();

			StartCoroutine (Recovery ());
		}
	}

	public void UpdateHealthBar()
	{
		healthBar.value = health / maxHealth;
	}

	void Die()
	{
		gameObject.SetActive (false);
	}

	IEnumerator Recovery ()
	{
		isRecovering = true;
		yield return new WaitForSeconds (recovery);
		isRecovering = false;
	}
}
