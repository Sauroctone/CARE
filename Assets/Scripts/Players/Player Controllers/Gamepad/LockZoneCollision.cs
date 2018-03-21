using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockZoneCollision : MonoBehaviour {

	public List<Transform> lockedEnemies = new List<Transform>();
	//public Material darkRed;
	//public Material darkerRed;
	//public Material black;


	void OnTriggerEnter(Collider col)
	{
		Vector3 viewPos = Camera.main.WorldToViewportPoint(col.transform.position);

		if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
		{
			lockedEnemies.Add (col.transform);
            col.transform.Find("LockFeedback").gameObject.SetActive(true);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (lockedEnemies.Contains (col.transform)) 
		{
			lockedEnemies.Remove (col.transform);
            col.transform.Find("LockFeedback").gameObject.SetActive(false);
        }
	}

    public void CleanList()
    {

    }

	//void Update ()
	//{
	//	if (lockedEnemies.Count > 0) 
	//	{
	//		Renderer rend = lockedEnemies [0].GetComponent<Renderer> (); //SAAAAALE
	//		if (rend.material != darkRed)
	//			rend.material = darkRed;

	//	/*	for (int i = 0; i < lockedEnemies.Count; i++) 
	//		{
	//			Vector3 viewPos = Camera.main.WorldToViewportPoint(lockedEnemies[i].position);

	//			if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1 || viewPos.z <= 0) 
	//			{
	//				lockedEnemies.RemoveAt(i);
	//				print ("removed"); 
	//			}
	//		} */
	//	}
	//}
}
