using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class StrategyMapUIController : MonoBehaviour {

	public static StrategyMapUIController reference = null;

	public GameObject currentTown;
	public GameObject currentTownFrame;
	public GameObject destinationTown;
	public GameObject currentRoad;
	GameObject potentialTown = null;
	float potentialTravelTime = 0;
	public float travelTime = 0;
	public Transform roads;
	public GameObject roadProgressbar;
	public UILabel travelTimer;

	public void LoadInfo()
	{
		destinationTown = null;
		currentRoad = null;
		foreach (ObjectInfo info in objectInfoPanels) {
			if (info.reference != null)
				Destroy(info.reference);
		}
	}

	public string GetDestinationTownName()
	{
		return destinationTown.GetComponent<StrategyMapTownInfo> ().name;
	}

	public void GoBack()
	{
		GlobalUI.reference.GoBack ();
	}

	void OnEnable()
	{
		reference = this;
	}

    GameObject IsRoadBetween(GameObject firstTown, GameObject secondTown)
	{
		if (firstTown == null || secondTown == null)
			return null;
		for (int roadIndex = 0; roadIndex < roads.childCount; roadIndex++)
		{
			StrategyMapUIRoad road = roads.GetChild(roadIndex).GetComponent<StrategyMapUIRoad>();
			if (road.firstTown != firstTown.transform && road.firstTown != secondTown.transform)
				continue;
			if (road.secondTown != firstTown.transform && road.secondTown != secondTown.transform)
				continue;
			return road.gameObject;
		}
		return null;
	}
	Vector3 GetRoadRotation(GameObject road)
	{
		Vector3 rotation = road.transform.localEulerAngles;
		if (road.GetComponent<StrategyMapUIRoad> ().firstTown.gameObject != currentTown)
			rotation.z += 180;
		return rotation;
	}

	public void SelectDestinationTown(GameObject town)
	{
		if (destinationTown == town)
			return;
		GameObject road = IsRoadBetween (currentTown, town);
		if (road != null)
		{

			if (roadProgressbar != null)
			{
				roadProgressbar.transform.position = currentTown.transform.position;
				roadProgressbar.transform.localEulerAngles = GetRoadRotation(road);
				roadProgressbar.GetComponent<UISprite>().width = road.GetComponent<StrategyMapUIRoad>().roadLength;
				roadProgressbar.SetActive(true);
				if (potentialTown != town)
				{
					roadProgressbar.GetComponent<UISlider>().value = 0;
				}
			}
			isSelecting = true;
			potentialTown = town;
			potentialTravelTime = road.GetComponent<StrategyMapUIRoad>().travelTime;
		}
		else
		{
			Debug.Log ("town is out of range");
		}
	}

	public void ResetDestinationTown()
	{
		isSelecting = false;

	}

	bool isSelecting = false;
	bool IsSelectingTown()
	{
		return Input.GetMouseButton (0);
	}

	void SetCurrentRoad()
	{
		float potentialTravelTime = 0;
		GameObject potentialRoad = IsRoadBetween(currentTown, destinationTown);
		if (currentRoad != null && currentRoad != potentialRoad)
		{
			for (int objectIndex = 0; objectIndex < currentRoad.transform.childCount; objectIndex++)
			{
				Transform roadObject = currentRoad.transform.GetChild(objectIndex);
				roadObject.GetChild(0).gameObject.SetActive(false);
			}
		}
		currentRoad = potentialRoad;
		if (currentRoad != null)
		{
			for (int objectIndex = 0; objectIndex < currentRoad.transform.childCount; objectIndex++)
			{
				Transform roadObject = currentRoad.transform.GetChild(objectIndex);
				roadObject.GetChild(0).gameObject.SetActive(true);
				potentialTravelTime += roadObject.GetComponent<StrategyMapRoadObject>().timeToLastObject;
				roadObject.GetChild(0).GetComponent<UILabel>().text = FormatTime(potentialTravelTime) + " - ";
				potentialTravelTime += roadObject.GetComponent<StrategyMapRoadObject>().timeOnObject;
				roadObject.GetChild(0).GetComponent<UILabel>().text += FormatTime(potentialTravelTime);
			}
		}
	}

	public int waystationInfoCount = 0;
	//bool showWaystationInfo = false;
	public GameObject waystationInfoPanel;
	public ObjectInfo[] objectInfoPanels = new ObjectInfo[4];

	[System.Serializable]
	public class ObjectInfo
	{
		public Vector3 position;
		public GameObject reference;
	}
	/*public void ToggleWaystaytionInfo(StrategyMapRoadObject objectInfo)
	{
		if (waystationInfoPanel.GetComponent<WaystationInfoPanel> ().reference != objectInfo)
			showWaystationInfo = true;
		else
		{
			waystationInfoPanel.GetComponent<WaystationInfoPanel> ().reference = null;
			showWaystationInfo = false;
		}
		if (showWaystationInfo)
		{
			waystationInfoPanel.SetActive(true);
			waystationInfoPanel.GetComponent<WaystationInfoPanel>().SetReference(objectInfo);
		}
		else
		{
			waystationInfoPanel.SetActive(false);
		}
	}*/

	string FormatTime(float time)
	{
		float seconds = time;
		int minutes = 0;
		int hours = 0;
		while(seconds >= 60)
		{
			seconds -= 60;
			minutes += 1;
			if (minutes >= 60)
			{
				minutes -= 60;
				hours += 1;
			}
		}
		string text = "";
		if (hours > 0) 
		{
			if (hours < 10)
				text += "0";
			text += hours + ":";
		}
		if (minutes < 10)
			text += "0";
		text += minutes + ":";
		if (seconds < 10)
			text += "0";
		text += (int)seconds;
		return text;
	}

	void UpdateTravelTimer()
	{
		float seconds = travelTime;
		int minutes = 0;
		int hours = 0;
		while(seconds >= 60)
		{
			seconds -= 60;
			minutes += 1;
			if (minutes >= 60)
			{
				minutes -= 60;
				hours += 1;
			}
		}
		travelTimer.text = "";
		if (hours < 10)
			travelTimer.text += "0";
		travelTimer.text += hours + ":";
		if (minutes < 10)
			travelTimer.text += "0";
		travelTimer.text += minutes + ":";
		if (seconds < 10)
			travelTimer.text += "0";
		travelTimer.text += (int)seconds;
	}

	void Update()
	{
		UpdateTravelTimer ();

		if (currentTown != null && currentTownFrame != null) 
		{
			currentTownFrame.transform.position = currentTown.transform.position;
		}
		if (potentialTown == null)
		{
			roadProgressbar.SetActive(false);
		}
		else
		{
			if (isSelecting)
			{
				roadProgressbar.GetComponent<UISlider>().value += Time.unscaledDeltaTime;
				if (roadProgressbar.GetComponent<UISlider>().value >= 1)
				{
					destinationTown = potentialTown;
					travelTime = potentialTravelTime;
					potentialTown = null;
					//currentTown = destinationTown;
					TownController.reference.levelToLoadIndex = IsRoadBetween(currentTown,destinationTown).GetComponent<StrategyMapUIRoad>().roadSceneIndex;
				}
			}
			else
			{
				roadProgressbar.GetComponent<UISlider>().value -= Time.unscaledDeltaTime;
				if (roadProgressbar.GetComponent<UISlider>().value <= 0)
				{
					potentialTown = null;
				}
			}
		}
		SetCurrentRoad ();
	}
}
