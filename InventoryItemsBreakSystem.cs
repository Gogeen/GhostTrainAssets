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
							wagonUI.GetUIItemInSlot(slot).reference.durabilityInfo.current -= Random.Range(itemDurabilityBreakMin, itemDurabilityBreakMax);
					}
				}
			}
		}
		yield return null;
	}
	
	public void Start()
	{
		StartCoroutine ("BreakItems");
	}
}
