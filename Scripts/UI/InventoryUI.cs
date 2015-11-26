using UnityEngine;
using System.Collections;

public class InventoryUI : MonoBehaviour {

	public UILabel statsLabel;
	public GameObject DescriptionWindow;

	bool IsShowStatInfo(float stat)
	{
		if (stat != 0)
			return true;
		return false;
	}
	public void ShowItemDescription(InventoryItem item)
	{
		DescriptionWindow.SetActive (true);
		UILabel descriptionLabel = DescriptionWindow.transform.GetChild (0).GetComponent<UILabel> ();

		descriptionLabel.text = "";
		descriptionLabel.text += item.uiInfo.description + "\n\n";


		descriptionLabel.text += "Weight: " + item.costInfo.weight + "\n"; 
		descriptionLabel.text += "Passenger Space: " + item.costInfo.passengerSpace + "\n";

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

		descriptionLabel.text += "Durability: " + (item.durabilityInfo.current/item.durabilityInfo.max)*100 + "%"; 

	}

	public void HideItemDescription()
	{
		DescriptionWindow.SetActive (false);
	}

	public void PrintStats()
	{
		statsLabel.text = "";
		statsLabel.text += "Power/Weight/Max Weight: " + 
			PlayerSaveData.reference.trainData.power + "/" + 
			PlayerSaveData.reference.trainData.currentWeight + "/" + 
			PlayerSaveData.reference.trainData.maxWeight + "\n";

		statsLabel.text += "Attraction/Passengers/Max Passengers: " + "\n";
		foreach (PlayerSaveData.WagonData wagonData in PlayerSaveData.reference.wagonData)
		{
			statsLabel.text += "Wagon " + (PlayerSaveData.reference.wagonData.IndexOf(wagonData) + 1) + ": " +
				wagonData.attraction + "/" + 
				wagonData.currentPassengersCount + "/" + 
				wagonData.maxPassengersCount + "\n";
		}

		statsLabel.text += "Magic Power: " + PlayerSaveData.reference.trainData.magicPower + "\n";
		//statsLabel.text += "Crew: " + PlayerSaveData.reference.trainData.currentCrewCount + "/" + PlayerSaveData.reference.trainData.maxCrewCount + "\n";
		statsLabel.text += "Speed: " + PlayerSaveData.reference.trainData.GetCurrentSpeed() + "/" + PlayerSaveData.reference.trainData.maxSpeed + "\n";
		/*
		PlayerSaveData.reference.trainData.magicPower;
		PlayerSaveData.reference.trainData.currentWeight;
		PlayerSaveData.reference.trainData.maxWeight;
		PlayerSaveData.reference.trainData.currentCrewCount;
		PlayerSaveData.reference.trainData.maxCrewCount;
		PlayerSaveData.reference.trainData.maxSpeed;

		PlayerSaveData.reference.wagonData[].attraction;
		PlayerSaveData.reference.wagonData[].currentPassengersCount;
		PlayerSaveData.reference.wagonData[].maxPassengersCount;
		*/
	}

	void Update()
	{
		PrintStats ();

		if (UICamera.hoveredObject.GetComponent<Item>())
		{
			ShowItemDescription(UICamera.hoveredObject.GetComponent<Item>().reference);
		}
		else
		{
			HideItemDescription();
		}
	}
}
