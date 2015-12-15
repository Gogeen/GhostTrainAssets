using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	public UILabel statsLabel;
	public GameObject DescriptionWindow;
	public GameObject CompareWindow;
	static Item itemToCompare = null;

	public UIStat PowerStat;
	public UIStat WeightStat;
	public UIStat MagicPowerStat;
	public UIStat SpeedStat;
	public List<WagonUIStat> WagonsStat = new List<WagonUIStat>();
	
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

	public static void SetItemToCompare(Item item)
	{
		//Debug.Log ("item to compare set");
		itemToCompare = item;
	}

	public void ShowItemDescription(Item UIitem, bool isCompareWindow = false)
	{
		InventoryItem item = UIitem.reference;

		UILabel descriptionLabel;
		if (isCompareWindow)
		{
			CompareWindow.SetActive(true);
			descriptionLabel = CompareWindow.transform.GetChild (0).GetComponent<UILabel> ();
		}
		else
		{
			DescriptionWindow.SetActive (true);
			descriptionLabel = DescriptionWindow.transform.GetChild (0).GetComponent<UILabel> ();
			if (itemToCompare != null)
				ShowItemDescription(itemToCompare, true);
		}
		descriptionLabel.text = "";

		descriptionLabel.text += "Weight: " + item.costInfo.weight + "\n"; 
		if (IsShowStatInfo(item.costInfo.passengerSpace))
			descriptionLabel.text += "Passenger Space: " + item.costInfo.passengerSpace + "\n";

		descriptionLabel.text += "Durability: " + (item.durabilityInfo.current/item.durabilityInfo.max)*100 + "%" + "\n"; 

		if (IsShowStatInfo(InventorySystem.reference.GetItemTotalPrice(UIitem)))
			descriptionLabel.text += "Price: " + InventorySystem.reference.GetItemTotalPrice(UIitem) + "\n";


		if (IsShowStatInfo(item.bonusInfo.power))
			descriptionLabel.text += "Bonus Power: " + item.bonusInfo.power + "\n"; 
		if (IsShowStatInfo(item.bonusInfo.maxWeight))
			descriptionLabel.text += "Bonus Max Weight: " + item.bonusInfo.maxWeight + "\n";
		if (IsShowStatInfo(item.bonusInfo.attraction))
			descriptionLabel.text += "Bonus Attraction: " + item.bonusInfo.attraction + "\n";
		if (IsShowStatInfo(item.bonusInfo.maxPassengerSpace))
			descriptionLabel.text += "Bonus Max Passenger Space: " + item.bonusInfo.maxPassengerSpace + "\n";
		if (IsShowStatInfo(item.bonusInfo.magicPower))
			descriptionLabel.text += "Bonus Magic Power: " + item.bonusInfo.magicPower + "\n";
		if (IsShowStatInfo(item.bonusInfo.maxSpeed))
			descriptionLabel.text += "Bonus Max Speed: " + item.bonusInfo.maxSpeed + "\n";
		if (IsShowStatInfo(item.bonusInfo.equipmentDurability))
			descriptionLabel.text += "Bonus Equipment Durability: " + item.bonusInfo.equipmentDurability + "\n";


		descriptionLabel.text += "\n" + item.uiInfo.description;

		if (isCompareWindow)
		{
			CompareWindow.transform.position = DescriptionWindow.transform.position;
			CompareWindow.transform.localPosition -= new Vector3(CompareWindow.transform.FindChild("Background").GetComponent<UISprite>().localSize.x,0,0);
		}
		else
		{
			// set window near mouse
			DescriptionWindow.transform.position = UICamera.lastHit.point;
			DescriptionWindow.transform.localPosition += new Vector3 (10,-10,0);
			DescriptionWindow.transform.localPosition += new Vector3 (0,DescriptionWindow.transform.FindChild("Background").GetComponent<UISprite>().localSize.y,0);

			UIRoot root = null;
			Transform currentObject = transform;
			while (true) 
			{
				if (currentObject.GetComponent<UIRoot>())
				{
					root = currentObject.GetComponent<UIRoot>();
					break;
				}
				currentObject = currentObject.parent;
			}
			//Debug.Log (root.manualWidth);
			if (DescriptionWindow.transform.localPosition.x - 10 + DescriptionWindow.transform.FindChild("Background").GetComponent<UISprite>().localSize.x > root.manualWidth/2)
			{
				DescriptionWindow.transform.localPosition += new Vector3 (-DescriptionWindow.transform.FindChild("Background").GetComponent<UISprite>().localSize.x,0,0);
			}
			if (DescriptionWindow.transform.localPosition.y + 10 > root.manualHeight/2)
			{
				DescriptionWindow.transform.localPosition += new Vector3 (0,-DescriptionWindow.transform.FindChild("Background").GetComponent<UISprite>().localSize.y,0);
			}
		}
	}

	public void HideItemDescription()
	{
		DescriptionWindow.SetActive (false);
		CompareWindow.SetActive(false);

	}

	public void PrintStats()
	{
		SpeedStat.description.text = "Speed:";
		SpeedStat.value.text = PlayerSaveData.reference.trainData.GetCurrentSpeed().ToString();
		if (PlayerSaveData.reference.trainData.GetSpeedPenalty() > 0)
		{
			SpeedStat.penalty.color = Color.red;
			SpeedStat.penalty.text = "(-" + PlayerSaveData.reference.trainData.GetSpeedPenalty().ToString() + ")";
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
		if (UICamera.hoveredObject.GetComponent<Item> () == null)
			return false;
		return true;
	}

	void Update()
	{
		PrintStats ();

		if (IsHoveringItem())
		{
			ShowItemDescription(UICamera.hoveredObject.GetComponent<Item>());
		}
		else
		{
			HideItemDescription();
		}
	}
}
