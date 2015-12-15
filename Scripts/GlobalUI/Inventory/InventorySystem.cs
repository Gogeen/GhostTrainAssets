using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour {

	public static InventorySystem reference = null;
	public Camera UICamera;
	public GameObject baseItem;

	public InventoryEquipmentUI equipmentUI;
	public List<WagonInventoryUI> wagonUIs = new List<WagonInventoryUI> ();
	public GameObject shopUI; 
	public WagonInventoryUI shopInventory;

	public int synergyModifier;

	bool selectingItemToRepair = false;

	void CheckSynergy(int wagonIndex)
	{
		int synergyItemsCount = 0;
		foreach(Transform slot in wagonUIs[wagonIndex].slots)
		{
			if (wagonUIs[wagonIndex].GetUIItemInSlot(slot) != null)
			{
				if (wagonUIs[wagonIndex].GetUIItemInSlot(slot).reference.bonusInfo.attraction > 0)
				{
					synergyItemsCount += 1;
				}
				else
				{
					RemoveSynergyEffect(wagonIndex);
					//Debug.Log ("synergy doesn't work, incorrect items in inventory");
					return;
				}
			}
		}
		ApplySynergyEffect (wagonIndex, synergyItemsCount);
	}

	void ApplySynergyEffect(int wagonIndex, int itemsCount)
	{
		PlayerSaveData.reference.wagonData [wagonIndex].attraction -= PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction;
		PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction = itemsCount * synergyModifier;
		PlayerSaveData.reference.wagonData [wagonIndex].attraction += PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction;

	}

	void RemoveSynergyEffect(int wagonIndex)
	{
		PlayerSaveData.reference.wagonData [wagonIndex].attraction -= PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction;
		PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction = 0;

	}

	public void RepairWholeInventory()
	{
		foreach(PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData)
		{
			foreach(Item item in wagon.items)
			{
				RepairItem(item.reference);
			}
		}
		foreach(InventoryItem item in PlayerSaveData.reference.trainData.equippedItems)
		{
			RepairItem(item);
		}
	}

	public void RepairItem(InventoryItem item)
	{
		// let repair cost be 1 sec for now
		if (TrainTimeScript.reference.HaveEnoughTime(1))
		{
			item.durabilityInfo.current = item.durabilityInfo.max;
			TrainTimeScript.reference.AddTime (-1);
		}
		selectingItemToRepair = false;
	}

	public bool IsSelectingItemToRepair()
	{
		return selectingItemToRepair;
	}

	public void SelectItemToRepair()
	{
		selectingItemToRepair = true;
	}

	public void BreakItem(InventoryItem item, float value)
	{
		item.durabilityInfo.current -= value;
		if (item.durabilityInfo.current < 0)
			item.durabilityInfo.current = 0;
		Debug.Log ("breaking "+item.name+" for "+value);
	}

	public void BreakWholeInventory(float value)
	{
		foreach (WagonInventoryUI wagonUI in wagonUIs)
		{
			foreach(Transform slot in wagonUI.slots)
			{
				if (wagonUI.GetUIItemInSlot(slot) != null)
					BreakItem(wagonUI.GetUIItemInSlot(slot).reference, value);
			}
		}
		foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
		{
			if (equippedItem != null)
				BreakItem(equippedItem, value);
		}
	}

	public InventoryItem GetRandomItem()
	{
		// get total items count
		int itemsCount = 0;
		foreach (WagonInventoryUI wagonUI in wagonUIs)
		{
			foreach(Transform slot in wagonUI.slots)
			{
				if (wagonUI.GetUIItemInSlot (slot) != null)
					itemsCount += 1;
			}
		}
		foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
		{
			if (equippedItem != null)
				itemsCount += 1;
		}
		// get random item
		int itemIndexToBreak = Random.Range (0, itemsCount);
		foreach (WagonInventoryUI wagonUI in wagonUIs)
		{
			foreach(Transform slot in wagonUI.slots)
			{
				if (wagonUI.GetUIItemInSlot (slot) != null) {
					if (itemIndexToBreak > 0) {
						itemIndexToBreak -= 1;
						continue;
					} else {
						return wagonUI.GetUIItemInSlot (slot).reference;
					}
				}
			}
		}
		foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
		{
			if (equippedItem != null) {
				if (itemIndexToBreak > 0) {
					itemIndexToBreak -= 1;
					continue;
				} else {
					return equippedItem;
				}
			}
		}
		Debug.Log ("somesting wrong in this function");
		return null;
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

	public void Clear()
	{
		foreach (WagonInventoryUI wagonUI in wagonUIs)
		{
			foreach(Transform slot in wagonUI.slots)
			{
				if (wagonUI.GetUIItemInSlot(slot) != null)
				{
					InventoryItem item = wagonUI.GetUIItemInSlot(slot).reference;
					if (item.type == InventoryItem.Type.NonEquippable)
					{
						RemoveTrainStats(item);
					}
					Destroy(wagonUI.GetUIItemInSlot(slot).gameObject);
				}
					
			}
		}
		foreach(InventoryEquipmentUI.SlotInfo info in equipmentUI.slots)
		{
			Transform slot = info.slot;
			if (equipmentUI.GetUIItemInSlot(slot) != null)
			{
				InventoryItem item = equipmentUI.GetUIItemInSlot(slot).reference;
				RemoveTrainStats(item);
				Destroy(equipmentUI.GetUIItemInSlot(slot).gameObject);
			}
		}
	}

	public void InitItem(InventoryItem item, bool isEquip, int wagonIndex = -1, int slotIndex = -1)
	{
		GameObject UIItem = Instantiate (baseItem) as GameObject;
		UIItem.GetComponent<Item> ().reference = new InventoryItem(item);
		UIItem.GetComponent<ItemDragDropScript> ().uiCamera = UICamera;

		//!!! put item in available inventory slot
		if (!isEquip)
			PutUIItemInSlot (UIItem, wagonIndex, slotIndex);
		else
		{
			Equip (UIItem.GetComponent<Item> ().reference);
			equipmentUI.InitItem(UIItem);
		}

		PlayerSaveData.reference.trainData.currentWeight += item.costInfo.weight;
		if (item.type == InventoryItem.Type.NonEquippable)
		{
			ApplyTrainStats(item);
		}
	}

	public void MoveItem(InventoryItem item, WagonInventoryUI fromWagon, WagonInventoryUI toWagon)
	{
		if (toWagon != shopInventory)
		{
			ApplyWagonStats (item, wagonUIs.IndexOf(toWagon));
			CheckSynergy(wagonUIs.IndexOf(toWagon));
		}
		if (fromWagon != shopInventory)
		{
			RemoveWagonStats (item, wagonUIs.IndexOf(fromWagon));
			CheckSynergy(wagonUIs.IndexOf(fromWagon));
		}
	}

	void SetupUIItem(GameObject item, int finalWagonIndex, int finalSlotIndex)
	{
		ApplyWagonStats (item.GetComponent<Item>().reference, finalWagonIndex);
		CheckSynergy(finalWagonIndex);

		item.GetComponent<Item> ().wagonIndex = finalWagonIndex;
		item.GetComponent<Item> ().slotIndex = finalSlotIndex;
		wagonUIs[finalWagonIndex].InitItem(item);
		PlayerSaveData.reference.wagonData [finalWagonIndex].items.Add (item.GetComponent<Item> ());
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

	public bool IsShopActive()
	{
		return shopUI.activeSelf;
	}

	public void ToggleShopUI()
	{
		shopUI.SetActive (!shopUI.activeSelf);
	}

	VendorShop vendorShopInfo;
	public void LoadShopInfo(VendorShop info)
	{
		vendorShopInfo = info;

	}

	public void OpenShop()
	{
		foreach(VendorShop.ItemInfo info in vendorShopInfo.items)
		{
			for (int itemCounter = 0; itemCounter < info.quantity; itemCounter++)
			{
				InventoryItem item = ItemDatabase.reference.FindByIndex(info.index);
				InitShopItem (item); // need to be initialized in shop inventory, not player inventory
			}
		}
		if (!IsShopActive ())
			ToggleShopUI ();
	}

	public void CloseShop()
	{
		shopInventory.Clear ();
		if (IsShopActive ())
			ToggleShopUI ();
	}

	public void InitShopItem(InventoryItem item)
	{
		GameObject UIItem = Instantiate (baseItem) as GameObject;
		UIItem.GetComponent<Item> ().reference = new InventoryItem(item);
		UIItem.GetComponent<ItemDragDropScript> ().uiCamera = UICamera;
		
		PutUIItemInShopSlot (UIItem);
	}

	void PutUIItemInShopSlot(GameObject item)
	{
		int emptySlotIndex = shopInventory.FindEmptySlotForItem(item);
		if (emptySlotIndex != -1)
		{
			item.GetComponent<Item> ().slotIndex = emptySlotIndex;
			shopInventory.InitItem(item);
			return;
		}

		Debug.Log ("no shop space for item");
	}

	public int GetItemTotalPrice(Item item)
	{
		if (!IsShopActive ())
		{
			//Debug.Log ("shop is not active");
			return 0;
		}
		Transform slot = item.transform.parent.parent;
		WagonInventoryUI inventory = slot.parent.parent.GetComponent<WagonInventoryUI>();
		if (inventory == shopInventory) 
		{
			float buyMod = vendorShopInfo.FindInfoByIndex (item.reference.databaseIndex).buyModifier;
			return (int)(item.reference.costInfo.timePrice * buyMod);
		}
		else
		{
			if (vendorShopInfo.FindInfoByIndex (item.reference.databaseIndex) != null)
			{
				float sellMod = vendorShopInfo.FindInfoByIndex (item.reference.databaseIndex).sellModifier;
				return (int)(item.reference.costInfo.timePrice * sellMod);
			}
			else
			{
				//Debug.Log ("vendor is not interested");
				return 0;
			}
		}
	}

	public void BuyItem(InventoryItem item)
	{
		float buyMod = vendorShopInfo.FindInfoByIndex (item.databaseIndex).buyModifier;
		int totalPrice = (int)(item.costInfo.timePrice * buyMod);
		TrainTimeScript.reference.AddTime (-totalPrice);
	}

	public void SellItem(InventoryItem item)
	{
		float sellMod = vendorShopInfo.FindInfoByIndex (item.databaseIndex).sellModifier;
		int totalPrice = (int)(item.costInfo.timePrice * sellMod);
		TrainTimeScript.reference.AddTime (totalPrice);
	}
}
