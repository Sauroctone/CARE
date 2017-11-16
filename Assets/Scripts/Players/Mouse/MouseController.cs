using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController: MonoBehaviour {

	public GameObject healZone;
	public GameObject ccZone;
	private GameObject ccZoneInst;
	private GameObject arrowInst;
	public GameObject shield;
	public Transform player2;
	public Transform player1;

	RaycastHit hit;
	Ray ray;
	int layerMask;

	bool isBuffering;
	public float bufferTimer;
	public float shieldTimer;
	public float shieldWidth;
	public float ccZoneWidth;
	public float healZoneWidth;

	public float pullStrength;
	public float pullTime;	
	public Vector3 pullDir;
	public float pullHeight;

	public ParticleSystem healParticles;

	public ListManager lists;

	void Start ()
	{
		layerMask = LayerMask.GetMask ("CCZone");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			healZone.SetActive (true);

			if (ccZoneInst != null)
				Destroy (ccZoneInst);

			CheckShield ();
		}

		if (Input.GetMouseButton (1)) 
		{
			if (healZone.activeSelf && Vector3.Distance (transform.position, player2.position) <= healZoneWidth) 
			{
				Heal ();
			} 

			else 
			{
				if (healParticles.isEmitting)
					healParticles.Stop();
			}
		}

		if (Input.GetMouseButtonUp (1)) 
		{
			healZone.SetActive (false);
			if (healParticles.isEmitting)
				healParticles.Stop();
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			healZone.SetActive (false);

			ccZoneInst = GameObject.Instantiate (ccZone) as GameObject;
			arrowInst = ccZoneInst.transform.Find ("Arrow").gameObject;
			ccZoneInst.transform.position = transform.position;

			CheckShield ();
		}

		if (Input.GetMouseButton (0)) 
		{
			if (ccZoneInst != null)
			{
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
				{
					if (arrowInst.activeSelf)
						arrowInst.SetActive (false);
				}

				else
				{
					if (!arrowInst.activeSelf)
						arrowInst.SetActive (true);

					ccZoneInst.transform.LookAt (transform);
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) 
		{
			if (ccZoneInst != null) 
			{
				for (int i = 0; i < lists.enemyDatabase.Count; i++) 
				{
					if (Vector3.Distance(lists.enemyDatabase[i].transform.position, ccZoneInst.transform.position) <= ccZoneWidth)
						CrowdControl (lists.enemyDatabase[i]);
				}

				Destroy (ccZoneInst);
			} 
		}

		player1.LookAt (new Vector3 (transform.position.x, player1.transform.position.y, transform.position.z));
	}

	void CheckShield ()
	{
		if (!shield.activeSelf) 
		{
			if (isBuffering) 
			{
				if (Vector3.Distance(transform.position, player2.position) <= shieldWidth)
					StartCoroutine ("Shield");
			}

			else
				StartCoroutine ("ShieldInputBuffer");
		}
	}

	void CrowdControl (GameObject enemy)
	{
		if (arrowInst.activeSelf) 
		{
			pullDir = new Vector3 (transform.position.x - enemy.transform.position.x, pullHeight, transform.position.z - enemy.transform.position.z).normalized;
			StartCoroutine (enemy.GetComponent<EnemyBehaviour>().GetPulled());
		}

		else
			print ("stunned");

	}

	void Heal ()
	{
		if (!healParticles.isEmitting)
			healParticles.Play();
	}

	IEnumerator Shield ()
	{
		//print ("shield");
		shield.SetActive (true);
		yield return new WaitForSeconds (shieldTimer);
		shield.SetActive (false);
	}

	IEnumerator ShieldInputBuffer ()
	{
		//print ("is buffering");
		isBuffering = true;
		yield return new WaitForSeconds (bufferTimer);
		isBuffering = false;
		//print ("is not buffering");
	}
}
