using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VendorShop : ScriptableObject {

	public List<ItemInfo> items = new List<ItemInfo>();

	[System.Serializable]
	public class ItemInfo
	{
		public int index;
		public int quantity;
		public float buyModifier;
		public float sellModifier;
	}

	public ItemInfo FindInfoByIndex(int index)
	{
		foreach (ItemInfo info in items)
		{
			if (info.index == index)
				return info;
		}
		return null;
	}
}
