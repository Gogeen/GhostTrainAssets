using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldTowns : ScriptableObject {

	public static WorldTowns reference;
	public List<TownInfo> towns = new List<TownInfo>();

	[System.Serializable]
	public class TownInfo
	{
		public string name;
		public PassengerInfo passengerInfo;
		public VendorShop shopInfo;
		public string questName;
	}

	public TownInfo FindByName(string name)
	{
		foreach (TownInfo info in towns) {
			if (info.name == name)
				return info;
		}
		return null;
	}

	[System.Serializable]
	public class PassengerInfo
	{
		public int MinPercentIn;
		public int MaxPercentIn;
		public int MinPercentOut;
		public int MaxPercentOut;

	}
}
