using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class WagonInventoryUI : MonoBehaviour {

	public bool developerAccess;

	public GameObject slotPrefab;
	public Transform wagonGameObject;
	public int sizeX;
	public int sizeY;
	public List<Transform> slots = new List<Transform>();
	//public List<Transform> items = new List<Transform>();

	public string itemForSignName;

	void InitSlotsGrid()
	{
		Vector2 backgroundSize = GetComponent<UISprite> ().localSize;
		UIGrid slotsGrid = transform.GetChild (0).GetComponent<UIGrid> ();
		slotsGrid.maxPerLine = sizeX;
		slotsGrid.cellWidth = backgroundSize.x / slotsGrid.maxPerLine;
		slotsGrid.cellHeight = slotsGrid.cellWidth;
		if (slotsGrid.transform.childCount > sizeX*sizeY)
		{
			for(int slotIndex = slotsGrid.transform.childCount - 1; slotIndex >= 0; slotIndex--)
			{
				if (slotIndex >= sizeX*sizeY)
				{
					DestroyImmediate (slotsGrid.transform.GetChild(slotIndex).gameObject);
				}
			}
		}
		else if (slotsGrid.transform.childCount < sizeX*sizeY)
		{
			for(int slotIndex = 0; slotIndex < sizeX*sizeY; slotIndex++)
			{
				if (slotsGrid.transform.childCount <= slotIndex)
				{
					GameObject slot = Instantiate(slotPrefab) as GameObject;
					slot.transform.parent = slotsGrid.transform;
					slot.transform.localScale = new Vector2(1,1);
				}
			}
		}
		for(int slotIndex = 0; slotIndex < slotsGrid.transform.childCount; slotIndex++)
		{
			slotsGrid.transform.GetChild(slotIndex).GetComponent<UISprite>().width = (int)slotsGrid.cellWidth;
			slotsGrid.transform.GetChild(slotIndex).GetComponent<UISprite>().height = (int)slotsGrid.cellHeight;
		}

		slotsGrid.Reposition();
	}

	public bool CanPutInSlot(Transform slotToPut, Vector2 itemSize)
	{
		// first, check that item not going through right side of wagon
		if (slots.IndexOf (slotToPut) % sizeX + itemSize.x - 1 > sizeX)
			return false;
		
		for (int slotIndex = 0; slotIndex < slots.IndexOf(slotToPut); slotIndex++)
		{
			// dont need to check slots righter than right side of item
			if (slotIndex % sizeX > slots.IndexOf (slotToPut) % sizeX + itemSize.x - 1)
				continue;
			
			Transform slot = slots[slotIndex];
			if (!InventorySystem.reference.IsSlotEmpty(slot))
			{
				InventoryItemObject itemInCheckingSlot = InventorySystem.reference.GetItemObjectInSlot (slot);
				Vector2 checkingItemSize = itemInCheckingSlot.info.uiInfo.size;
				int checkingSlotIndex = slots.IndexOf (slot);
				int potentialSlotIndex = slots.IndexOf (slotToPut);
				if (checkingItemSize.x + checkingSlotIndex%sizeX >= potentialSlotIndex%sizeX + 1 &&
					checkingItemSize.y + checkingSlotIndex/sizeX >= potentialSlotIndex/sizeX + 1)
				{
					//Debug.Log ("cant put item in slot "+slotToPut);
					return false;
				}
			}
		}
		int currentSlotIndex = slots.IndexOf(slotToPut);

		for(int XIndex = 0; XIndex < itemSize.x; XIndex++)
		{
			for(int YIndex = 0; YIndex < itemSize.y; YIndex++)
			{
				int slotToCheckIndex = currentSlotIndex + XIndex + YIndex*sizeX;
				if (slotToCheckIndex >= slots.Count)
					return false;
				int firstSlotRow = currentSlotIndex/sizeX + YIndex;
				int slotToCheckRow = slotToCheckIndex/sizeX;
				if (firstSlotRow != slotToCheckRow)
					return false;
				Transform slotToCheck = slots[slotToCheckIndex];
				if (!InventorySystem.reference.IsSlotEmpty(slotToCheck))
					return false;
			}
		}
		return true;
	}
		
	public bool IsEmptySlotForItem(int slotIndex, InventoryItemObject item)
	{
		Transform slot = slots [slotIndex];
		return CanPutInSlot (slot, item.info.uiInfo.size);
	}

	public int FindEmptySlotForItem(InventoryItemObject item)
	{
		foreach(Transform slot in slots)
		{
			if(CanPutInSlot (slot, item.info.uiInfo.size))
				return slots.IndexOf(slot);
		}
		return -1;
	}

	bool IsTriangle(int slotIndex)
	{
		Transform slot = slots[slotIndex];
		if (InventorySystem.reference.IsSlotEmpty(slot))
			return false;
		InventoryItemObject item = InventorySystem.reference.GetItemObjectInSlot (slot);
		if (item.info.name != itemForSignName)
			return false;
		if (item.IsBroken ())
			return false;
        int itemRowIndex = slotIndex % sizeX;
        int cycleIterations = 0;
        if (itemRowIndex <= (sizeX - 1) / 2)
            cycleIterations = itemRowIndex;
        else
            cycleIterations = sizeX - 1 - itemRowIndex;

        for (int signHeight = 1; signHeight <= cycleIterations; signHeight++)
        {
            // triangle sign check
            if (slotIndex + sizeX * signHeight + signHeight >= slots.Count)
                continue;
			if (InventorySystem.reference.IsSlotEmpty(slots[slotIndex + sizeX * signHeight - signHeight]) || InventorySystem.reference.IsSlotEmpty(slots[slotIndex + sizeX * signHeight + signHeight]))
                continue;
            if ((slotIndex + sizeX * signHeight - signHeight) / sizeX != (slotIndex + sizeX * signHeight + signHeight) / sizeX)
                continue;
			
			if (InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight - signHeight]).info.name == itemForSignName &&
				InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight + signHeight]).info.name == itemForSignName)
            {
				if (InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight - signHeight]).IsBroken() ||
					InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight + signHeight]).IsBroken())
				{
					continue;
				}
				else return true;//Debug.Log ("has triangle");
            }
        }
		return false;
	}

	bool IsRectangle(int slotIndex)
	{
		Transform slot = slots[slotIndex];
		if (InventorySystem.reference.IsSlotEmpty(slot))
			return false;
		InventoryItemObject item = InventorySystem.reference.GetItemObjectInSlot (slot);
		if (item.info.name != itemForSignName)
			return false;
		if (item.IsBroken ())
			return false;
		
        int itemRowIndex = slotIndex % sizeX;
        int cycleIterations = sizeX - 1 - itemRowIndex;
        
        for (int signHeight = 1; signHeight <= cycleIterations; signHeight++)
        {
            // rectangle sign check
            if (slotIndex + sizeX * signHeight + signHeight >= slots.Count)
                continue;
			if (InventorySystem.reference.IsSlotEmpty(slots[slotIndex + signHeight]) || InventorySystem.reference.IsSlotEmpty(slots[slotIndex + sizeX * signHeight]) || InventorySystem.reference.IsSlotEmpty(slots[slotIndex + sizeX * signHeight + signHeight]))
                continue;
            if ((slotIndex) / sizeX != (slotIndex + signHeight) / sizeX)
                continue;
			if (InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + signHeight]).info.name == itemForSignName &&
				InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight]).info.name == itemForSignName &&
				InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight + signHeight]).info.name == itemForSignName)
            {
				if (InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + signHeight]).IsBroken() ||
					InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight]).IsBroken() ||
					InventorySystem.reference.GetItemObjectInSlot (slots[slotIndex + sizeX * signHeight + signHeight]).IsBroken())
				{
					continue;
				}
				else return true;//Debug.Log ("has rectangle");
            }
        }
		return false;
	}

	public void CheckSigns()
	{
		if (wagonGameObject == null)
		{
			return;
		}
		for (int slotIndex = 0; slotIndex < slots.Count; slotIndex++)
		{
			if (IsTriangle(slotIndex))
			{
				wagonGameObject.GetComponent<WagonScript>().signType = SignsController.SignType.Triangle;
				return;
			}
			else if (IsRectangle(slotIndex))
			{
				wagonGameObject.GetComponent<WagonScript>().signType = SignsController.SignType.Rectangle;
				return;
			}

		}
		wagonGameObject.GetComponent<WagonScript>().signType = SignsController.SignType.None;

	}

	void Start () {
		slots.Clear ();
		Transform slotsGrid = transform.GetChild (0);
		for(int slotIndex = 0; slotIndex < slotsGrid.childCount; slotIndex++)
		{
			slots.Add (slotsGrid.GetChild (slotIndex));
		}
		//InitItems ();
	}
	
	// Update is called once per frame
	void Update () {
		if (developerAccess)
			InitSlotsGrid ();
		//CheckSigns ();
	}
}
