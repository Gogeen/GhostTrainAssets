using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostsFeature : RoadFeature {

	public float tickTimeMin;
	public float tickTimeMax;
	public float packTickTimeMin;
	public float packTickTimeMax;
	float tickTimer;
	float packTickTimer;
	public int ghostsInPack;
	public int maxGhostCount;
	float speedPenalty;
	[Range(0,100)]public float ghostStrength;
	[Range(0,100)]public float ghostBreakingStrength;
	bool canCast;

	public GameObject ghostPrefab;
	List<GhostInfo> spawnedGhosts = new List<GhostInfo>();

	class GhostInfo
	{
		public GameObject obj;
		public int wagonIndex;
	}

	void SpawnGhost(PlayerTrain playerTrain, Vector3 side)
	{
		GhostInfo info = new GhostInfo ();
		GameObject newGhost = Instantiate (ghostPrefab) as GameObject;
		newGhost.GetComponent<GhostController> ().feature = this;
		info.obj = newGhost;
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
			if (spawnPoint != null) {
				info.wagonIndex = wagon.GetIndex ();
				break;
			}


			wagon = playerTrain.GetWagon (wagonIndex);
			spawnPoint = wagon.GetGhostPoint ();
			if (spawnPoint != null) {
				info.wagonIndex = wagon.GetIndex ();
				break;
			}

		}
		//GameObject wagon = playerTrain.GetWagon (Random.Range(0,wagonsCount)).gameObject;
		if (spawnPoint == null)
			return;

		newGhost.transform.parent = spawnPoint;
		newGhost.transform.localPosition = new Vector3(0,0,0);
		newGhost.transform.localScale = new Vector3(1,1,1);
		newGhost.transform.localEulerAngles = side;
		newGhost.GetComponent<Animator>().Play("move");

		spawnedGhosts.Add (info);

	}

	void RemoveGhosts()
	{
		for(int ghostIndex = spawnedGhosts.Count - 1; ghostIndex >= 0; ghostIndex--)
		{
			Destroy (spawnedGhosts[ghostIndex].obj);
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
			if (!turnedOn) {
				yield return null;
				continue;
			}
			if (stopFeature)
			{
				stopFeature = false;
				tickTimer = Random.Range(tickTimeMin, tickTimeMax);
				RemoveGhosts();
				playerTrain.speedDebuffPercent -= speedPenalty;
				speedPenalty = 0;

			}
			if (tickTimer <= 0)
			{
				tickTimer = Random.Range(tickTimeMin, tickTimeMax);
				for(int ghostIndex = 0; ghostIndex < spawnedGhosts.Count; ghostIndex++)
					InventoryItemsBreakSystem.reference.BreakWagon (spawnedGhosts[ghostIndex].wagonIndex, ghostBreakingStrength, true);
				if (spawnedGhosts.Count < maxGhostCount)
				{
					SpawnGhost(playerTrain, new Vector3(0,0,Random.Range(0.0f, 360.0f)));

				}
			}
			if (packTickTimer <= 0) {
				packTickTimer = Random.Range(packTickTimeMin, packTickTimeMax);
				Vector3 side = new Vector3(0,0,Random.Range(0.0f, 360.0f));
				for (int ghostIndex = 0; ghostIndex < ghostsInPack; ghostIndex++) {
					if (spawnedGhosts.Count < maxGhostCount)
					{
						SpawnGhost(playerTrain, side);

					}
				}
					
			}
			tickTimer -= Time.deltaTime;
			packTickTimer -= Time.deltaTime;
			yield return null;
		}
	}

	public override void OnStart()
	{
		tickTimer = Random.Range(tickTimeMin, tickTimeMax);
		packTickTimer = Random.Range(packTickTimeMin, packTickTimeMax);
		canCast = true;
		speedPenalty = 0;
	}
}
