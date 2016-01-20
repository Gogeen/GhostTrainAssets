using UnityEngine;
using System.Collections;

public class RoadObjectTownFeatureDefence : MonoBehaviour {

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("player")) {
			if (coll.GetComponent<WagonScript> ().IsLast ()) {
				RoadFeatureController.reference.ToggleFeatures (true);
				GameObject.Find ("Map").GetComponent<AITrainSpawn> ().isSpawning = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("player")) {
			RoadFeatureController.reference.ToggleFeatures (false);
			//GameObject.Find ("Map").GetComponent<AITrainSpawn> ().isSpawning = false;
		}
	}
}
