using UnityEngine;
using System.Collections;

public class WagonInventorySlot : MonoBehaviour {

	public bool CanPut(Vector2 size)
	{
		int currentSlotIndex = 0;
		for (int childIndex = 0; childIndex < transform.parent.childCount; childIndex++)
		{
			if (transform.parent.GetChild(childIndex) == transform)
			{
				currentSlotIndex = childIndex;
				break;
			}
		}
		for(int XIndex = 0; XIndex < size.x; XIndex++)
		{
			for(int YIndex = 0; YIndex < size.y; YIndex++)
			{
				int slotToCheckIndex = currentSlotIndex + XIndex + YIndex*6;
				if (slotToCheckIndex >= transform.parent.childCount)
					return false;
				int firstSlotRow = currentSlotIndex/6 + YIndex;
				int slotToCheckRow = slotToCheckIndex/6;
				if (firstSlotRow != slotToCheckRow)
					return false;
				WagonInventorySlot slotToCheck = transform.parent.GetChild(slotToCheckIndex).GetComponent<WagonInventorySlot>();
				if (!slotToCheck.IsEmpty())
					return false;
			}
		}
		return true;
	}
	public bool IsEmpty()
	{
		Debug.Log (gameObject+" IsEmpty = "+!(GetComponent<UIDragDropContainer> ().reparentTarget.childCount > 0));
		if (GetComponent<UIDragDropContainer> ().reparentTarget.childCount > 0)
			return false;
		return true;
	}



	/*void OnDrop(GameObject go)
	{
		if (go.GetComponent<Item> () == null)
			return;

		if (!CanPut (go.GetComponent<Item> ().size)) 
		{
			go.transform.parent = go.GetComponent<ItemDragDropScript> ().lastSlot;
			go.transform.localScale = new Vector2 (1, 1);
		} 
		else
		{
			go.GetComponent<ItemDragDropScript> ().lastSlot = GetComponent<UIDragDropContainer> ().reparentTarget;
		}
	}*/

}
