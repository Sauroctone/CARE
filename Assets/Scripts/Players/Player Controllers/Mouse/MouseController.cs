using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseController: MonoBehaviour {

	public GameObject healZone;
	public GameObject ccZone;
	GameObject ccZoneInst;
	GameObject arrowInst;
	public GameObject shield;
	public Transform player2;
	public Transform player1;

	Ray ray;
	int layerMask;

	bool isBuffering;
	bool leftClicked;
	public float bufferTimer;
	public float shieldTimer;
	public float shieldWidth;
	public float ccZoneWidth;
	public float healZoneWidth;

	Vector3 clickPosition;
	public float pullStrength;
	public float pullTime;	
	public Vector3 pullDir;
	public float pullHeight;

	public ParticleSystem healParticles;

	void Start ()
	{
		layerMask = LayerMask.GetMask ("CCZone");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			//On active le bouclier ou on lance le buffer
			CheckShield ();
		}

		if (Input.GetMouseButton (1)) 
		{
			//S'il n'y a pas eu de bouclier, on active la zone de heal
			if (!healZone.activeSelf && !isBuffering) 
			{
				healZone.SetActive (true);

				if (ccZoneInst != null)
					Destroy (ccZoneInst);
			}

			//On heale le joueur 2 à chaque frame
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
			//On arrête le heal

			healZone.SetActive (false);
			if (healParticles.isEmitting)
				healParticles.Stop();
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			//On active le bouclier ou on lance le buffer
			CheckShield ();
			//On conserve la position du clic pour la zone de CC 
			clickPosition = transform.position;
		}

		if (Input.GetMouseButton (0)) 
		{
			//S'il n'y a pas eu de bouclier, on crée la zone de CC

			if (ccZoneInst == null && !isBuffering)
			{
				healZone.SetActive (false);

				ccZoneInst = GameObject.Instantiate (ccZone) as GameObject;
				arrowInst = ccZoneInst.transform.Find ("Arrow").gameObject;
				ccZoneInst.transform.position = clickPosition;
			}

			//On gère la flèche de la zone 
			if (ccZoneInst != null)
			{
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, Mathf.Infinity, layerMask)) 
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
			//Si le joueur clique et relâche avant la fin du buffer

			if (ccZoneInst == null && isBuffering) 
			{
				//On enregistre qu'il a cliqué
				leftClicked = true;
			}

			//On checke les ennemis dans la zone et on les crowd control
			if (ccZoneInst != null) 
			{
				CrowdControlCollision ccList = ccZoneInst.GetComponent<CrowdControlCollision> ();

				if (ccList.ccEnemies.Count > 0) 
				{
					foreach (Transform enemy in ccList.ccEnemies) 
					{
						CrowdControl (enemy);
					}
				}

				Destroy (ccZoneInst);
			}
		}

		//S'il n'y a pas eu de shield et que le joueur avait cliqué rapidement
		if (leftClicked && !isBuffering) 
		{
			StartCoroutine (FastLeftClick ());
			leftClicked = false;
		}

		//On oriente l'avatar vers la souris
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

	void CrowdControl (Transform enemy)
	{
		if (arrowInst != null && arrowInst.activeSelf) 
		{
			pullDir = new Vector3 (transform.position.x - enemy.position.x, pullHeight, transform.position.z - enemy.position.z).normalized;
			StartCoroutine (enemy.gameObject.GetComponent<EnemyBehaviour> ().GetPulled ());
		} 

		else
			print ("stun");

		//else
			//print ("stunned");
	}

	void Heal ()
	{
		if (!healParticles.isEmitting)
			healParticles.Play();
	}

	IEnumerator FastLeftClick()
	{
		//On crée la zone

		if (ccZoneInst == null) 
		{
			healZone.SetActive (false);

			ccZoneInst = GameObject.Instantiate (ccZone) as GameObject;
			ccZoneInst.transform.position = clickPosition;

			yield return new WaitForFixedUpdate();
		}

		//On résout le crowd control des ennemis dans la zone

		CrowdControlCollision ccList = ccZoneInst.GetComponent<CrowdControlCollision> ();

		if (ccList.ccEnemies.Count > 0) 
		{
			foreach (Transform enemy in ccList.ccEnemies) 
			{
				CrowdControl (enemy);
			}
		}

		Destroy (ccZoneInst);
	}

	IEnumerator Shield ()
	{
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
