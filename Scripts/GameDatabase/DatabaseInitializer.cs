using UnityEngine;
using System.Collections;

public class DatabaseInitializer : MonoBehaviour {

	public static DatabaseInitializer reference;
	public JournalQuestsDatabase journalQuestsDatabase;
	public ItemDatabase itemDatabase;
	public WorldTowns townsDatabase;
	public WorldWaystations waystationsDatabase;
	void Awake () 
	{
		if (reference == null) 
		{
			reference = this;
			DontDestroyOnLoad (gameObject);
			
			JournalQuestsDatabase.reference = journalQuestsDatabase;
			Debug.Log("JournalQuestsDatabase initialized");

			ItemDatabase.reference = itemDatabase;
			foreach(InventoryItem item in ItemDatabase.reference.items)
			{
				item.databaseIndex = ItemDatabase.reference.GetIndexOf(item);
			}
			Debug.Log("ItemDatabase initialized");

			WorldTowns.reference = townsDatabase;
			Debug.Log("TownsDatabase initialized");

			WorldWaystations.reference = waystationsDatabase;
			Debug.Log("WaystationsDatabase initialized");

		}
		else
		{
			Destroy(gameObject);
		}
	}
}
