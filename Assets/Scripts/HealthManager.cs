using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	public int hitPoints;

	void Update()
	{
		if (hitPoints <= 0) 
		{
			Destroy (gameObject);
		}
	}
}
