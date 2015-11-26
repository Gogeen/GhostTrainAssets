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
}
