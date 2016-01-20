using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextQuest : ScriptableObject {

	new public string name;
    public bool showUnavailableAnswers;
    public int startNodeIndex;
    public int finishNodeIndex;
    public List<AINode> nodes = new List<AINode>();
    public int zeroResultIndex;
	public List<ResultNode> results = new List<ResultNode>();

	public GameObject target;

    [System.Serializable]
    public class AINode
    {
        public string text;
        public string spriteName;
        public int index;
        public int resultIndex;
        public List<PlayerNode> answers = new List<PlayerNode>();

        public string GetText()
        {
            return text;
        }
        public string GetSprite()
        {
            return spriteName;
        }
        public int GetAnswersCount()
        {
            return answers.Count;
        }
        public PlayerNode GetAnswer(int index)
        {
            return answers[index];
        }
        public int GetResultIndex()
        {
            return resultIndex;
        }
    }
    [System.Serializable]
    public class PlayerNode
    {
        public string text;
        public string spriteName;
        public bool checkInfluence;
        public int influenceGained;
        public int influenceRequired;
        public int influenceRequiredMax;
        public int nextNodeIndex;

        public string GetText()
        {
            return text;
        }
        public string GetSprite()
        {
            return spriteName;
        }
        public int GetNextNodeIndex()
        {
            return nextNodeIndex;
        }
        public int GetInfluenceRequired()
        {
            return influenceRequired;
        }
        public int GetInfluenceRequiredMax()
        {
            return influenceRequiredMax;
        }
        public int GetInfluenceGained()
        {
            return influenceGained;
        }
    }
    [System.Serializable]
    public class ResultNode
    {
        public string text;
		public string description;
        public int index;
        public string functionName;
        public string GetText()
        {
            return text;
        }
    }

    public AINode FindAINode (int index) {
        foreach (AINode node in nodes)
        {
            if (node.index == index)
                return node;
        }
        return null;
	}
    public ResultNode FindResultNode(int index)
    {
        foreach (ResultNode result in results)
        {
            if (result.index == index)
                return result;
        }
        return null;
    }

	public bool IsPassRequirements()
	{
		if (name == "Treasury") {
			if (PlayerSaveData.reference.quests.Contains (JournalQuestsDatabase.reference.FindByName ("сокровищница времени")))
			{
				if (PlayerSaveData.reference.quests[PlayerSaveData.reference.quests.IndexOf(JournalQuestsDatabase.reference.FindByName ("сокровищница времени"))].currentStage == 0)
					return true;
			}
			return false;
		} else if (name == "Girl part 2") {
			if (PlayerSaveData.reference.quests.Contains (JournalQuestsDatabase.reference.FindByName ("сокровищница времени")))
			{
				if (PlayerSaveData.reference.quests[PlayerSaveData.reference.quests.IndexOf(JournalQuestsDatabase.reference.FindByName ("сокровищница времени"))].currentStage == 1)
					return true;
			}
			return false;
		} else if (name == "mail complete") {
			if (!MailQuestsController.reference.IsTimeOut ())
				return true;
			return false;
		}else
			return true;
	}

	public void ApplyResult(int index)
	{
		if (FindResultNode (index) == null)
			return;
		if (name == "Girl")
		{
			if (index == 1)
			{
				if (InventorySystem.reference.FindItem ("фонарь") != null) {
					Destroy (InventorySystem.reference.FindItem ("фонарь").gameObject);
				}
			}
			else if (index == 2)
			{
				PlayerSaveData.reference.quests.Add (JournalQuestsDatabase.reference.FindByName("сокровищница времени"));
				PlayerSaveData.reference.quests [PlayerSaveData.reference.quests.Count - 1].currentStage = 0;
			}
			else if (index == 3)
			{
				PlayerSaveData.reference.quests.Add (JournalQuestsDatabase.reference.FindByName("сокровищница времени"));
				PlayerSaveData.reference.quests [PlayerSaveData.reference.quests.Count - 1].currentStage = 1;
			}
		}
		else if (name == "Girl part 2")
		{
			if (index == 1)
			{
				//Debug.Log ("need to remove waystation here");
				if (target != null)
					target.SetActive (false);
			}
		}
		else if (name == "mail01")
		{
			if (index == 1)
			{
				InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName("письмо"),InventorySystem.SlotType.Wagon);
				MailQuestsController.reference.StartQuestTimer (180);
				MailQuestsController.reference.canComplete = false;
				//PlayerSaveData.reference.quests.Add (JournalQuestsDatabase.reference.FindByName("почтовое поручение"));
				//PlayerSaveData.reference.quests [PlayerSaveData.reference.quests.Count - 1].currentStage = 0;
			}
		}
		else if (name == "mail complete")
		{
			if (index == 1)
			{
				TrainTimeScript.reference.AddTime (180);
				MailQuestsController.reference.EndQuest();

				//PlayerSaveData.reference.quests[PlayerSaveData.reference.quests.LastIndexOf(JournalQuestsDatabase.reference.FindByName ("почтовое поручение"))].currentStage = 1;
			}
		}
	}
}
