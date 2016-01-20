using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportAnomaly : Anomaly {

	public int teleportsCount;
	public float teleportTime;

	bool teleported = false;

	IEnumerator Teleport()
	{
		for (int teleportCounter = 0; teleportCounter < teleportsCount; teleportCounter++) {
			GameObject.Find ("Map").GetComponent<PlayerTrainSpawn> ().SetTrainTo (GameObject.Find ("Map").GetComponent<RoadScript> ().GetTeleportPoint(PlayerTrain.reference.GetWagon(0).nextUnreachedPoint));
			yield return new WaitForSeconds (teleportTime);
		}
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (!coll.gameObject.GetComponent<WagonScript>().IsHead())
				return;
			if (PlayerSaveData.reference.trainData.conditions.LostControl)
				return;
			if (teleported)
				return;
			teleported = true;
			StartCoroutine ("Teleport");
		}
	}
}
