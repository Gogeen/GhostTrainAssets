using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TentacklesObject : MonoBehaviour {

	public float difficulty;
	public float grabTime;
	public int tentacklesCount;
	public float damage;
	public float damageFrequency;

	List<int> wagonsInside = new List<int>();

	bool PassCheck(PlayerTrain player)
	{
		if (difficulty > player.speed * PlayerSaveData.reference.trainData.currentWeight)
			return false;
		return true;
	}

	IEnumerator Grab()
	{
		PlayerSaveData.reference.trainData.conditions.LostControl = true;
		PlayerTrain.reference.AccelerateTo (0);

		float grabTimer = grabTime;
		float damageTimer = 0;
		while (grabTimer > 0) {
			if (damageTimer <= 0) {
				//Damage ();
				for (int tentacleIndex = 0; tentacleIndex < tentacklesCount; tentacleIndex++) {
					int wagonIndexToDamage = Random.Range (0,wagonsInside.Count);
					wagonIndexToDamage = wagonsInside [wagonIndexToDamage];
					InventoryItemsBreakSystem.reference.BreakWagon (wagonIndexToDamage, damage);
				}
				damageTimer = damageFrequency;
			}
			damageTimer -= Time.deltaTime;
			grabTimer -= Time.deltaTime;
			yield return null;
		}
		PlayerSaveData.reference.trainData.conditions.LostControl = false;
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("player")) {
			wagonsInside.Add (coll.GetComponent<WagonScript>().GetIndex());
			if (!coll.GetComponent<WagonScript> ().IsHead ())
				return;
			if (PassCheck (PlayerTrain.reference)) {
				Destroy (gameObject);
				return;
			} else {
				StartCoroutine ("Grab");
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("player")) {
			wagonsInside.Remove (coll.GetComponent<WagonScript> ().GetIndex ());
		}
	}
}
