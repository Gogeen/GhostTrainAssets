using UnityEngine;
using System.Collections;

public class RoadPileDetectionController : MonoBehaviour {

	public PlayerTrain target;
	public float range;
	public GameUI gameUI;
	public string description;

	float GetMinimumRange()
	{
		float trainSpeed = target.speed;
		float trainAcceleration = target.acceleration;
		float range = (trainSpeed/2) * (trainSpeed/trainAcceleration);
		return range;
	}

	void Update()
	{
		GetComponent<CircleCollider2D> ().radius = range + GetMinimumRange ();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (!gameUI.IsWarningShown())
			{
				gameUI.ShowRoadWarning(description);
			}
		}
	}

	void OnDestroy()
	{
		gameUI.HideRoadWarning();

	}
}
