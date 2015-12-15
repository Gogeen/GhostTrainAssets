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
		Item itemInfo = item.GetComponent<Item>();
		item.GetComponent<UISprite> ().spriteName = itemInfo.reference.uiInfo.spriteName;
		Transform slot = null;
		foreach (SlotInfo info in slots)
		{
			if (info.type == itemInfo.reference.type)
				slot = info.slot;

		}
		if (slot == null)
		{
			Debug.Log ("wrong item to init!");
			return;
		}
		item.transform.parent = slot.GetComponent<UIDragDropContainer>().reparentTarget;
		item.transform.localPosition = new Vector3 (0,0,0);
		slot.GetComponent<UIDragDropContainer> ().reparentTarget.GetComponent<UIGrid> ().Reposition ();
	}

	public bool IsSlotEmpty(Transform slot)
	{
		if (slot.GetComponent<UIDragDropContainer> ().reparentTarget.childCount > 0)
			return false;
		return true;
	}

	public Item GetUIItemInSlot(Transform slot)
	{
		if (IsSlotEmpty (slot))
			return null;
		return slot.GetComponent<UIDragDropContainer>().reparentTarget.GetChild(0).GetComponent<Item>();
	}
}
