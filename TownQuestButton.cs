using UnityEngine;
using System.Collections;

public class TownQuestButton : MonoBehaviour {

	public TextQuest quest;
	void Awake()
	{
		EventDelegate action;
		
		action = new EventDelegate(this, "ClickButton");
		EventDelegate.Add(GetComponent<UIButton>().onClick, action);
	}
	void ClickButton()
	{
		if (quest != null)
		{
			DialogueController.reference.StartQuest (quest);
			quest = null;
		}
	}
}
