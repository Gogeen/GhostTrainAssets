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
	List<Transform> slots = new List<Transform>();
	public List<Transform> items = new List<Transform>();

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
				if (!IsSlotEmpty(slotToCheck))
					return false;
			}
		}
		return true;
	}

	public bool IsSlotEmpty(Transform slot)
	{
		if (slot.GetComponent<UIDragDropContainer> ().reparentTarget.childCount > 0)
			return false;
		return true;
	}

	void InitItems()
	{
		foreach(Transform item in items)
		{
			Item itemInfo = item.GetComponent<Item>();
			Transform slot = slots[itemInfo.slotIndex];
			item.parent = slot.GetComponent<UIDragDropContainer>().reparentTarget;
		}
	}

	bool IsTriangle(int slotIndex)
	{
		Transform slot = slots[slotIndex];
		if (IsSlotEmpty(slot))
			return false;
		Item item = slot.GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>();
		if (item.itemName != itemForSignName)
			return false;
		
		// triangle sign check
		if (slotIndex+sizeX*2+1 >= slots.Count)
			return false;
		if (IsSlotEmpty(slots[slotIndex+sizeX*2-1]) || IsSlotEmpty(slots[slotIndex+sizeX*2+1]))
			return false;
		if ((slotIndex+sizeX*2-1)/sizeX != (slotIndex+sizeX*2+1)/sizeX)
			return false;
		if (slots[slotIndex+sizeX*2-1].GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>().itemName == itemForSignName && 
		    slots[slotIndex+sizeX*2+1].GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>().itemName == itemForSignName)
		{
			return true;//Debug.Log ("has triangle");
		}
		return false;
	}

	bool IsRectangle(int slotIndex)
	{
		Transform slot = slots[slotIndex];
		if (IsSlotEmpty(slot))
			return false;
		Item item = slot.GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>();
		if (item.itemName != itemForSignName)
			return false;
		
		// rectangle sign check
		if (slotIndex+sizeX*2+2 >= slots.Count)
			return false;
		if (IsSlotEmpty(slots[slotIndex+2]) || IsSlotEmpty(slots[slotIndex+sizeX*2]) || IsSlotEmpty(slots[slotIndex+sizeX*2+2]))
			return false;
		if ((slotIndex)/sizeX != (slotIndex+2)/sizeX)
			return false;
		if (slots[slotIndex+2].GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>().itemName == itemForSignName && 
		    slots[slotIndex+sizeX*2].GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>().itemName == itemForSignName &&
		    slots[slotIndex+sizeX*2+2].GetComponent<UIDragDropContainer>().reparentTarget.GetChild (0).GetComponent<Item>().itemName == itemForSignName)
		{
			return true;//Debug.Log ("has rectangle");
		}
		return false;
	}

	public void CheckSigns()
	{
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
		InitItems ();
	}
	
	// Update is called once per frame
	void Update () {
		if (developerAccess)
			InitSlotsGrid ();
		CheckSigns ();
	}
}
