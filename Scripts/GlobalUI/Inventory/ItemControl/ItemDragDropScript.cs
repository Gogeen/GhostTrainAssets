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
		WagonInventoryUI toWagon = potentialSlot.parent.parent.GetComponent<WagonInventoryUI>();


		transform.parent = null;

		if (IsEquipmentSlot(potentialSlot))
		{
			if (IsCorrectEquipmentSlot(potentialSlot))
			{
				if (!IsSlotEmpty(potentialSlot))
				{
					Item item = GetUIItemInSlot(potentialSlot);
					item.GetComponent<ItemDragDropScript>().SetFromSlot();

					item.GetComponent<ItemDragDropScript>().PutInSlot(fromSlot);
					if (InventorySystem.reference.IsEquipped(item.reference))
						InventorySystem.reference.Unequip ((int)item.reference.type-1);



				}
				PutInSlot(potentialSlot);
				InventorySystem.reference.Equip (GetComponent<Item>().reference);
				return;
			}
			else
			{
				PutInSlot(fromSlot);
				return;
			}
		}

		if (toWagon != null)
		{
			if (!toWagon.CanPutInSlot (potentialSlot, GetComponent<Item> ().reference.uiInfo.size)) {
				PutInSlot(fromSlot);
				return;
			}
			else
			{
				PutInSlot(potentialSlot);
			}
		}
		else
		{
			if (IsSlotEmpty(potentialSlot))
			{
				PutInSlot(potentialSlot);
			}
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

	public void SetFromSlot()
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
		WagonInventoryUI fromWagon = fromSlot.parent.parent.GetComponent<WagonInventoryUI>();
		WagonInventoryUI toWagon = slot.parent.parent.GetComponent<WagonInventoryUI>();
		if (toWagon != null)
		{
			//need to move item stats from one wagon to another
			if (fromWagon != null)
				InventorySystem.reference.MoveItem(GetComponent<Item>().reference, fromWagon, toWagon);

			GetComponent<Item>().slotIndex = toWagon.slots.IndexOf (slot);
			GetComponent<Item> ().wagonIndex = InventorySystem.reference.wagonUIs.IndexOf (toWagon);
		}

		if (fromWagon != toWagon)
		{

			if (InventorySystem.reference.shopInventory == fromWagon) // trying to buy
			{
				// buy item
				InventorySystem.reference.BuyItem(GetComponent<Item>().reference);
			}
			else if (InventorySystem.reference.shopInventory == toWagon) // trying to sell
			{
				// sell item
				InventorySystem.reference.SellItem(GetComponent<Item>().reference);
			}
		}
	}
	
	void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			onClickOffset = uiCamera.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			SetFromSlot();
			if (InventorySystem.reference.IsSelectingItemToRepair())
				InventorySystem.reference.RepairItem(GetComponent<Item>().reference);
			else
				InventoryUI.SetItemToCompare(GetComponent<Item>());
		}
		else
		{

		}
	}
}
