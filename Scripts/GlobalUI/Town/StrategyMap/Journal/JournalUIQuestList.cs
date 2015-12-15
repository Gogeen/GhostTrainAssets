using UnityEngine;
using System.Collections;

public class JournalUIQuestList : MonoBehaviour {

	public bool IsForCompletedQuests = false;
	public GameObject buttonPrefab;
	public GameObject description;
	public GameObject tableNameLabel;
	public GameObject optionalChildTable;
	public void UpdateList()
	{
		for(int childIndex = transform.childCount - 1; childIndex >= 0; childIndex--)
		{
			if (transform.GetChild(childIndex).gameObject == description)
				continue;
			if (transform.GetChild(childIndex).gameObject == tableNameLabel)
				continue;
			if (optionalChildTable != null)
				if (transform.GetChild(childIndex).gameObject == optionalChildTable)
					continue;
			Destroy(transform.GetChild(childIndex).gameObject);
		}
		HideQuestDescription ();
		int questsCount = PlayerSaveData.reference.quests.Count;
		for(int questIndex = 0; questIndex < questsCount; questIndex++)
		{
			bool isCompleted = PlayerSaveData.reference.quests[questIndex].stages[PlayerSaveData.reference.quests[questIndex].currentStage].isFinishing;
			if (IsForCompletedQuests)
			{
				if (!isCompleted)
					continue;
			}
			else
			{
				if (isCompleted)
					continue;
			}
			GameObject button = Instantiate(buttonPrefab);
			button.transform.parent = transform;
			button.transform.localScale = new Vector3(1,1,1);
			button.transform.SetSiblingIndex(questIndex+1);
			button.transform.GetChild (0).GetComponent<UILabel>().text = PlayerSaveData.reference.quests[questIndex].name;

			EventDelegate action = new EventDelegate(this, "ShowQuestDescription");
			action.parameters[0].value = questIndex;
			EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
		}
		GetComponent<UITable>().Reposition();
	}

	void HideQuestDescription()
	{
		description.SetActive (false);
		GetComponent<UITable>().Reposition();
	}

	public void ShowQuestDescription(int index)
	{
		description.SetActive (false);
		GetComponent<UITable>().Reposition();
		Debug.Log (index);
		JournalQuest quest = PlayerSaveData.reference.quests [index];
		Debug.Log (quest.stages [quest.currentStage]);
		description.SetActive (true);
		description.transform.SetSiblingIndex (index+2);
		description.transform.GetChild(0).GetComponent<UILabel>().text = quest.stages [quest.currentStage].description;
		GetComponent<UITable>().Reposition();
	}

	/*void Update()
	{
		GetComponent<UITable>().Reposition();

	}*/

	void OnEnable()
	{
		UpdateList ();
	}
}
