using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JournalQuestsDatabase : ScriptableObject {

	public static JournalQuestsDatabase reference = null;
    public List<JournalQuest> quests = new List<JournalQuest>();

	public JournalQuest FindByName(string name)
	{
		foreach(JournalQuest quest in quests)
		{
			if (quest.name == name)
				return quest;
		}
		return null;
	}

	public int GetIndexOf(JournalQuest quest)
	{
		return quests.IndexOf (quest);
	}

	public JournalQuest FindByIndex(int index)
	{
		return quests [index];
	}


}
