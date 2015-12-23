using UnityEngine;
using System.Collections;

public class RoadObjectWaystationController : MonoBehaviour {

	public string name;

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain.reference.nearObject = true;
			if (PlayerTrain.reference.speed == 0 && !PlayerTrain.reference.ghostMode)
			{
				PlayerSaveData.reference.LoadWaystationInfo (name);
				TrainTimeScript.reference.ComeInCommunity();
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
