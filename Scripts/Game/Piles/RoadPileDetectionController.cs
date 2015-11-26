using UnityEngine;
using System.Collections;

public class RoadPileDetectionController : MonoBehaviour {

	public float range;
    
    float GetMinimumRange()
	{
		float trainSpeed = PlayerTrain.reference.speed;
		float trainAcceleration = PlayerTrain.reference.acceleration;
		float range = (trainSpeed/2) * (trainSpeed/trainAcceleration);
		return range;
	}

	void Update()
	{
		GetComponent<CircleCollider2D> ().radius = range + GetMinimumRange ();
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
           
			PlayerTrain.reference.nearObject = true;
        }
	}

	void OnDestroy()
	{
		RoadFeatureController.reference.ToggleFeatures(true);

    }
}
