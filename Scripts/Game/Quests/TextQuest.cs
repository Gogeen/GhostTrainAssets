using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextQuest : ScriptableObject {

	public string name;
    public bool showUnavailableAnswers;
    public int startNodeIndex;
    public int finishNodeIndex;
    public List<AINode> nodes = new List<AINode>();
    public int zeroResultIndex;
	public ScriptableObject resultScript;
    public List<ResultNode> results = new List<ResultNode>();
    // Use this for initialization

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

	public void ApplyResult(int index)
	{
		if (name == "Girl")
		{
			if (index == 1)
			{
				Debug.Log (FindResultNode(index).GetText());
			}
			else if (index == 2)
			{
				Debug.Log (FindResultNode(index).GetText());
				PlayerSaveData.reference.quests.Add (JournalQuestsDatabase.reference.FindByName("сокровищница времени"));
			}
		}
	}
}
