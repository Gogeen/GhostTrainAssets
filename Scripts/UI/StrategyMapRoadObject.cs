using UnityEngine;
using System.Collections;

public class StrategyMapRoadObject : MonoBehaviour {

	public bool isWaystation;
	public bool isTrade;
	public bool isRepair;
	public float timeOnAction;
	public float timeToLastObject;
	public float timeOnObject;

	public void SetTrade(bool value)
	{
		if (isTrade != value)
		{
			if (isTrade)
			{
				timeOnObject -= timeOnAction;
			}
			else
			{
				timeOnObject += timeOnAction;
			}
			isTrade = value;
		}
	}

	public void SetRepair(bool value)
	{
		if (isRepair != value)
		{
			if (isRepair)
			{
				timeOnObject -= timeOnAction;
			}
			else
			{
				timeOnObject += timeOnAction;
			}
			isRepair = value;
		}
	}

	public void ShowWaystationInfo()
	{
		if (isWaystation)
		{

		}
	}
}
