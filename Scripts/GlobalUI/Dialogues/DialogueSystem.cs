using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour {

	public static DialogueSystem reference = null;
	public TextQuest quest;
    
    public List<int> log = new List<int>();
    public int currentLogIndex;

	public int currentNodeIndex;
	public int currentInfluence;

    public void StartQuest (TextQuest questToPlay) {
		if (!questToPlay.IsPassRequirements ()) {
			Debug.Log ("Can't pass requirements! quest doesn't start");
			return;
		}
		quest = questToPlay;
		currentInfluence = 0;
        currentNodeIndex = quest.startNodeIndex;
        log = new List<int>();
        currentLogIndex = 0;

		DialogueController.reference.Activate();

    }

    public void FinishQuest()
    {
		quest.ApplyResult(quest.FindAINode (currentNodeIndex).GetResultIndex ());
		currentNodeIndex = quest.finishNodeIndex;
        quest = null;

		DialogueController.reference.Deactivate();
    }

    

    

    
}
