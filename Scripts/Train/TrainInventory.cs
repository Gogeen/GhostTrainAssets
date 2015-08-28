using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainInventory : MonoBehaviour {

	int wagonsCount;
	Vector2 wagonSize = new Vector2 (6, 8);
	public List<WagonInventory> wagons = new List<WagonInventory>();
	public List<GameObject> itemsToPut = new List<GameObject>();
	public class Slot
	{
		public bool isEmpty;
		public GameObject item;
		public Slot()
		{
			isEmpty = true;
			item = null;
		}
	}

	public class WagonInventory
	{
		public Slot[,] slots = new Slot[6,8];
		public WagonInventory()
		{
			slots = new Slot[6,8];
			for(int XIndex = 0; XIndex < 6; XIndex++)
			{
				for(int YIndex = 0; YIndex < 8; YIndex++)
				{
					slots[XIndex,YIndex] = new Slot();
				}
			}
		}
	}

	void Start()
	{
		wagons.Add (new WagonInventory());
		wagons.Add (new WagonInventory());
		wagons.Add (new WagonInventory());
		foreach (GameObject prefab in itemsToPut) 
		{
			GameObject item = Instantiate (prefab) as GameObject;

		}
	}

}