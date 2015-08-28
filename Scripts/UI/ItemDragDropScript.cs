using UnityEngine;
using System.Collections;

public class ItemDragDropScript : MonoBehaviour {
	public Transform lastSlot;
	void OnDragEnd()
	{
		WagonInventorySlot slot = transform.parent.parent.GetComponent<WagonInventorySlot> ();
		Transform potentialSlot = transform.parent;
		transform.parent = null;
		if (!slot.CanPut (GetComponent<Item> ().size)) {
			Debug.Log ("slot is full");
			transform.parent = lastSlot.GetComponent<UIDragDropContainer> ().reparentTarget;
			GetComponent<UIDragDropItem> ().SendMessage ("OnDragDropRelease", lastSlot.gameObject);
			return;
		}
		else
		{
			transform.parent = potentialSlot;

		}

	}
	
	void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			lastSlot = transform.parent.parent;

		}
		else
		{
			transform.localScale = new Vector2(1,1);
		}
	}
}
