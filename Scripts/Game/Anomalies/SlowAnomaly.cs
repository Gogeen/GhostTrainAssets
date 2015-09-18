using UnityEngine;
using System.Collections;

public class SlowAnomaly : Anomaly {

	[Range(1,99)]public float strength;
	bool playerInside = false;
	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (playerInside)
				return;
			playerInside = true;
			TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			trainController.maxSpeed -= trainController.GetStartMaxSpeed()*(1 - strength/100);

		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			playerInside = false;
			TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			trainController.maxSpeed += trainController.GetStartMaxSpeed()*(1 - strength/100);
		}
	}
}
