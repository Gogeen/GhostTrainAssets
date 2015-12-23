using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InventoryEquipmentUI : MonoBehaviour {

	public List<SlotInfo> slots = new List<SlotInfo>();

	[System.Serializable]
	public class SlotInfo
	{
		public Transform slot;
		public InventoryItem.Type type;
	}

	public InventoryItem.Type GetSlotType(Transform slot)
	{
		foreach(SlotInfo info in slots)
		{
			if (info.slot == slot)
				return info.type;
		}
		Debug.Log ("this is not equipment slot");
		return InventoryItem.Type.NonEquippable;
	}

	public void InitItem(GameObject item)
	{
		InventoryItemObject itemObject = item.GetComponent<InventoryItemObject>();
		Transform slot = null;
		foreach (SlotInfo info in slots)
		{
			if (info.type == itemObject.info.type)
				slot = info.slot;

		}
		if (slot == null)
		{
			Debug.Log ("wrong item to init!");
			return;
		}
		itemObject.Place (slot);
	}


}
