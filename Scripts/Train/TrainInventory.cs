using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainInventory : MonoBehaviour {

	int wagonsCount;
	//Vector2 wagonSize = new Vector2 (6, 8);
	public List<WagonInventory> wagons = new List<WagonInventory>();
	public List<GameObject> itemsToPut = new List<GameObject>();

	public class WagonInventory
	{

	}

	[System.Serializable]
	public class InventoryItem
	{
		public int slotIndex;
		public Vector2 size;
		public string description;
		public Sprite image;
	}
	public InventoryItem item;
	/*void Start()
	{
		wagons.Add (new WagonInventory());
		wagons.Add (new WagonInventory());
		wagons.Add (new WagonInventory());
		foreach (GameObject prefab in itemsToPut) 
		{
			//GameObject item = Instantiate (prefab) as GameObject;

		}
	}*/

}