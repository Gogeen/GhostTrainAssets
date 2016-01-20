using UnityEngine;
using System.Collections;

public class RoadPile : MonoBehaviour {

	public bool canRemove = false;
	public float timeToRemove;
	float removeTimer;
	public int hitPoints = 1;
	public float hitDamage;
	public bool isInvisible = false;
	
	void Start()
	{
		if (isInvisible)
			transform.FindChild ("sprite").gameObject.SetActive (false);
		removeTimer = timeToRemove;
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (!canRemove)
			return;
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

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (isInvisible)
				transform.FindChild ("sprite").gameObject.SetActive (true);
		}
	}

	void Update()
	{
		if (hitPoints <= 0)
			Destroy (gameObject);
		if (hitPoints > 1) {
			transform.FindChild("sprite").GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}

	void OnDestroy()
	{
		//PlayerTrain.reference.RemovePileNear (gameObject);
	}
}
