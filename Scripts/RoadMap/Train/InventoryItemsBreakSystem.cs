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
						if (wagonUI.GetUIItemInSlot(slot) != null)
							InventorySystem.reference.BreakItem(wagonUI.GetUIItemInSlot(slot).reference, Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax));
					}
				}
				foreach (InventoryItem equippedItem in PlayerSaveData.reference.trainData.equippedItems)
				{
					if (equippedItem != null)
						InventorySystem.reference.BreakItem(equippedItem, Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax));
				}
			}
		}
	}


	public void Start()
	{
		StartCoroutine ("BreakItems");
	}
}
