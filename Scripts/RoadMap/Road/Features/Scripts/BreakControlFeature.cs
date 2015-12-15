using UnityEngine;
using System.Collections;

public class BreakControlFeature : RoadFeature {

    /*
	public float repeatTime;
	float repeatTimer;
	
	public float tickTime;
	public float workTime;
	
	[Range(0,100)]public float maxSpeedPercent;

	public override bool CanCast()
	{
		return repeatTimer <= 0;
	}

	public override IEnumerator Cast(PlayerTrain playerTrain)
	{
		repeatTimer = repeatTime;
		playerTrain.speedWheelScrollbar.backgroundWidget.color = Color.red;
		playerTrain.canControl = false;
		float workTimer = workTime;
		float tickTimer = 0;
		while (workTimer > 0)
		{
			if (stopFeature)
			{
				stopFeature = false;
				Debug.Log ("stopped break control");
				yield break;
			}
			if (tickTimer <= 0)
			{
				tickTimer = tickTime;
				playerTrain.AccelerateTo(Random.Range(playerTrain.minSpeed, playerTrain.maxSpeed*maxSpeedPercent/100));
			}
			tickTimer -= Time.deltaTime;
			workTimer -= Time.deltaTime;
			yield return null;
		}
		repeatTimer = repeatTime;
		if (workTimer <= 0)
		{
			playerTrain.canControl = true;
			playerTrain.speedWheelScrollbar.backgroundWidget.color = Color.white;
		}
		
	}



	public override void OnStart()
	{
		repeatTimer = repeatTime;

	}

	public override void OnUpdate()
	{
		base.OnUpdate ();
		if (!turnedOn)
			return;

		if (repeatTimer > 0) {
			repeatTimer -= Time.deltaTime;
		}
	}*/
}