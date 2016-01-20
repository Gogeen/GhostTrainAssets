using UnityEngine;
using System.Collections;

public class SlowAnomaly : Anomaly {

	[Range(1,99)]public float strength;
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
            if (!coll.gameObject.GetComponent<WagonScript>().IsHead())
                return;
			if (PlayerSaveData.reference.trainData.conditions.LostControl)
				return;
			TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			trainController.speedDebuffPercent += strength;
            source.Play();
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
            if (!coll.gameObject.GetComponent<WagonScript>().IsLast())
                return;
            TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			trainController.speedDebuffPercent -= strength;
		}
	}

    /*void Start()
    {
        GetComponent<AudioSource>().clip = sound;
        GetComponent<AudioSource>().Play();
    }*/
}
