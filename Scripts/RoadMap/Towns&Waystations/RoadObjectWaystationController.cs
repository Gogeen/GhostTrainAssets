using UnityEngine;
using System.Collections;

public class RoadObjectWaystationController : MonoBehaviour {

	public VendorShop shopInfo;

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain.reference.nearObject = true;
			if (PlayerTrain.reference.speed == 0 && !PlayerTrain.reference.ghostMode)
			{
				TrainTimeScript.reference.ComeInWaystation();
				InventorySystem.reference.LoadShopInfo (shopInfo);
				GlobalUI.reference.SetState (GlobalUI.States.Waystation);
				GetComponent<BoxCollider2D>().enabled = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("enemy"))
		{
			if (coll.gameObject.GetComponent<WagonScript>().isHead)
			{
				EnemyTrain controller = coll.transform.parent.GetComponent<EnemyTrain>();
				controller.StopFor(controller.waystationTime);
			}
		}
	}
}
