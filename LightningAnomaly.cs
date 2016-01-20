using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningAnomaly : MonoBehaviour {

	public float tickTime = 1;
	public float tickTimeSlow = 1;
	public float tickTimeFast = 1;
	public float Damage = 0;
	public float DetectRangeMin;
	public float DetectRangeMax;

	bool isPlayerInside = false;
	bool isPlayerNear = false;

	List<int> wagonIndexes = new List<int>();

	public void Activate()
	{
		if (!isPlayerNear) {
			transform.FindChild ("sprite").gameObject.SetActive (true);

			GetComponent<Animator> ().Play ("recharge");
			StartCoroutine ("Action");
		}
		isPlayerNear = true;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			isPlayerInside = true;
			wagonIndexes.Add (coll.gameObject.GetComponent<WagonScript>().GetIndex());
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (coll.gameObject.GetComponent<WagonScript> ().IsLast ())
				isPlayerInside = false;
			wagonIndexes.Remove (coll.gameObject.GetComponent<WagonScript>().GetIndex());
		}
	}

	IEnumerator Action()
	{
		yield return new WaitForSeconds (tickTime);
		if (isPlayerInside) {
			foreach (int wagonIndex in wagonIndexes) {
				InventoryItemsBreakSystem.reference.BreakWagon (wagonIndex, Damage);
			}
		}
		Destroy (gameObject);
	}

	void Start()
	{
		int speedType = Random.Range (0, 2);
		if (speedType == 0)
			tickTime = tickTimeSlow;
		else
			tickTime = tickTimeFast;

		GetComponent<Animator> ().speed = GetComponent<Animator> ().GetCurrentAnimatorClipInfo(0)[0].clip.length / tickTime;
		GetComponent<Animator> ().Play ("idle");
		transform.FindChild ("detectCollider").GetComponent<CircleCollider2D> ().radius = Random.Range (DetectRangeMin,DetectRangeMax);
	}
}
