using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public ListManager lists;
	public MouseController mouse;
	public bool isPulled;
	Rigidbody rb;
	public GameObject dust;

	void Start ()
	{
		lists.enemyDatabase.Add (gameObject);
		rb = GetComponent<Rigidbody> ();
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
}
