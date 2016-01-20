using UnityEngine;
using System.Collections;

public class ShieldVisualController : MonoBehaviour {
	bool isShown = false;

	Vector3 pos = new Vector3();

	void Start()
	{
		pos = transform.localPosition;
		GetComponent<Animator> ().Play ("shieldIdle");

	}

	void Update () {
		transform.localPosition = pos;
		if (PlayerSaveData.reference.trainData.conditions.Shield) {
			if (!isShown) {
				//show
				GetComponent<SpriteRenderer>().enabled = true;
				GetComponent<Animator> ().Play ("shieldUp");
				isShown = true;
			}
		}
		else{
			if (isShown){	
				//hide
				GetComponent<Animator> ().Play ("shieldDown");
				isShown = false;
			}
		}
	}
}
