using UnityEngine;
using System.Collections;

public class RoadPileCollisionController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			InventorySystem.reference.BreakWholeInventory(transform.parent.GetComponent<RoadPile>().hitDamage);
			Destroy (transform.parent.gameObject);
		}
	}
}
