using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostsFeature : RoadFeature {

	public float tickTime;
	float tickTimer;
	int ghostCount;
	public int maxGhostCount;
	float speedPenalty;
	[Range(0,100)]public float ghostStrength;
	[Range(0,100)]public float ghostBreakingStrength;
	bool canCast;

	public GameObject ghostPrefab;
	List<GameObject> spawnedGhosts = new List<GameObject>();
	void SpawnGhost(PlayerTrain playerTrain)
	{
		ghostCount += 1;
		Debug.Log ("ghosts: "+ghostCount);
		GameObject newGhost = Instantiate (ghostPrefab) as GameObject;
		spawnedGhosts.Add (newGhost);
		newGhost.GetComponent<GhostController> ().feature = this;

		int wagonsCount = 0;
		for(int childIndex = 0; childIndex < playerTrain.transform.childCount; childIndex++)
		{
			if (playerTrain.transform.GetChild(childIndex).GetComponent<WagonScript>() != null)
			{
				wagonsCount += 1;
			}
		}

		WagonScript wagon = null;
		Transform spawnPoint = null;
		for (int wagonIndex = 0; wagonIndex < wagonsCount; wagonIndex++) {
			// try to get random wagon point
			wagon = playerTrain.GetWagon (Random.Range(0,wagonsCount));
			spawnPoint = wagon.GetGhostPoint ();
			if (spawnPoint != null)
				break;


			wagon = playerTrain.GetWagon (wagonIndex);
			spawnPoint = wagon.GetGhostPoint ();
			if (spawnPoint != null)
				break;

		}
		//GameObject wagon = playerTrain.GetWagon (Random.Range(0,wagonsCount)).gameObject;
		if (spawnPoint == null)
			return;

		newGhost.transform.parent = spawnPoint;
		newGhost.transform.localPosition = new Vector3(0,0,0);
		newGhost.transform.localScale = new Vector3(1,1,1);
		newGhost.transform.localEulerAngles = new Vector3(0,0,Random.Range(0.0f, 360.0f));
		newGhost.GetComponent<Animator>().Play("move");
	}

	void RemoveGhosts()
	{
		ghostCount = 0;
		for(int ghostIndex = spawnedGhosts.Count - 1; ghostIndex >= 0; ghostIndex--)
		{
			Destroy (spawnedGhosts[ghostIndex]);
		}
		spawnedGhosts.Clear ();
	}

	public void FinishGhostMove()
	{
		PlayerTrain playerTrain = PlayerTrain.reference;
		speedPenalty += ghostStrength;
		playerTrain.speedDebuffPercent += ghostStrength;

	}

	public override bool CanCast()
	{
		return canCast;
	}

	public override IEnumerator Cast(PlayerTrain playerTrain)
	{
		canCast = false;
		while (true)
		{
			if (stopFeature)
			{
				stopFeature = false;
				tickTimer = tickTime;
				RemoveGhosts();
				playerTrain.speedDebuffPercent -= speedPenalty;
				speedPenalty = 0;

			}
			if (tickTimer <= 0)
			{
				tickTimer = tickTime;
				for(int ghostIndex = 0; ghostIndex < ghostCount; ghostIndex++)
					InventorySystem.reference.BreakItem (InventorySystem.reference.GetRandomItem(), ghostBreakingStrength);
				if (ghostCount < maxGhostCount)
				{
					SpawnGhost(playerTrain);

				}
			}
			tickTimer -= Time.deltaTime;
			yield return null;
		}
	}

	public override void OnStart()
	{
		ghostCount = 0;
		tickTimer = tickTime;
		canCast = true;
		speedPenalty = 0;
	}
}
