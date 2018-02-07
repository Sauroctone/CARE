using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

	public MouseController mouse;
	public Rigidbody rb;
	public GameObject dust;
	public EnemyHealthManager health;
	public ScreenShakeGenerator shake;

	private bool isPulled;

	public int smallGroup;
	public int largeGroup;

	public Transform player1;
	public Transform player2;
	public Transform initPos;
	public NavMeshAgent navMeshAgent;

	public int enemiesAround;
	public enum enemyState {notGrouped, groupedSmall, groupedLarge};
	public enemyState currentState;

	public EnemyListManager listManager;

	// MANAGE ENEMIES IN A LIST (enemyManager on camera?) TO AVOID GAMEOBJECT.FIND

	void Start () {
		listManager.enemyList.Add (gameObject);
	}

	void Update () {
		SetEnemyState ();
	}

	void SetEnemyState () {

		// Check how many enemies are around this enemy, and set its currentState
		if (enemiesAround < smallGroup) {
			currentState = enemyState.notGrouped;
			GameObject tempTarget = ChooseTarget ();
			if (tempTarget == null) {
				tempTarget = player2.gameObject;
			}
			navMeshAgent.destination = tempTarget.transform.position;
		} else if ((enemiesAround >= smallGroup) && (enemiesAround < largeGroup)) {
			currentState = enemyState.groupedSmall;
			navMeshAgent.destination = player2.position;
		} else if (enemiesAround >= largeGroup) {
			currentState = enemyState.groupedLarge;
			navMeshAgent.destination = player1.position;
		}

	}

	GameObject ChooseTarget () {

		// Create an array of all existing enemies
		GameObject[] enemies;
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject target = null;
		GameObject closest = null;
		GameObject closestGrouped = null;
		float distance = Mathf.Infinity;

		// Seek and return the closest enemy in the array
		foreach (GameObject enemy in enemies) {

			// Exclude this Gameobject from the array (so that it won't target itself)
			if (enemy.Equals(this.gameObject)) {
				continue;
			}

			// Compare the distance between this enemy and its target
			Vector3 diff = enemy.transform.position - transform.position;
			float currentDistance = diff.sqrMagnitude;
			if (currentDistance < distance) {
				closest = enemy;
				distance = currentDistance;
			}

			// 
			if (enemy.GetComponent<EnemyBehaviour> ().currentState == enemyState.groupedSmall) {
				closestGrouped = enemy;
			} else {
				closest = enemy;
			}

			if (closestGrouped != null) {
				target = closestGrouped;
			} else if (closest != null) {
				target = closest;
			}

		}

		return target;
	}

	public IEnumerator GetPulled ()
    {
		isPulled = true;
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