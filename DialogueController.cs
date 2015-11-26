using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour {

	public static DialogueController reference = null;
	public TextQuest quest;
    
    public List<int> log = new List<int>();
    public int currentLogIndex;

	public int currentNodeIndex;
	public int currentInfluence;

    public void StartQuest (TextQuest questToPlay) {
		quest = questToPlay;
		currentInfluence = 0;
        currentNodeIndex = quest.startNodeIndex;
        log = new List<int>();
        currentLogIndex = 0;

		GameController.Pause();
		DialogueSystem.reference.Activate();

    }

    public void FinishQuest()
    {
		quest.ApplyResult(quest.FindAINode (currentNodeIndex).GetResultIndex ());
		currentNodeIndex = quest.finishNodeIndex;
        quest = null;

		GameController.Resume();
		DialogueSystem.reference.Deactivate();
    }

    

    

    
}
