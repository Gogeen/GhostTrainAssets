using UnityEngine;
using System.Collections;

public class DatabaseInitializer : MonoBehaviour {

	public JournalQuestsDatabase journalQuestsDatabase;
	public ItemDatabase itemDatabase;

	void Awake () 
	{
		JournalQuestsDatabase.reference = journalQuestsDatabase;
		Debug.Log("JournalQuestsDatabase initialized");
		ItemDatabase.reference = itemDatabase;
		Debug.Log("ItemDatabase initialized");

	}
}
