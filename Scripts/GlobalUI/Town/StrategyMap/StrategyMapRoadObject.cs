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
				int freePanelIndex = 0;
				foreach (StrategyMapUIController.ObjectInfo info in StrategyMapUIController.reference.objectInfoPanels)
				{
					if (info.reference != null)
					{
						freePanelIndex += 1;
						continue;
					}
					break;
				}
				if (freePanelIndex >= StrategyMapUIController.reference.objectInfoPanels.Length)
				{
					Debug.Log ("no free info panels");
					return;
				}

				waystationInfo = Instantiate(waystationInfoPrefab) as GameObject;
				waystationInfo.transform.parent = StrategyMapUIController.reference.transform;
				waystationInfo.GetComponent<WaystationInfoPanel>().SetReference(this, StrategyMapUIController.reference.waystationInfoCount*3);
				waystationInfo.transform.localScale = new Vector3(1,1,1);
				waystationInfo.transform.localPosition = StrategyMapUIController.reference.objectInfoPanels[freePanelIndex].position;
				StrategyMapUIController.reference.objectInfoPanels[freePanelIndex].reference = waystationInfo;
			}
			else
			{
				Destroy (waystationInfo);
				waystationInfo = null;
			}
		}
	}
}
