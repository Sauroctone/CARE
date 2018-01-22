using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {

	public int hitPoints;
	public GameObject explosion;
	GameObject explosionInst;

	void Update()
	{
		if (hitPoints <= 0) 
		{
			//if (

			explosionInst = Instantiate (explosion) as GameObject;
			explosionInst.transform.position = transform.position;
			float dur = explosionInst.GetComponent<ParticleSystem> ().main.duration;
			Destroy (explosionInst, dur);
			Destroy (gameObject); 
		}
	}
}
