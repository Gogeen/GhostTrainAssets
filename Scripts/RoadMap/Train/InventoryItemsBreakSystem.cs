using UnityEngine;
using System.Collections;

public class InventoryItemsBreakSystem : MonoBehaviour {

	public float itemBreakTick;
	public float itemDurabilityBreakMin;
	public float itemDurabilityBreakMax;

	IEnumerator BreakItems()
	{
		while (true)
		{
			yield return new WaitForSeconds(itemBreakTick);
			if (PlayerTrain.reference != null)
			{
				Debug.Log("BreakItems");
				foreach (WagonInventoryUI wagonUI in InventorySystem.reference.wagonUIs)
				{
					foreach(Transform slot in wagonUI.slots)
					{
						InventoryItemObject item = InventorySystem.reference.GetItemObjectInSlot (slot);
						if (item != null)
							item.Break(Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax));
					}
				}
				foreach (InventoryItemObject equippedItem in PlayerSaveData.reference.trainData.equippedItems)
				{
					if (equippedItem != null)
						equippedItem.Break(Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax));
				}
			}
		}
	}


	public void Start()
	{
		StartCoroutine ("BreakItems");
	}
}
