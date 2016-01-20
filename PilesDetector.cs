using UnityEngine;
using System.Collections;

// piles detector for firewave sign
public class PilesDetector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.GetComponent<RoadPileCollisionController>() != null)
		{
			Debug.Log ("add pile");
			transform.parent.GetComponent<PlayerTrain> ().AddPileNear (coll.transform.parent.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.GetComponent<RoadPileCollisionController>() != null)
		{
			Debug.Log ("remove pile");
			transform.parent.GetComponent<PlayerTrain> ().RemovePileNear (coll.transform.parent.gameObject);
		}
	}

	void Update()
	{
		transform.localPosition = new Vector3 (0,0,0);
	}
}
