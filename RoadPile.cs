using UnityEngine;
using System.Collections;

public class RoadPile : MonoBehaviour {

	//public WagonScript headWagon;
	//public float warningTime;
	public float timeToRemove;
	float removeTimer;
	public float hitDamage;


	void Start()
	{
		removeTimer = timeToRemove;
	}

	/*float GetRangeToPlayer()
	{
		float range = 0;
		Transform lastTrainPoint = headWagon.transform;
		Transform nextTrainPoint = headWagon.nextPoint;
		RoadScript road = headWagon.transform.parent.GetComponent<RoadScript> ();
		while ((lastTrainPoint.transform.position - transform.position).magnitude > (lastTrainPoint.transform.position - nextTrainPoint.position).magnitude)
		{
			range += (lastTrainPoint.transform.position - nextTrainPoint.position).magnitude;
			lastTrainPoint = nextTrainPoint;
			nextTrainPoint = road.GetNextPoint(nextTrainPoint);

		}
		range += (lastTrainPoint.transform.position - transform.position).magnitude;
		return range;
	}
	
	float GetMinimumRange()
	{
		float trainSpeed = TrainController.currentSpeed;
		float trainAcceleration = TrainController.currentAcceleration;
		float range = (trainSpeed/2) * (trainSpeed/trainAcceleration);
		return range;
	}



	void Update()
	{
		float rangeToPlayer = GetRangeToPlayer ();
		float minRange = GetMinimumRange();
		if (rangeToPlayer > minRange && rangeToPlayer <= minRange + GetComponent<CircleCollider2D>().radius)
		{
			if (!gameUI.IsWarningShown())
			{
				gameUI.ShowRoadWarning(description);
			}
		}

	}*/

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (TrainController.currentSpeed == 0)
			{
				removeTimer -= Time.deltaTime;
				if (removeTimer <= 0)
				{
					Destroy (gameObject);
				}
			}
		}
	}


}
