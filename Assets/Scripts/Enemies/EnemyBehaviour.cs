using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

    public int smallGroup;
    public int largeGroup;
    public int enemiesAround;
    bool isPulled;
    public EnemyState currentState;

    [Header("References")]
    public MouseController mouse;
	public Rigidbody rb;
	public GameObject dust;
	public EnemyHealthManager health;
	public ScreenShakeGenerator shake;
	public Transform player1;
	public Transform player2;
	public Transform initPos;
	public NavMeshAgent navMeshAgent;
	public EnemyListManager listManager;

	void Start ()
    {
		listManager.enemyList.Add (this);
	}

	void Update ()
    {
		SetEnemyState ();
	}

	void SetEnemyState ()
    {
		// Check how many enemies are around this enemy, and set its currentState
		if (enemiesAround < smallGroup)
        {
			currentState = EnemyState.NotGrouped;
			GameObject tempTarget = ChooseTarget ();

            if (tempTarget == null)
            {
				tempTarget = player2.gameObject;
			}

			navMeshAgent.destination = tempTarget.transform.position;
		}

        else if ((enemiesAround >= smallGroup) && (enemiesAround < largeGroup))
        {
			currentState = EnemyState.GroupedSmall;
			navMeshAgent.destination = player2.position;
		}

        else if (enemiesAround >= largeGroup)
        {
			currentState = EnemyState.GroupedLarge;
			navMeshAgent.destination = player1.position;
		}

	}

	GameObject ChooseTarget ()
    {
		// Create an array of all existing enemies
		//GameObject[] enemies;
		//enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		GameObject target = null;
		EnemyBehaviour closest = null;
		EnemyBehaviour closestGrouped = null;
		float distance = Mathf.Infinity;

		// Seek and return the closest enemy in the array
		foreach (EnemyBehaviour enemy in listManager.enemyList)
        {
			// Exclude this Gameobject from the array (so that it won't target itself)
			if (enemy.Equals(this))
            {
				continue;
			}

			// Compare the distance between this enemy and its target
			Vector3 diff = enemy.transform.position - transform.position;
			float currentDistance = diff.sqrMagnitude;
			if (currentDistance < distance)
            {
				closest = enemy;
				distance = currentDistance;
			}

			// 
			if (enemy.currentState == EnemyState.GroupedSmall)
            {
				closestGrouped = enemy;
			}

            else
            {
				closest = enemy;
			}

			if (closestGrouped != null)
            {
				target = closestGrouped.gameObject;
			}

            else if (closest != null)
            {
				target = closest.gameObject;
			}

		}

		return target;
	}

    public void GetPulled()
    {
        StartCoroutine(PullCor());
    }

    public void GetStunned(float _stunTime)
    {
        StartCoroutine(StunCor(_stunTime));
    }

    IEnumerator PullCor ()
    {
		isPulled = true;
        navMeshAgent.isStopped = true;
		yield return new WaitForSeconds (0.2f);

		GameObject dustInst = Instantiate (dust, transform.position, Quaternion.identity) as GameObject;
		Destroy (dustInst, 0.5f);
		rb.AddForce(mouse.pullDir * mouse.pullStrength, ForceMode.Impulse);
		yield return new WaitForSeconds (mouse.pullTime);

		rb.velocity = Vector3.zero;
		rb.useGravity = false;
		yield return new WaitForSeconds (0.1f);

		rb.useGravity = true;
		isPulled = false;
        navMeshAgent.isStopped = false;
	}

    IEnumerator StunCor (float _stunTime)
    {
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(_stunTime);
        navMeshAgent.isStopped = false;
    }

	void OnTriggerEnter (Collider col)
    {
		if (col.gameObject.tag == "Player")
        {
			health.hitPoints -= 1;
          //  Debug.Log("ERITHBRTBHIRETIBHERTBIHERTBHIERTBIHRETB");
			shake.ShakeScreen (.2f, .15f);
		}
	}

}