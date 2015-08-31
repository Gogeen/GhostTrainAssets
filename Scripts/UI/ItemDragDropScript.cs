using UnityEngine;
using System.Collections;

public class ItemDragDropScript : MonoBehaviour {
	public Transform fromSlot;
	public Camera uiCamera;
	Vector3 onClickOffset = new Vector3();
	void OnDragEnd()
	{
		Transform potentialSlot = transform.parent.parent;
		WagonInventoryUI wagonInventory = potentialSlot.parent.parent.GetComponent<WagonInventoryUI>();
		transform.parent = null;
		if (!wagonInventory.CanPutInSlot (potentialSlot, GetComponent<Item> ().size)) {
			PutInSlot(fromSlot);
			return;
		}
		else
		{
			PutInSlot(potentialSlot);

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
