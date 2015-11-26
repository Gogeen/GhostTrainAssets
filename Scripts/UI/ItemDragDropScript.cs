using UnityEngine;
using System.Collections;

public class ItemDragDropScript : MonoBehaviour {
	public Transform fromSlot;
	public Camera uiCamera;
	Vector3 onClickOffset = new Vector3();

	bool IsEquipmentSlot(Transform slot)
	{
		if(slot.parent.parent.GetComponent<InventoryEquipmentUI>() == null)
		{
			return false;
		}
		return true;
	}

	bool IsCorrectEquipmentSlot(Transform slot)
	{
		if (GetComponent<Item> ().reference.type == slot.parent.parent.GetComponent<InventoryEquipmentUI> ().GetSlotType (slot))
			return true;
		return false;
	}

	public bool IsSlotEmpty(Transform slot)
	{
		if (slot.GetComponent<UIDragDropContainer> ().reparentTarget.childCount > 0)
			return false;
		return true;
	}

	Item GetUIItemInSlot(Transform slot)
	{
		if (IsSlotEmpty (slot))
			return null;
		return slot.GetComponent<UIDragDropContainer> ().reparentTarget.GetChild (0).GetComponent<Item>();
	}

	void OnDragEnd()
	{
		Transform potentialSlot = transform.parent.parent;
		WagonInventoryUI wagonInventory = potentialSlot.parent.parent.GetComponent<WagonInventoryUI>();
		if (IsEquipmentSlot(potentialSlot))
		{
			if (IsCorrectEquipmentSlot(potentialSlot))
			{
				if (!IsSlotEmpty(potentialSlot))
				{
					Item item = GetUIItemInSlot(potentialSlot);
					Debug.Log ("slot is not empty: "+item);
					item.GetComponent<ItemDragDropScript>().PutInSlot(fromSlot);
					if (InventorySystem.reference.IsEquipped(item.reference))
						InventorySystem.reference.Unequip ((int)item.reference.type-1);

				}
				PutInSlot(potentialSlot);
				InventorySystem.reference.Equip (GetComponent<Item>().reference);
				return;
			}
			PutInSlot(fromSlot);
			return;
		}
		if (!IsEquipmentSlot (fromSlot))
		{
			WagonInventoryUI fromWagon = fromSlot.parent.parent.GetComponent<WagonInventoryUI>();
			if (!wagonInventory.slots.Contains(fromSlot))
			{
				//need to move item stats from one wagon to another
				InventorySystem.reference.MoveItem(GetComponent<Item>().reference, fromWagon, wagonInventory);
			}
		}
		transform.parent = null;
		if (!wagonInventory.CanPutInSlot (potentialSlot, GetComponent<Item> ().reference.uiInfo.size)) {
			PutInSlot(fromSlot);
			return;
		}
		else
		{
			PutInSlot(potentialSlot);
			if (InventorySystem.reference.IsEquipped (GetComponent<Item>().reference))
				InventorySystem.reference.Unequip((int)GetComponent<Item>().reference.type-1);

		}

	}

	IEnumerator ApplyClickOffset()
	{
		float timer = 0.2f;
		while (timer > 0)
		{
			timer -= Time.unscaledDeltaTime;
			transform.position += onClickOffset*5*Time.unscaledDeltaTime;
			yield return null;
		}
	}



	void OnDragStart()
	{
		StopCoroutine ("ApplyClickOffset");
		StartCoroutine ("ApplyClickOffset");
	}

	void SetFromSlot()
	{
		fromSlot = GetCurrentSlot();
	}

	Transform GetCurrentSlot()
	{
		return transform.parent.parent;
	}
	
	void PutInSlot(Transform slot)
	{
		if (slot.GetComponent<UIDragDropContainer> () == null)
		{
			PutInSlot(fromSlot);
			return;
		}
		GetComponent<UIDragDropItem> ().SendMessage ("OnDragDropRelease", slot.gameObject);
	}

	void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			onClickOffset = uiCamera.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			SetFromSlot();
		}
		else
		{

		}
	}
}
