using UnityEngine;
using System.Collections;

public class TownController : MonoBehaviour {

	public PlayerTrain playerTrain;

	public PlayerTrainSpawn spawner;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
            playerTrain.Stop();
            playerTrain.AccelerateTo(0);
            playerTrain.canControl = false;
			
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (playerTrain.speed == 0)
			{
				Debug.Log ("finished map");
                GameController.EnterTown(1);
				//spawner.SetTrainTo(spawner.spawnPoint);
				//playerTrain.canControl = true;
			}
		}
	}
}
