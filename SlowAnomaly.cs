using UnityEngine;
using System.Collections;

public class SlowAnomaly : Anomaly {

	[Range(1,99)]public float strength;

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			if (!trainController.IsSlowed()){
				trainController.minSpeed *= 1 - strength/100;
				trainController.maxSpeed *= 1 - strength/100;
				//trainController.acceleration *= 1 - strength/100;
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			if (trainController.IsSlowed())
			{
				trainController.minSpeed /= 1 - strength/100;
				trainController.maxSpeed /= 1 - strength/100;
			}
			//trainController.acceleration /= 1 - strength/100;
		}
	}
}
