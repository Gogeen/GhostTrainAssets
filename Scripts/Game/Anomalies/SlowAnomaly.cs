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
			TrainController trainController = coll.transform.parent.GetComponent<TrainController>();
			trainController.maxSpeed -= trainController.GetStartMaxSpeed()*(1 - strength/100);
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
			trainController.maxSpeed += trainController.GetStartMaxSpeed()*(1 - strength/100);
		}
	}

    /*void Start()
    {
        GetComponent<AudioSource>().clip = sound;
        GetComponent<AudioSource>().Play();
    }*/
}
