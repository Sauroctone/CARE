using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawnManager : MonoBehaviour {

	public float respawnTime;

	public void LaunchRespawnTimer(GameObject _pack)
	{
		StartCoroutine (RespawnTimer (_pack));
	}

	public IEnumerator RespawnTimer(GameObject _pack)
	{
		_pack.SetActive (false);
		yield return new WaitForSeconds (respawnTime);
		_pack.SetActive (true);
	}
}
