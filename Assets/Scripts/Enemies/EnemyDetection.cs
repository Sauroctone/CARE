using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

	public EnemyBehaviour enemy;

	void OnTriggerEnter (Collider col) {
		if (col.transform.tag == "Enemy") {
			enemy.enemiesAround++;
		}
        
	}

	void OnTriggerExit (Collider col) {
		if (col.transform.tag == "Enemy") {
			enemy.enemiesAround--;
        }
	}

}
