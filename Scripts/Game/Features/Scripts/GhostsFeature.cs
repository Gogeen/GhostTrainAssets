using UnityEngine;
using System.Collections;

public class GhostsFeature : RoadFeature {

	public float tickTime;
	float tickTimer;
	int ghostCount;
	public int maxGhostCount;
	float speedPenalty;
	[Range(0,100)]public float ghostStrength;
	bool canCast;

	void SpawnGhost()
	{
		ghostCount += 1;
		Debug.Log ("ghosts: "+ghostCount);
	}

	void RemoveGhosts()
	{
		ghostCount = 0;
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
				playerTrain.speedDebuffPercent += speedPenalty;
				speedPenalty = 0;

			}
			if (tickTimer <= 0)
			{
				tickTimer = tickTime;
				if (ghostCount < maxGhostCount)
				{
					SpawnGhost();
					speedPenalty += ghostStrength;
					playerTrain.speedDebuffPercent += ghostStrength;
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
