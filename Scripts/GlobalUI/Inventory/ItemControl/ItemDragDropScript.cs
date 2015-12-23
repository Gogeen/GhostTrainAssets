using UnityEngine;
using System.Collections;

public class ItemDragDropScript : MonoBehaviour {
	public Camera uiCamera;
	Vector3 onClickOffset = new Vector3();

	void OnDragEnd()
	{
		StopCoroutine ("ApplyClickOffset");

		Transform potentialSlot = transform.parent.parent;
		transform.parent = null;
		if (potentialSlot.GetComponent<UIDragDropContainer> () == null)
		{
			GetComponent<InventoryItemObject>().Place(GetComponent<InventoryItemObject> ().GetLastSlot());
			return;
		}
		GetComponent<InventoryItemObject>().Place(potentialSlot);
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
		if (UICamera.currentTouchID == -1) { // left click
			StopCoroutine ("ApplyClickOffset");
			StartCoroutine ("ApplyClickOffset");
		}
	}
	
	void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			if (UICamera.currentTouchID == -1) // left click
			{
				Debug.Log ("that was left click");
				onClickOffset = uiCamera.ScreenToWorldPoint (Input.mousePosition) - transform.position;

				if (GlobalUI.reference.IsState (GlobalUI.States.Shop)) {
					if (InventorySystem.reference.IsSelectingItemToRepair ()) {
						InventorySystem.reference.RepairItem (GetComponent<InventoryItemObject> ());
					}
				}
				InventoryUI.SetItemToCompare(GetComponent<InventoryItemObject>());
				
			}
			else if (UICamera.currentTouchID == -2) // right click
			{
				Debug.Log ("that was right click");
				if (GlobalUI.reference.IsState (GlobalUI.States.Shop)) {
					if (InventorySystem.reference.GetSlotInfo (GetComponent<InventoryItemObject> ().GetSlot ()).type != InventorySystem.SlotType.Shop) {
						GetComponent<InventoryItemObject> ().Place (InventorySystem.reference.FindEmptySlot (GetComponent<InventoryItemObject> (), InventorySystem.SlotType.Shop));
					} else {
						GetComponent<InventoryItemObject> ().Place (InventorySystem.reference.FindEmptySlot (GetComponent<InventoryItemObject> (), InventorySystem.SlotType.Wagon));
					}
				}
			}
		}
		else
		{
			
		}
	}

	void OnDoubleClick()
	{
		Debug.Log ("that was double click");
		// if player in shop window, then sell or buy item
		if (GlobalUI.reference.IsState (GlobalUI.States.Shop)) {
			if (InventorySystem.reference.GetSlotInfo (GetComponent<InventoryItemObject> ().GetSlot ()).type != InventorySystem.SlotType.Shop) {
				GetComponent<InventoryItemObject> ().Place (InventorySystem.reference.FindEmptySlot (GetComponent<InventoryItemObject> (), InventorySystem.SlotType.Shop));
			} else {
				GetComponent<InventoryItemObject> ().Place (InventorySystem.reference.FindEmptySlot (GetComponent<InventoryItemObject> (), InventorySystem.SlotType.Wagon));
			}
		}
	}
}
