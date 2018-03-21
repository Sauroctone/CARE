using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {

	public int hitPoints;
	public GameObject explosion;
	GameObject explosionInst;
    public LockZoneCollision lockList;

    void Update()
	{
		if (hitPoints <= 0) 
		{
			explosionInst = Instantiate (explosion) as GameObject;
			explosionInst.transform.position = transform.position;
			float dur = explosionInst.GetComponent<ParticleSystem> ().main.duration;
			Destroy (explosionInst, dur);

            if (lockList.lockedEnemies.Contains(transform))
                lockList.lockedEnemies.Remove(transform);

            gameObject.SetActive(false);
		}
	}
}
