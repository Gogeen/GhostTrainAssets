using UnityEngine;
using System.Collections;

public class RoadObjectWaystationController : MonoBehaviour {

	new public string name;

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain.reference.nearObject = true;
			if (PlayerTrain.reference.speed == 0)
			{
				GetComponent<BoxCollider2D>().enabled = false;
				PlayerSaveData.reference.LoadWaystationInfo (name);
				TrainTimeScript.reference.ComeInCommunity();
				GlobalUI.reference.SetState (GlobalUI.States.Waystation);
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

	void Update()
	{
		if (QuestsController.globalParameters.ContainsKey ("lostWaystation")) {
			if (QuestsController.globalParameters ["lostWaystation"] == 2) {
				QuestsController.globalParameters.Remove ("lostWaystation");
				Destroy (gameObject);
			}
		}
	}
}
