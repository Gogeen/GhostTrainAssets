using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldWaystations : ScriptableObject {

	public static WorldWaystations reference;
	public List<WaystationInfo> waystations = new List<WaystationInfo>();

	[System.Serializable]
	public class WaystationInfo
	{
		public string name;
		public WorldTowns.PassengerInfo passengerInfo;
		public VendorShop shopInfo;
		//public TextQuest quest;
	}

	public WaystationInfo FindByName(string name)
	{
		foreach (WaystationInfo info in waystations) {
			if (info.name == name)
				return info;
		}
		return null;
	}
}
