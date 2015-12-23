using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	public UILabel statsLabel;
	public DescriptionWindowController DescriptionWindow;
	public DescriptionWindowController CompareWindow;
	static InventoryItemObject itemToCompare = null;

	public UIStat PowerStat;
	public UIStat WeightStat;
	public UIStat MagicPowerStat;
	public UIStat SpeedStat;
	public List<WagonUIStat> WagonsStat = new List<WagonUIStat>();

	public UIScrollView inventoryScrollView;

	[System.Serializable]
	public class WagonUIStat
	{
		public UILabel description;
		public UIStat Attraction;
		public UIStat Passengers;
	}

	public void GoBack()
	{
		GlobalUI.reference.GoBack ();
	}

	bool IsShowStatInfo(float stat)
	{
		if (stat != 0)
			return true;
		return false;
	}

	public static void SetItemToCompare(InventoryItemObject item)
	{
		//Debug.Log ("item to compare set");
		itemToCompare = item;
	}
	public static void ResetItemToCompare()
	{
		//Debug.Log ("item to compare set");
		itemToCompare = null;
	}

	public void ShowItemDescription(InventoryItemObject itemObject)
	{
		DescriptionWindow.ShowItemDescription (itemObject);
		if (itemToCompare != null)
			CompareWindow.ShowItemDescription (itemToCompare, DescriptionWindow.transform.localPosition);
		
	}

	public void HideItemDescription()
	{
		DescriptionWindow.HideItemDescription ();
		CompareWindow.HideItemDescription();

	}

	public void PrintStats()
	{
		SpeedStat.description.text = "Speed:";
		float currentSpeed = PlayerSaveData.reference.trainData.GetCurrentSpeed ();
		if (PlayerTrain.reference != null) {
			currentSpeed *= (100 - PlayerTrain.reference.speedDebuffPercent) / 100f;
		}
		SpeedStat.value.text = (Mathf.Round(currentSpeed * 10f)/10f).ToString();

		float currentPenalty = PlayerSaveData.reference.trainData.GetSpeedPenalty ();
		if (PlayerTrain.reference != null) {
			currentPenalty += PlayerSaveData.reference.trainData.GetCurrentSpeed () * (PlayerTrain.reference.speedDebuffPercent / 100f);
		}
		if (currentPenalty > 0)
		{
			SpeedStat.penalty.color = Color.red;
			SpeedStat.penalty.text = "(-" + (Mathf.Round(currentPenalty * 10f)/10f).ToString() + ")";
		}
		else
		{
			SpeedStat.penalty.text = "";
		}

		PowerStat.description.text = "Power:";
		PowerStat.value.text = PlayerSaveData.reference.trainData.power.ToString();
		PowerStat.penalty.text = "";

		WeightStat.description.text = "Weight:";
		WeightStat.value.text = PlayerSaveData.reference.trainData.currentWeight.ToString()+"/"+PlayerSaveData.reference.trainData.maxWeight.ToString();
		WeightStat.penalty.text = "";

		MagicPowerStat.description.text = "Magic Power:";
		MagicPowerStat.value.text = PlayerSaveData.reference.trainData.magicPower.ToString();
		MagicPowerStat.penalty.text = "";

		int wagonIndex = 0;
		foreach(WagonUIStat wagonStat in WagonsStat)
		{
			wagonStat.description.text = "Wagon " + (wagonIndex + 1);

			wagonStat.Attraction.value.text = PlayerSaveData.reference.wagonData[wagonIndex].attraction.ToString();
			wagonStat.Passengers.value.text = PlayerSaveData.reference.wagonData[wagonIndex].currentPassengersCount.ToString() + "/" + PlayerSaveData.reference.wagonData[wagonIndex].maxPassengersCount.ToString();

			wagonIndex += 1;
		}

	}

	bool IsHoveringItem()
	{
		if (UICamera.hoveredObject == null)
			return false;
		if (UICamera.hoveredObject.GetComponent<InventoryItemObject> () == null)
			return false;
		return true;
	}

	void Update()
	{
		PrintStats ();

		if (IsHoveringItem())
		{
			if (Input.GetMouseButton (0)) {
				HideItemDescription();
				return;
			}
			ShowItemDescription(UICamera.hoveredObject.GetComponent<InventoryItemObject>());
		}
		else
		{
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1))
				ResetItemToCompare ();
			HideItemDescription();
		}

	}
}
