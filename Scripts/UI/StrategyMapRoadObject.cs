using UnityEngine;
using System.Collections;

public class StrategyMapRoadObject : MonoBehaviour {

	public bool isWaystation;
	public GameObject waystationInfoPrefab;
	GameObject waystationInfo = null;
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

	public void ToggleWaystationInfo()
	{
		if (isWaystation)
		{
			if (waystationInfo == null)
			{
				waystationInfo = Instantiate(waystationInfoPrefab) as GameObject;
				waystationInfo.transform.parent = StrategyMapUIController.reference.transform;
				waystationInfo.GetComponent<WaystationInfoPanel>().SetReference(this);
				waystationInfo.transform.localScale = new Vector3(1,1,1);
				waystationInfo.transform.localPosition = StrategyMapUIController.reference.waystationInfoPanel.transform.localPosition;
			}
			else
			{
				Destroy (waystationInfo);
				waystationInfo = null;
			}
		}
	}
}
