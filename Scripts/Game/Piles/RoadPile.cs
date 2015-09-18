using UnityEngine;
using System.Collections;

public class RoadPile : MonoBehaviour {

	public float timeToRemove;
	float removeTimer;
	public float hitDamage;
	
	void Start()
	{
		removeTimer = timeToRemove;
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain controller = coll.transform.parent.GetComponent<PlayerTrain>();
			if (controller.speed == 0)
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
