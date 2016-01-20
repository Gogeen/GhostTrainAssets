using UnityEngine;
using System.Collections;

public class RoadObjectTownController : MonoBehaviour {

	//public PlayerTrainSpawn spawner;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain.reference.Stop();
			PlayerTrain.reference.AccelerateTo(0);
			PlayerSaveData.reference.trainData.conditions.LostControl = true;

		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (PlayerTrain.reference.speed == 0)
			{
				GetComponent<BoxCollider2D> ().enabled = false;
				Debug.Log ("finished map");
				TrainTimeScript.reference.ComeInCommunity();
				GlobalUI.reference.SetState (GlobalUI.States.Town);
                //Application.LoadLevel(1);
			}
		}
	}
}
