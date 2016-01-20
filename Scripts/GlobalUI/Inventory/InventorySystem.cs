using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class InventorySystem : MonoBehaviour {

	public enum SlotType
	{
		Equipment,
		Wagon,
		Shop,
		Unknown
	}

	public class SlotInfo
	{
		public SlotType type;
		public int wagonIndex;
		public int slotIndex;
	}

	public static InventorySystem reference = null;
	public Camera UICamera;
	public GameObject baseItem;

	public InventoryEquipmentUI equipmentUI;
	public List<WagonInventoryUI> wagonUIs = new List<WagonInventoryUI> ();
	public GameObject shopUI; 
	public WagonInventoryUI shopInventory;

	public int synergyModifier;

	bool selectingItemToRepair = false;

	public Transform GetSlot(SlotType type, int index, int wagonIndex = 0)
	{
		switch (type) {
		case SlotType.Equipment:
			{return equipmentUI.slots [index].slot;}
		case SlotType.Wagon:
			{return wagonUIs[wagonIndex].slots [index];}
		case SlotType.Shop:
			{return shopInventory.slots [index];}
		}
		Debug.Log ("GetSlot Broken!");
		return null;
	}

	public SlotInfo GetSlotInfo(Transform slot)
	{
		SlotInfo slotInfo = new SlotInfo ();
		int slotIndex = 0;
		foreach (InventoryEquipmentUI.SlotInfo info in equipmentUI.slots) {
			if (slot == info.slot)
			{
				slotInfo.type = SlotType.Equipment;
				slotInfo.slotIndex = slotIndex;
				slotInfo.wagonIndex = -1;
				return slotInfo;
			}
			slotIndex += 1;
		}

		int wagonIndex = 0;
		foreach (WagonInventoryUI wagonUI in wagonUIs) {
			if (wagonUI.slots.Contains (slot)) {
				slotInfo.type =  SlotType.Wagon;
				slotInfo.slotIndex = wagonUI.slots.IndexOf(slot);
				slotInfo.wagonIndex = wagonIndex;
				return slotInfo;
			}
			wagonIndex += 1;
		}
		if (shopInventory.slots.Contains (slot)) {
			slotInfo.type =  SlotType.Shop;
			slotInfo.slotIndex = shopInventory.slots.IndexOf(slot);
			slotInfo.wagonIndex = -1;
			return slotInfo;
		}
		//Debug.Log ("GetSlotType Broken! "+slot);
		return null;
	}

	public bool IsSlotEmpty(Transform slot)
	{
		if (slot.GetComponent<UIDragDropContainer> ().reparentTarget.childCount > 0)
			return false;
		return true;
	}

	public bool CanPutInSlot(Transform slotToPut, InventoryItemObject item)
	{
		SlotInfo slotInfo = GetSlotInfo (slotToPut);
		if (slotInfo.type == SlotType.Equipment) {
			if (item.info.type == equipmentUI.GetSlotType (slotToPut))
				return true;
			return false;
		}else if (slotInfo.type == SlotType.Wagon) {
			WagonInventoryUI wagon = wagonUIs [slotInfo.wagonIndex];
			return wagon.CanPutInSlot (slotToPut,item.info.uiInfo.size);
		}else if (slotInfo.type == SlotType.Shop) {
			WagonInventoryUI wagon = shopInventory;
			return wagon.CanPutInSlot (slotToPut,item.info.uiInfo.size);
		}
		return false;
	}

	public InventoryItemObject GetItemObjectInSlot(Transform slot)
	{
		if (IsSlotEmpty (slot))
			return null;
		return slot.GetComponent<UIDragDropContainer>().reparentTarget.GetChild(0).GetComponent<InventoryItemObject>();
	}

	public enum SynergyType
	{
		Attraction,
		MagicPower
	}

	public void CheckSynergy(int wagonIndex)
	{
		//Debug.Log ("synergy check");
		// check attraction synergy
		bool isSynergy = true;
		int synergyItemsCount = 0;
		foreach(InventoryItemObject item in PlayerSaveData.reference.wagonData[wagonIndex].items)
		{
			if (item.info.bonusInfo.attraction > 0){
				synergyItemsCount += 1;
			}else{
				isSynergy = false;
				break;
			}

		}
		if (synergyItemsCount <= 1)
			isSynergy = false;
		if (isSynergy)
			ApplySynergyEffect (SynergyType.Attraction, wagonIndex, synergyItemsCount);
		else 
			RemoveSynergyEffect(SynergyType.Attraction, wagonIndex);

		// check magicPower synergy
		isSynergy = true;
		synergyItemsCount = 0;
		foreach(InventoryItemObject item in PlayerSaveData.reference.wagonData[wagonIndex].items)
		{
			//Debug.Log (item.info.name);
			if (item.info.bonusInfo.magicPower > 0){
				synergyItemsCount += 1;
			}else{
				isSynergy = false;
				break;
			}

		}
		if (synergyItemsCount <= 1)
			isSynergy = false;
		if (isSynergy)
			ApplySynergyEffect (SynergyType.MagicPower, wagonIndex, synergyItemsCount);
		else 
			RemoveSynergyEffect(SynergyType.MagicPower, wagonIndex);
		
	}

	void ApplySynergyEffect(SynergyType type, int wagonIndex, int itemsCount)
	{
		//Debug.Log ("applying synergy");
		if (type == SynergyType.Attraction) {
			PlayerSaveData.reference.wagonData [wagonIndex].attraction -= PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction;
			PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction = itemsCount * synergyModifier * PlayerSaveData.reference.wagonData [wagonIndex].attraction / 100;
			PlayerSaveData.reference.wagonData [wagonIndex].attraction += PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction;
		} else if (type == SynergyType.MagicPower) {
			PlayerSaveData.reference.trainData.magicPower -= PlayerSaveData.reference.wagonData [wagonIndex].synergyMagicPower;
			PlayerSaveData.reference.wagonData [wagonIndex].synergyMagicPower = itemsCount * synergyModifier * PlayerSaveData.reference.trainData.magicPower / 100;
			PlayerSaveData.reference.trainData.magicPower += PlayerSaveData.reference.wagonData [wagonIndex].synergyMagicPower;
		}

	}

	void RemoveSynergyEffect(SynergyType type, int wagonIndex)
	{
		if (type == SynergyType.Attraction) {
			PlayerSaveData.reference.wagonData [wagonIndex].attraction -= PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction;
			PlayerSaveData.reference.wagonData [wagonIndex].synergyAttraction = 0;
		} else if (type == SynergyType.MagicPower) {
			PlayerSaveData.reference.trainData.magicPower -= PlayerSaveData.reference.wagonData [wagonIndex].synergyMagicPower;
			PlayerSaveData.reference.wagonData [wagonIndex].synergyMagicPower = 0;
		}

	}

	public void CheckSigns()
	{
		foreach (WagonInventoryUI wagon in wagonUIs) {
			wagon.CheckSigns ();
		}
	}

	public int GetTotalRepairCost()
	{
		float totalRepairCost = 0;
		foreach(PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData){
			foreach(InventoryItemObject item in wagon.items){
				totalRepairCost += item.GetRepairCost ();
			}
		}
		foreach(InventoryItemObject item in PlayerSaveData.reference.trainData.equippedItems){
			if (item == null)
				continue;
			totalRepairCost += item.GetRepairCost ();
		}
		return (int)totalRepairCost;
	}

	public void RepairWholeInventory()
	{
		float totalRepairCost = 0;
		foreach(PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData)
		{
			foreach(InventoryItemObject item in wagon.items)
			{
				totalRepairCost += item.GetRepairCost ();
				item.Repair ();
			}
		}
		foreach(InventoryItemObject item in PlayerSaveData.reference.trainData.equippedItems)
		{
			totalRepairCost += item.GetRepairCost ();
			item.Repair ();
		}
		TrainTimeScript.reference.SimulateWaitForPassengers (totalRepairCost);
		selectingItemToRepair = false;

	}

	public void RepairWholeInventory(float value, bool isFree = false)
	{
		float totalRepairCost = 0;
		foreach(PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData)
		{
			foreach(InventoryItemObject item in wagon.items)
			{
				totalRepairCost += item.GetRepairCost ();
				item.Repair (value, isFree);
			}
		}
		foreach(InventoryItemObject item in PlayerSaveData.reference.trainData.equippedItems)
		{
			totalRepairCost += item.GetRepairCost ();
			item.Repair (value, isFree);
		}
		if (!isFree) {
			TrainTimeScript.reference.SimulateWaitForPassengers (totalRepairCost);
		}
		selectingItemToRepair = false;
	}

	public void RepairItem(InventoryItemObject item)
	{
		TrainTimeScript.reference.SimulateWaitForPassengers (item.GetRepairCost());
		item.Repair ();
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



	public InventoryItemObject FindItem(string name)
	{
		foreach (InventoryItemObject item in PlayerSaveData.reference.trainData.equippedItems) {
			if (item == null)
				continue;
			if (item.info.name == name)
				return item;
		}
		foreach (PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData) {
			foreach (InventoryItemObject item in wagon.items) {
				if (item.info.name == name)
					return item;
			}
		}
		return null;
	}

	public InventoryItemObject GetRandomItem()
	{
		// get total items count
		int itemsCount = 0;
		foreach (WagonInventoryUI wagonUI in wagonUIs)
		{
			foreach(Transform slot in wagonUI.slots)
			{
				if (GetItemObjectInSlot(slot) != null)
					itemsCount += 1;
			}
		}
		foreach (InventoryItemObject equippedItem in PlayerSaveData.reference.trainData.equippedItems)
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
				if (GetItemObjectInSlot(slot) != null) {
					if (itemIndexToBreak > 0) {
						itemIndexToBreak -= 1;
						continue;
					} else {
						return GetItemObjectInSlot(slot);
					}
				}
			}
		}
		foreach (InventoryItemObject equippedItem in PlayerSaveData.reference.trainData.equippedItems)
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

	public void Clear()
	{
		foreach (WagonInventoryUI wagonUI in wagonUIs){
			foreach(Transform slot in wagonUI.slots){
				InventoryItemObject item = GetItemObjectInSlot(slot);
				if (item != null){
					Destroy(item.gameObject);
				}	
			}
		}
		foreach(InventoryEquipmentUI.SlotInfo info in equipmentUI.slots)
		{
			Transform slot = info.slot;
			InventoryItemObject item = GetItemObjectInSlot(slot);
			if (item != null)
			{
				Destroy(item.gameObject);
			}
		}
	}

	public void InitItem(InventoryItem info, SlotType slotType, int wagonIndex = -1, int slotIndex = -1)
	{
		GameObject Item = Instantiate (baseItem) as GameObject;
		Item.GetComponent<InventoryItemObject> ().info = new InventoryItem(info);
		Item.GetComponent<ItemDragDropScript> ().uiCamera = UICamera;
		Item.GetComponent<CustomDragScrollView> ().scrollView = GetComponent<InventoryUI> ().inventoryScrollView;

		//!!! put item in available inventory slot
		if (slotType == SlotType.Wagon) {
			Transform slot = FindEmptySlot (Item.GetComponent<InventoryItemObject> (), slotType, wagonIndex, slotIndex);
			Item.GetComponent<InventoryItemObject> ().Place (slot);

			//PutUIItemInSlot (Item, wagonIndex, slotIndex);
		} else if (slotType == SlotType.Equipment) {
			Item.GetComponent<InventoryItemObject> ().Place (GetSlot (SlotType.Equipment, Item.GetComponent<InventoryItemObject> ().GetEquipmentSlotIndex ()));
			//Item.GetComponent<InventoryItemObject> ().Equip();
		} else if (slotType == SlotType.Shop) {
			Transform slot = FindEmptySlot (Item.GetComponent<InventoryItemObject> (), slotType, wagonIndex, slotIndex);
			Item.GetComponent<InventoryItemObject> ().Place (slot);
		}
	}

	public Transform FindEmptySlot(InventoryItemObject item, SlotType type, int wagonIndex = -1, int slotIndex = -1)
	{
		WagonInventoryUI wagon = null;
		if (type == SlotType.Wagon) {
			if (wagonIndex > -1 && wagonIndex < wagonUIs.Count)
				wagon = wagonUIs [wagonIndex];
			else 
				wagon = wagonUIs [0];
		}
		else if (type == SlotType.Shop)
			wagon = shopInventory;
		
		if (wagonIndex > -1 && wagonIndex < wagonUIs.Count)
		{
			if (slotIndex > -1 && slotIndex < wagonUIs[wagonIndex].slots.Count)
			{
				// if slot and wagon indexes is correct then return specific slot
				if (wagon.IsEmptySlotForItem(slotIndex, item))
				{
					// all right
					//SetupUIItem(item, wagonIndex, slotIndex);
					return GetSlot(type, slotIndex, wagonIndex);
				}
			}

			// if only wagon index is correct then return first empty slot in that wagon
			int emptySlotIndex = wagon.FindEmptySlotForItem(item);
			if (emptySlotIndex != -1)
			{
				// all right
				//SetupUIItem(item, wagonIndex, emptySlotIndex);
				return GetSlot(type, emptySlotIndex, wagonIndex);
			}
		}

		// else return completely first empty slot
		// should work only with player wagons, not shop
		for (int checkingWagonIndex = 0;  checkingWagonIndex < wagonUIs.Count; checkingWagonIndex++)
		{
			if (type == SlotType.Wagon) {
				wagon = wagonUIs [checkingWagonIndex];
			}
			int emptySlotIndex = wagon.FindEmptySlotForItem(item);
			if (emptySlotIndex != -1)
			{
				//SetupUIItem(item, checkingWagonIndex, emptySlotIndex);

				return GetSlot(type, emptySlotIndex, checkingWagonIndex);
			}
		}
		Debug.Log ("no inventory space for item");
		return null;
	}





	public bool IsShopActive()
	{
		return shopUI.activeSelf;
	}

	public void ToggleShopUI()
	{
		shopUI.SetActive (!shopUI.activeSelf);
		if (shopUI.activeSelf) {
			SortShop ();
		}
	}

	VendorShop vendorShopInfo;
	public void LoadShopInfo(VendorShop info)
	{
		vendorShopInfo = info;
	}

	public VendorShop GetVendorShopInfo()
	{
		return vendorShopInfo;
	}

	public void SortShop()
	{
		
		List<InventoryItemObject> items = new List<InventoryItemObject> ();
		foreach(Transform slot in shopInventory.slots){
			InventoryItemObject item = GetItemObjectInSlot(slot);
			if (item != null){
				items.Add (item);

				item.Take ();
				item.transform.parent = null;
			}	
		}
		List<InventoryItemObject> sortedItems;
		//sortedItems = items.OrderByDescending(go=>go.info.uiInfo.size.x*go.info.uiInfo.size.y).ToList();
		//sortedItems = items.OrderBy(go=>go.info.name).ToList();
		sortedItems = items.OrderBy(go=>go.info.costInfo.timePrice).ToList();
		//sortedItems.Reverse();
		
		foreach(InventoryItemObject itemObj in sortedItems)
		{
			itemObj.Place (FindEmptySlot (itemObj, SlotType.Shop));
			//InventoryItem item = ItemDatabase.reference.FindByIndex(itemObj.info.databaseIndex);
			//InitItem (item, SlotType.Shop); // need to be initialized in shop inventory, not player inventory

		}

	}

	public IEnumerator InitShop()
	{
		foreach(Transform slot in shopInventory.slots){
			InventoryItemObject item = GetItemObjectInSlot(slot);
			if (item != null){
				Destroy(item.gameObject);
			}	
		}
		yield return null;
		foreach(VendorShop.ItemInfo info in vendorShopInfo.items)
		{
			for (int itemCounter = 0; itemCounter < info.quantity; itemCounter++)
			{
				InventoryItem item = ItemDatabase.reference.FindByIndex(info.index);
				InitItem (item, SlotType.Shop); // need to be initialized in shop inventory, not player inventory
			}
		}
		Debug.Log ("init shop done");
	}

	public void OpenShop()
	{
		
		if (!IsShopActive ())
			ToggleShopUI ();
	}

	public void CloseShop()
	{
		//shopInventory.Clear ();
		if (IsShopActive ())
			ToggleShopUI ();
	}
		
	public int GetItemTotalPrice(InventoryItemObject item)
	{
		if (!IsShopActive ())
		{
			//Debug.Log ("shop is not active");
			return item.info.costInfo.timePrice;
		}
		Transform slot = item.GetSlot();
		if (slot == null)
			return 0;
		WagonInventoryUI inventory = slot.parent.parent.GetComponent<WagonInventoryUI>();
		if (inventory == shopInventory) 
		{
			return item.GetBuyPrice ();
		}
		else
		{
			if (vendorShopInfo.FindInfoByIndex (item.info.databaseIndex) != null)
			{
				return item.GetSellPrice ();
			}
			else
			{
				//Debug.Log ("vendor is not interested");
				return 0;
			}
		}
	}
}
