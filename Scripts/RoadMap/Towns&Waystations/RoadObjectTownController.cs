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
			PlayerTrain.reference.canControl = false;
			
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (PlayerTrain.reference.speed == 0)
			{
				Debug.Log ("finished map");
				TrainTimeScript.reference.ComeInCommunity();
                Application.LoadLevel(1);
			}
		}
	}
}
