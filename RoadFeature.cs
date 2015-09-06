using UnityEngine;
using System.Collections;

public class RoadFeature : MonoBehaviour {

	public TrainController playerTrain;
	public enum Features
	{
		BreakControl
	}
	public Features currentFeature;

	public float repeatTime;
	float repeatTimer;

	public float tickTime;
	public float workTime;

	[Range(0,100)]public float maxSpeedPercent;
	public IEnumerator BreakControl()
	{
		playerTrain.speedWheelScrollbar.backgroundWidget.color = Color.red;
		playerTrain.canControl = false;
		float workTimer = workTime;
		float tickTimer = 0;
		while (workTimer > 0)
		{
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
		playerTrain.canControl = true;
		playerTrain.speedWheelScrollbar.backgroundWidget.color = Color.white;

	}

	void Start()
	{
		repeatTimer = repeatTime;
	}

	bool turnedOn = true;
	public void ToggleFeature(bool value)
	{
		turnedOn = value;
	}

	void Update()
	{
		if (!turnedOn)
			return;
		if (repeatTimer > 0)
		{
			repeatTimer -= Time.deltaTime;
		}
		else
		{
			if (currentFeature == Features.BreakControl)
				StartCoroutine(BreakControl());

			repeatTimer = repeatTime;
		}
	}
}
