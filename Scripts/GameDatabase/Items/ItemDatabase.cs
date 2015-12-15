using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : ScriptableObject {

	public static ItemDatabase reference = null;
	public List<InventoryItem> items = new List<InventoryItem>();

	public InventoryItem FindByName(string name)
	{
		foreach(InventoryItem item in items)
		{
			if (item.name == name)
				return item;
		}
		return null;
	}
	
	public int GetIndexOf(InventoryItem item)
	{
		return items.IndexOf (item);
	}
	
	public InventoryItem FindByIndex(int index)
	{
		return items [index];
	}
}
