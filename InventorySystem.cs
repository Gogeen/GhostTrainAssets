using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour {

	public static InventorySystem reference = null;
	public Camera UICamera;
	public GameObject baseItem;


	public List<WagonInventoryUI> wagonUIs = new List<WagonInventoryUI> ();



	public void ToggleUI()
	{
		if (gameObject.activeSelf)
			GameController.Resume ();
		else
			GameController.Pause ();
		gameObject.SetActive (!gameObject.activeSelf);
	}

	void ApplyTrainStats(InventoryItem item)
	{
		PlayerSaveData.reference.trainData.maxCrewCount += item.costInfo.crewSpace;

		PlayerSaveData.reference.trainData.power += item.bonusInfo.power;
		PlayerSaveData.reference.trainData.magicPower += item.bonusInfo.magicPower;
		PlayerSaveData.reference.trainData.maxWeight += item.bonusInfo.maxWeight;
		PlayerSaveData.reference.trainData.maxCrewCount += item.bonusInfo.maxCrewSpace;
		PlayerSaveData.reference.trainData.maxSpeed += item.bonusInfo.maxSpeed;
	}

	void RemoveTrainStats(InventoryItem item)
	{
		PlayerSaveData.reference.trainData.maxCrewCount -= item.costInfo.crewSpace;

		PlayerSaveData.reference.trainData.power -= item.bonusInfo.power;
		PlayerSaveData.reference.trainData.magicPower -= item.bonusInfo.magicPower;
		PlayerSaveData.reference.trainData.maxWeight -= item.bonusInfo.maxWeight;
		PlayerSaveData.reference.trainData.maxCrewCount -= item.bonusInfo.maxCrewSpace;
		PlayerSaveData.reference.trainData.maxSpeed -= item.bonusInfo.maxSpeed;
	}

	void ApplyWagonStats(InventoryItem item, int wagonIndex)
	{
		PlayerSaveData.reference.wagonData[wagonIndex].attraction += item.bonusInfo.attraction;
		PlayerSaveData.reference.wagonData[wagonIndex].currentPassengersCount += item.costInfo.passengerSpace;
		PlayerSaveData.reference.wagonData[wagonIndex].maxPassengersCount += item.bonusInfo.maxPassengerSpace;
	}

	void RemoveWagonStats(InventoryItem item, int wagonIndex)
	{
		PlayerSaveData.reference.wagonData[wagonIndex].attraction -= item.bonusInfo.attraction;
		PlayerSaveData.reference.wagonData[wagonIndex].currentPassengersCount -= item.costInfo.passengerSpace;
		PlayerSaveData.reference.wagonData[wagonIndex].maxPassengersCount -= item.bonusInfo.maxPassengerSpace;
	}

	public void InitItem(InventoryItem item, int wagonIndex = -1, int slotIndex = -1)
	{
		GameObject UIItem = Instantiate (baseItem) as GameObject;
		UIItem.GetComponent<Item> ().reference = new InventoryItem(item);
		UIItem.GetComponent<ItemDragDropScript> ().uiCamera = UICamera;

		//!!! put item in available inventory slot
		PutUIItemInSlot (UIItem, wagonIndex, slotIndex);

		PlayerSaveData.reference.trainData.currentWeight += item.costInfo.weight;
		if (item.type == InventoryItem.Type.NonEquippable)
		{
			ApplyTrainStats(item);
			/*
			if (item.bonusInfo.equipmentDurability != 0)
			{
				foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
				{
					if (equippedItem == null)
						continue;
					equippedItem.durabilityInfo.max += item.bonusInfo.equipmentDurability;
					equippedItem.durabilityInfo.current += item.bonusInfo.equipmentDurability;
				}
			}
			*/
			return;
		}
	}

	public void MoveItem(InventoryItem item, WagonInventoryUI fromWagon, WagonInventoryUI toWagon)
	{
		ApplyWagonStats (item, wagonUIs.IndexOf(toWagon));
		RemoveWagonStats (item, wagonUIs.IndexOf(fromWagon));
	}

	void SetupUIItem(GameObject item, int finalWagonIndex, int finalSlotIndex)
	{
		ApplyWagonStats (item.GetComponent<Item>().reference, finalWagonIndex);

		item.GetComponent<Item> ().wagonIndex = finalWagonIndex;
		item.GetComponent<Item> ().slotIndex = finalSlotIndex;
		wagonUIs[finalWagonIndex].InitItem(item);
	}

	void PutUIItemInSlot(GameObject item, int wagonIndex = -1, int slotIndex = -1)
	{

		if (wagonIndex > -1 && wagonIndex < wagonUIs.Count)
		{
			if (slotIndex > -1 && slotIndex < wagonUIs[wagonIndex].slots.Count)
			{
				if (wagonUIs[wagonIndex].IsEmptySlotForItem(slotIndex, item))
				{
					// all right
					SetupUIItem(item, wagonIndex, slotIndex);
					return;
				}
			}
			int emptySlotIndex = wagonUIs[wagonIndex].FindEmptySlotForItem(item);
			if (emptySlotIndex != -1)
			{
				// all right
				SetupUIItem(item, wagonIndex, emptySlotIndex);
				return;
			}
		}
		// find an empty slot
		for (int checkingWagonIndex = 0;  checkingWagonIndex < wagonUIs.Count; checkingWagonIndex++)
		{
			int emptySlotIndex = wagonUIs[checkingWagonIndex].FindEmptySlotForItem(item);
			if (emptySlotIndex != -1)
			{
				SetupUIItem(item, checkingWagonIndex, emptySlotIndex);

				return;
			}
		}
		Debug.Log ("no inventory space for item");
	}

	public bool IsEquipped(InventoryItem item)
	{
		switch(item.type)
		{
		case InventoryItem.Type.Slot1: 
		{
			return item == PlayerSaveData.reference.trainData.equippedItems[0];
		}
		case InventoryItem.Type.Slot2: 
		{
			return item == PlayerSaveData.reference.trainData.equippedItems[1];
		}
		case InventoryItem.Type.Slot3: 
		{
			return item == PlayerSaveData.reference.trainData.equippedItems[2];
		}
		}
		return false;
	}

	public void Equip(InventoryItem item)
	{
		if (item.type == InventoryItem.Type.NonEquippable)
		{
			Debug.Log ("Item cant be equipped!");
			return;
		}
		ApplyTrainStats (item);

		switch (item.type)
		{
		case InventoryItem.Type.Slot1: 
		{
			if (PlayerSaveData.reference.trainData.equippedItems[0] != null)
			{
				Unequip(0);
			} 
			PlayerSaveData.reference.trainData.equippedItems[0] = item; 
			break;
		}
		case InventoryItem.Type.Slot2: 
		{
			if (PlayerSaveData.reference.trainData.equippedItems[1] != null)
			{
				Unequip(1);
			} 
			PlayerSaveData.reference.trainData.equippedItems[1] = item; 
			break;
		}
		case InventoryItem.Type.Slot3: 
		{
			if (PlayerSaveData.reference.trainData.equippedItems[2] != null)
			{
				Unequip(2);
			} 
			PlayerSaveData.reference.trainData.equippedItems[2] = item; 
			break;
		}
		}
		
		if (item.bonusInfo.equipmentDurability != 0)
		{
			foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
			{
				if (equippedItem == null)
					continue;
				equippedItem.durabilityInfo.max += item.bonusInfo.equipmentDurability;
				equippedItem.durabilityInfo.current += item.bonusInfo.equipmentDurability;
			}
		}
		foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
		{
			if (equippedItem == null)
				continue;
			if (equippedItem.bonusInfo.equipmentDurability != 0)
			{
				item.durabilityInfo.max += equippedItem.bonusInfo.equipmentDurability;
				item.durabilityInfo.current += equippedItem.bonusInfo.equipmentDurability;
			}
		}
		//!!! put item in specific equipment slot

	}

	public void Unequip(int slotIndex)
	{
		Debug.Log (PlayerSaveData.reference.trainData.equippedItems [slotIndex]);
		InventoryItem item = PlayerSaveData.reference.trainData.equippedItems [slotIndex];
		
		RemoveTrainStats (item);

		if (item.bonusInfo.equipmentDurability != 0)
		{
			foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
			{
				if (equippedItem == null)
					continue;
				equippedItem.durabilityInfo.current -= item.bonusInfo.equipmentDurability;
				equippedItem.durabilityInfo.max -= item.bonusInfo.equipmentDurability;
			}
		}
		foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
		{
			if (equippedItem == null)
				continue;
			if (equippedItem.bonusInfo.equipmentDurability != 0)
			{
				item.durabilityInfo.max -= equippedItem.bonusInfo.equipmentDurability;
				item.durabilityInfo.current -= equippedItem.bonusInfo.equipmentDurability;
			}
		}
		PlayerSaveData.reference.trainData.equippedItems[slotIndex] = null;
		//!!! put item in available inventory slot
	}
}
