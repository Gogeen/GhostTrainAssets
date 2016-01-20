using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class InventoryItemObject : MonoBehaviour {

	public int wagonIndex;
	public int slotIndex;

	public Transform lastSlot = null;

	public InventoryItem info = new InventoryItem();

	public Transform GetSlot()
	{
		if (transform.parent == null)
			return null;
		return transform.parent.parent;
	}

	public Transform GetLastSlot()
	{
		return lastSlot;
	}

	public bool IsBroken()
	{
		return info.durabilityInfo.current <= 0;
	}

	public void Break(float value)
	{
		if (PlayerSaveData.reference.trainData.conditions.Invulnerable)
			return;
		RemoveStats ();
		info.durabilityInfo.current -= value;
		if (info.durabilityInfo.current < 0) {
			info.durabilityInfo.current = 0;
			ResetEfficiency ();
		}
		//Debug.Log ("breaking "+item.name+" for "+value);
		ApplyStats();
	}

	public void AddEfficiency(float time)
	{
		RemoveStats ();
		Debug.Log ("deltatime: "+time);
		info.extraInfo.EfficiencyModifier += time  / info.extraInfo.TimeToMaxPotential;
		Debug.Log ("bonus efficiency: "+time  / info.extraInfo.TimeToMaxPotential);
		ApplyStats ();
	}

	public void ResetEfficiency()
	{
		info.extraInfo.EfficiencyModifier = 0;
	}

	public int GetRepairCost()
	{
		return (int)(info.costInfo.timePrice * (1 - info.durabilityInfo.current / info.durabilityInfo.max) * (1f - PlayerSaveData.reference.trainData.bonusRepairPricePercent/100f));
	}

	public int GetRepairCost(float value)
	{
		float repairPoints = 0;
		if (info.durabilityInfo.current + value > info.durabilityInfo.max)
			repairPoints = info.durabilityInfo.max - info.durabilityInfo.current;
		else
			repairPoints = value;
		return (int)(info.costInfo.timePrice * (repairPoints / info.durabilityInfo.max) * (1f - PlayerSaveData.reference.trainData.bonusRepairPricePercent/100f));
	}

	public void Repair()
	{
		RemoveStats ();
		// cost is equals item total price if item is totally broken
		int cost = GetRepairCost();
		if (TrainTimeScript.reference.HaveEnoughTime(cost))
		{
			info.durabilityInfo.current = info.durabilityInfo.max;
			TrainTimeScript.reference.AddTime (-cost);
		}
		ApplyStats();
	}

	public void Repair(float value, bool isFree = false)
	{
		RemoveStats ();
		// cost is equals item total price if item is totally broken
		int cost = 0;
		if (!isFree)
			cost = GetRepairCost(value);
		
		if (TrainTimeScript.reference.HaveEnoughTime(cost))
		{
			info.durabilityInfo.current += value;
			if (info.durabilityInfo.current > info.durabilityInfo.max)
				info.durabilityInfo.current = info.durabilityInfo.max;
			TrainTimeScript.reference.AddTime (-cost);
		}
		ApplyStats();
	}

	public void Take()
	{
		lastSlot = GetSlot ();
	}

	public void Place(Transform slot)
	{
		//Debug.Log ("place item function is on work");
		// check that slot is correctly set up
		if (slot.GetComponent<UIDragDropContainer> () == null) {
			//Debug.Log ("slot is broken!");
			return;
		}

		// check that potential slot is empty or is equipment slot
		if (GetLastSlot () != null) {
			if (!InventorySystem.reference.CanPutInSlot (slot, this)) {
				Place (GetLastSlot ());
				return;
			}
		}


		if (!PlayerSaveData.reference.trainData.conditions.CanManageInventory) {
			if (InventorySystem.reference.GetSlotInfo(GetLastSlot ()) != null) {
				if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type != InventorySystem.reference.GetSlotInfo (slot).type) {
					Place (GetLastSlot ());
					return;
				}
				if (wagonIndex != InventorySystem.reference.GetSlotInfo (slot).wagonIndex) {
					Place (GetLastSlot ());
					return;
				}
			}
		}

		transform.parent = slot.GetComponent<UIDragDropContainer>().reparentTarget;
		transform.localPosition = new Vector3 (0,0,0);
		GetComponent<UISprite>().spriteName = info.uiInfo.spriteName;
		GetComponent<UISprite> ().enabled = false;
		GetComponent<UISprite> ().enabled = true;


		// remove item from its slot...
		if (GetLastSlot () != null){
			if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type == InventorySystem.SlotType.Wagon) {
				if (PlayerSaveData.reference.wagonData [wagonIndex].items.Contains (this)) {
					TakeFromWagon (wagonIndex);
				}
			} else if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type == InventorySystem.SlotType.Equipment) {
				Unequip ();
			} else if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type == InventorySystem.SlotType.Shop) {
				if (InventorySystem.reference.GetSlotInfo (slot).type != InventorySystem.SlotType.Shop)
					Buy ();
			}
		}
		wagonIndex = InventorySystem.reference.GetSlotInfo(slot).wagonIndex;
		slotIndex = InventorySystem.reference.GetSlotInfo(slot).slotIndex;


		// ...and place it in new slot
		if (InventorySystem.reference.GetSlotInfo (slot).type == InventorySystem.SlotType.Wagon) {
			PutInWagon (wagonIndex);
		} else if (InventorySystem.reference.GetSlotInfo (slot).type == InventorySystem.SlotType.Equipment) {
			Equip ();
		} else if (InventorySystem.reference.GetSlotInfo (slot).type == InventorySystem.SlotType.Shop) {
			if (GetLastSlot () != null && InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type != InventorySystem.SlotType.Shop)
				Sell ();
		}

		// after all changes with item info check wagons passenger count
		// if player moved item from its wagon, remove extra passengers from there
		if (GetLastSlot () != null){
			if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type == InventorySystem.SlotType.Wagon) {
				if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type != InventorySystem.reference.GetSlotInfo (GetSlot ()).type ||
				    InventorySystem.reference.GetSlotInfo (GetLastSlot ()).wagonIndex != InventorySystem.reference.GetSlotInfo (GetSlot ()).wagonIndex) {

					PlayerSaveData.WagonData wagonData = PlayerSaveData.reference.wagonData [InventorySystem.reference.GetSlotInfo (GetLastSlot ()).wagonIndex];
					int extraPassengersCount = wagonData.currentPassengersCount - wagonData.maxPassengersCount;
					Debug.Log ("extra passengers count: " + extraPassengersCount);
					for (int passengerCounter = 0; passengerCounter < extraPassengersCount; passengerCounter++) {
						PlayerSaveData.reference.passengerData.RemovePassenger (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).wagonIndex);
					}
				}
			}
		}

		transform.SetSiblingIndex (0);
		slot.GetComponent<UIDragDropContainer> ().reparentTarget.GetComponent<UIGrid> ().Reposition ();


		lastSlot = slot;
		//Debug.Log ("place item function finished, new slot is  " + slot);
		// after all changes with item info check its durability
		if (info.durabilityInfo.current > info.durabilityInfo.max) {
			info.durabilityInfo.current = info.durabilityInfo.max;
		}

	}

	bool statsApplied = false;
	public void ApplyStats()
	{
		if (statsApplied)
			RemoveStats ();
		//Debug.Log ("apply stats");

		PlayerSaveData.reference.trainData.currentWeight += info.costInfo.weight;

		statsApplied = true;
		if (IsBroken()) {
			return;
		}

		if (info.type == InventoryItem.Type.NonEquippable || IsEquipped ()) {
			// train stats part
			PlayerSaveData.reference.trainData.power += info.bonusInfo.power;
			PlayerSaveData.reference.trainData.magicPower += info.bonusInfo.magicPower;
			PlayerSaveData.reference.trainData.maxWeight += info.bonusInfo.maxWeight;
			PlayerSaveData.reference.trainData.maxCrewCount += info.bonusInfo.maxCrewSpace;
			PlayerSaveData.reference.trainData.maxSpeed += info.bonusInfo.maxSpeed;

			if (info.extraInfo.MoreEffectiveWithTime) {
				info.bonusInfo.tradePricePercent += info.bonusInfo.tradePricePercent * info.extraInfo.maxPotentialModifier * info.extraInfo.EfficiencyModifier;
				info.bonusInfo.repairPricePercent += info.bonusInfo.repairPricePercent * info.extraInfo.maxPotentialModifier * info.extraInfo.EfficiencyModifier;
			}

			PlayerSaveData.reference.trainData.bonusTradePricePercent += info.bonusInfo.tradePricePercent;
			PlayerSaveData.reference.trainData.bonusRepairPricePercent += info.bonusInfo.repairPricePercent;

			if (IsEquipped ()) {
				if (info.bonusInfo.equipmentDurability != 0) {
					foreach (InventoryItemObject equippedItem in PlayerSaveData.reference.trainData.equippedItems) {
						if (equippedItem == null)
							continue;
						equippedItem.info.durabilityInfo.max *= (100 + info.bonusInfo.equipmentDurability)/100;
						equippedItem.info.durabilityInfo.current *= (100 + info.bonusInfo.equipmentDurability)/100;
						if (equippedItem.info.durabilityInfo.current > equippedItem.info.durabilityInfo.max) {
							equippedItem.info.durabilityInfo.current = equippedItem.info.durabilityInfo.max;
						}
					}
					foreach (PlayerSaveData.WagonData wagonData in PlayerSaveData.reference.wagonData) {
						foreach (InventoryItemObject item in wagonData.items) {
							item.info.durabilityInfo.max *= (100 + info.bonusInfo.equipmentDurability)/100;
							item.info.durabilityInfo.current *= (100 + info.bonusInfo.equipmentDurability)/100;
							if (item.info.durabilityInfo.current > item.info.durabilityInfo.max) {
								item.info.durabilityInfo.current = item.info.durabilityInfo.max;
							}
						}
					}
				}
				PlayerSaveData.reference.trainData.bonusEquipmentDurability += info.bonusInfo.equipmentDurability;
			}

			// wagon stats part
			if (info.type == InventoryItem.Type.NonEquippable){
				PlayerSaveData.reference.wagonData[wagonIndex].attraction += info.bonusInfo.attraction;
				PlayerSaveData.reference.wagonData[wagonIndex].currentPassengersCount += info.costInfo.passengerSpace;
				PlayerSaveData.reference.wagonData[wagonIndex].maxPassengersCount += info.bonusInfo.maxPassengerSpace;
			}
		}
		//statsApplied = true;
	}

	void RemoveStats()
	{
		if (!statsApplied)
			return;
		//Debug.Log ("remove stats");
		PlayerSaveData.reference.trainData.currentWeight -= info.costInfo.weight;

		statsApplied = false;
		if (IsBroken()) {
			return;
		}
			
		if (info.type == InventoryItem.Type.NonEquippable || IsEquipped ()) {
			// train stats part
			PlayerSaveData.reference.trainData.power -= info.bonusInfo.power;
			PlayerSaveData.reference.trainData.magicPower -= info.bonusInfo.magicPower;
			PlayerSaveData.reference.trainData.maxWeight -= info.bonusInfo.maxWeight;
			PlayerSaveData.reference.trainData.maxCrewCount -= info.bonusInfo.maxCrewSpace;
			PlayerSaveData.reference.trainData.maxSpeed -= info.bonusInfo.maxSpeed;
			PlayerSaveData.reference.trainData.bonusTradePricePercent -= info.bonusInfo.tradePricePercent;
			PlayerSaveData.reference.trainData.bonusRepairPricePercent -= info.bonusInfo.repairPricePercent;

			if (info.extraInfo.MoreEffectiveWithTime) {
				info.bonusInfo.tradePricePercent /= (1 + info.extraInfo.maxPotentialModifier * info.extraInfo.EfficiencyModifier);
				info.bonusInfo.repairPricePercent /= (1 + info.extraInfo.maxPotentialModifier * info.extraInfo.EfficiencyModifier);
			}

			if (IsEquipped ()) {
				if (info.bonusInfo.equipmentDurability != 0) {
					foreach (InventoryItemObject equippedItem in PlayerSaveData.reference.trainData.equippedItems) {
						if (equippedItem == null)
							continue;
						equippedItem.info.durabilityInfo.max /= (100 + info.bonusInfo.equipmentDurability)/100;
						equippedItem.info.durabilityInfo.current /= (100 + info.bonusInfo.equipmentDurability)/100;
						//Debug.Log (GetSlot ());
						if (InventorySystem.reference.GetSlotInfo (GetSlot ()).type != InventorySystem.SlotType.Equipment) {
							if (equippedItem.info.durabilityInfo.current > equippedItem.info.durabilityInfo.max) {
								equippedItem.info.durabilityInfo.current = equippedItem.info.durabilityInfo.max;
							}
						}
					}
					foreach (PlayerSaveData.WagonData wagonData in PlayerSaveData.reference.wagonData) {
						foreach (InventoryItemObject item in wagonData.items) {
							item.info.durabilityInfo.max /= (100 + info.bonusInfo.equipmentDurability)/100;
							item.info.durabilityInfo.current /= (100 + info.bonusInfo.equipmentDurability)/100;
							if (InventorySystem.reference.GetSlotInfo (GetSlot ()).type != InventorySystem.SlotType.Equipment) {
								if (item.info.durabilityInfo.current > item.info.durabilityInfo.max) {
									item.info.durabilityInfo.current = item.info.durabilityInfo.max;
								}
							}
						}
					}
				}
				PlayerSaveData.reference.trainData.bonusEquipmentDurability -= info.bonusInfo.equipmentDurability;
			}

			// wagon stats part
			if (info.type == InventoryItem.Type.NonEquippable) {
				PlayerSaveData.reference.wagonData [wagonIndex].attraction -= info.bonusInfo.attraction;
				PlayerSaveData.reference.wagonData [wagonIndex].currentPassengersCount -= info.costInfo.passengerSpace;
				PlayerSaveData.reference.wagonData [wagonIndex].maxPassengersCount -= info.bonusInfo.maxPassengerSpace;
			}
		}
		//statsApplied = false;
	}

	public bool IsEquipped()
	{
		if (info.type == InventoryItem.Type.NonEquippable){
			return false;
		}

		int equipmentSlotIndex = GetEquipmentSlotIndex ();
		return this == PlayerSaveData.reference.trainData.equippedItems[equipmentSlotIndex];
	}

	public int GetEquipmentSlotIndex()
	{
		switch (info.type){
		case InventoryItem.Type.NonEquippable: 	{return -1;}
		case InventoryItem.Type.Slot1: 			{return 0;}
		case InventoryItem.Type.Slot2: 			{return 1;}
		case InventoryItem.Type.Slot3: 			{return 2;}
		}
		return -1;
	}

	public void Equip()
	{
		Debug.Log ("tryimg to equip item");
		if (info.type == InventoryItem.Type.NonEquippable){
			Debug.Log ("Item cant be equipped!"); return;
		}

		int equipmentSlotIndex = GetEquipmentSlotIndex(); 

		InventoryItemObject equippedItem = PlayerSaveData.reference.trainData.equippedItems [equipmentSlotIndex];
		if (equippedItem != null)
		{
			if (GetLastSlot () != null) {
				Transform potentialSlot = InventorySystem.reference.FindEmptySlot (this, InventorySystem.SlotType.Wagon);
				if (potentialSlot != null)
					equippedItem.Place (potentialSlot);
				else 
					equippedItem.Place (GetLastSlot ());
			}
		} 

		RemoveStats ();
		PlayerSaveData.reference.trainData.equippedItems[equipmentSlotIndex] = this;
		ApplyStats ();

		if (info.bonusInfo.equipmentDurability == 0) {
			info.durabilityInfo.max *= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability) / 100;
			info.durabilityInfo.current *= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability) / 100;
		}
	}

	public void Unequip()
	{
		Debug.Log ("tryimg to unequip item");
		RemoveStats ();
		int equipmentSlotIndex = GetEquipmentSlotIndex ();
		PlayerSaveData.reference.trainData.equippedItems[equipmentSlotIndex] = null;
		ResetEfficiency ();
		ApplyStats ();

		info.durabilityInfo.max /= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability)/100;
		info.durabilityInfo.current /= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability)/100;

	}

	public void PutInWagon(int index)
	{
		PlayerSaveData.reference.wagonData [index].items.Add (this);
		ApplyStats ();
		InventorySystem.reference.CheckSynergy(index);

		info.durabilityInfo.max *= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability)/100;
		info.durabilityInfo.current *= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability)/100;

	}

	public void TakeFromWagon(int index)
	{
		RemoveStats ();
		PlayerSaveData.reference.wagonData [index].items.Remove (this);
		InventorySystem.reference.CheckSynergy(index);

		info.durabilityInfo.max /= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability)/100;
		info.durabilityInfo.current /= (100 + PlayerSaveData.reference.trainData.bonusEquipmentDurability)/100;

	}

	public int GetBuyPrice()
	{
		VendorShop vendorShopInfo = InventorySystem.reference.GetVendorShopInfo();
		float buyMod = vendorShopInfo.FindInfoByIndex (info.databaseIndex).buyModifier;
		int totalPrice = (int)(info.costInfo.timePrice * buyMod  * (1f - PlayerSaveData.reference.trainData.bonusTradePricePercent/100f));
		return totalPrice;
	}

	public int GetSellPrice()
	{
		VendorShop vendorShopInfo = InventorySystem.reference.GetVendorShopInfo();
		float sellMod = vendorShopInfo.FindInfoByIndex (info.databaseIndex).sellModifier;
		int totalPrice = (int)(info.costInfo.timePrice * sellMod * (1f + PlayerSaveData.reference.trainData.bonusTradePricePercent/100f));
		return totalPrice;
	}

	public void Buy()
	{
		Debug.Log ("tryimg to buy item");
		TrainTimeScript.reference.AddTime (-GetBuyPrice());
		//StartCoroutine(InventorySystem.reference.SortShop ());
		//InventorySystem.reference.SortShop ();
	}

	public void Sell()
	{
		Debug.Log ("tryimg to sell item");
		TrainTimeScript.reference.AddTime (GetSellPrice());
		//StartCoroutine(InventorySystem.reference.SortShop ());
		//InventorySystem.reference.SortShop ();
	}

	void Update()
	{
		// set up item image size
		transform.localScale = info.uiInfo.size;


	}

	void OnDestroy()
	{
		
		if (InventorySystem.reference.GetSlotInfo (GetSlot ()).type == InventorySystem.SlotType.Wagon) {
			if (PlayerSaveData.reference.wagonData [wagonIndex].items.Contains (this)) {
				TakeFromWagon (wagonIndex);
			}
		} else if (InventorySystem.reference.GetSlotInfo (GetLastSlot ()).type == InventorySystem.SlotType.Equipment) {
			Unequip ();
			RemoveStats ();
		}
	}
}
