using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryItemsBreakSystem : MonoBehaviour {

	public static InventoryItemsBreakSystem reference;

	public float itemBreakTick;
	public float itemDurabilityBreakMin;
	public float itemDurabilityBreakMax;

	public List<InventoryItemObject> itemsToBreak = new List<InventoryItemObject> ();

	IEnumerator BreakItems()
	{
		while (true)
		{
			if (PlayerTrain.reference == null)
				yield return null;
			yield return new WaitForSeconds(itemBreakTick);

			BreakWagon (0, Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax), true);
			for(int wagonIndex = 0; wagonIndex < PlayerSaveData.reference.wagonData.Count; wagonIndex++)
			{
				BreakWagon (1 + wagonIndex, Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax), true);
			}
		}
	}

	public void BreakItem(InventoryItemObject item, float value)
	{
		item.Break (value);
	}

	/*public void BreakWholeInventory(float value)
	{
		BreakWagon (0, value);
		for(int wagonIndex = 0; wagonIndex < PlayerSaveData.reference.wagonData.Count; wagonIndex++)
		{
			BreakWagon (1 + wagonIndex, value);
		}
	}*/

	IEnumerator UpdateItemsToBreak()
	{
		while (true) {
			while (itemsToBreak.Count != 1 + PlayerSaveData.reference.wagonData.Count) {
				if (itemsToBreak.Count < 1 + PlayerSaveData.reference.wagonData.Count)
					itemsToBreak.Add (null);
				else if (itemsToBreak.Count > 1 + PlayerSaveData.reference.wagonData.Count)
					itemsToBreak.RemoveAt (itemsToBreak.Count - 1);
			}
			//if (itemsToBreak [0] == null || itemsToBreak [0].IsBroken() || !itemsToBreak [0].IsEquipped()) {
			for (int itemIndex = PlayerSaveData.reference.trainData.equippedItems.Length - 1; itemIndex >= 0; itemIndex--)
			{
				InventoryItemObject equippedItem = PlayerSaveData.reference.trainData.equippedItems [itemIndex];
				if (equippedItem != null && !equippedItem.IsBroken()) {
					itemsToBreak [0] = equippedItem;
					break;
				}
			}

			for (int wagonIndex = 0; wagonIndex < PlayerSaveData.reference.wagonData.Count; wagonIndex++) {
				if (itemsToBreak [wagonIndex + 1] == null || itemsToBreak [wagonIndex + 1].IsBroken() || !PlayerSaveData.reference.wagonData [wagonIndex].items.Contains(itemsToBreak [wagonIndex + 1])) {
					PlayerSaveData.WagonData wagonData = PlayerSaveData.reference.wagonData [wagonIndex];
					if (wagonData.items.Count == 0) {
						continue;
					}
					int repairedItemsCount = 0;
					foreach (InventoryItemObject item in wagonData.items) {
						if (!item.IsBroken ()) {
							repairedItemsCount += 1;
						}
					}
					if (repairedItemsCount == 0)
						continue;
					int itemIndex = Random.Range (0, repairedItemsCount);
					foreach (InventoryItemObject item in wagonData.items) {
						if (!item.IsBroken ()) {
							if (itemIndex == 0) {
								itemsToBreak [wagonIndex + 1] = item;
								break;
							}
							itemIndex -= 1;
						}
					}
				}
			}
			yield return null;	
		}
	}

	public void BreakWagon(int index, float damage, bool ignoreShield = false)
	{
		if (!ignoreShield && PlayerSaveData.reference.trainData.conditions.Shield) {
			PlayerSaveData.reference.trainData.conditions.Shield = false;
			return;
		}
		BreakItem (itemsToBreak [index], damage);
	}

	void Start()
	{
		DontDestroyOnLoad (gameObject);
		StartCoroutine ("BreakItems");
		StartCoroutine ("UpdateItemsToBreak");
		reference = this;
	}
}
