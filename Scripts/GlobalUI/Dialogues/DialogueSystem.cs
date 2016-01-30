using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour {

	public static DialogueSystem reference = null;
	//public string questName;
	public QuestsController.Quest quest = null;
	public List<string[]> log = new List<string[]>();
    public int currentLogIndex;

	public void StartQuest (string questName) {
		quest = QuestsController.GetQuest (questName);
		if (quest == null) {
			Debug.Log ("Quest doesn't exist!");
			return;
		}
		if (!QuestsController.IsPassRequirements (quest)) {
			Debug.Log ("Can't pass requirements! quest doesn't start");
			return;
		}
		log = new List<string[]>();
        currentLogIndex = 0;

		DialogueController.reference.Activate();

    }

    public void FinishQuest()
    {
		quest = null;
		//questName = "";
		DialogueController.reference.Deactivate();
		QuestsController.ClearLocalData ();
    }

    

    

    
}
