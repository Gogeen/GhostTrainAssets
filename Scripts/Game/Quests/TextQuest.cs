using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextQuest : ScriptableObject {

    public int startNodeIndex;
    public int finishNodeIndex;
    public List<AINode> nodes = new List<AINode>();
    // Use this for initialization
	
    [System.Serializable]
    public class AINode
    {
        public string text;
        public string spriteName;
        public int index;
        public List<PlayerNode> answers = new List<PlayerNode>();
    }
    [System.Serializable]
    public class PlayerNode
    {
        public string text;
        public string spriteName;
        public int influenceGained;
        public int influenceRequired;
        public int nextNodeIndex;
    }
    AINode FindAINode (int index) {
        foreach (AINode node in nodes)
        {
            if (node.index == index)
                return node;
        }
        return null;
	}

    public string GetAIText(int index)
    {
        if (FindAINode(index) != null)
            return FindAINode(index).text;
        return null;
    }

    public string GetAISprite(int index)
    {
        if (FindAINode(index) != null)
            return FindAINode(index).spriteName;
        return null;
    }

    public int GetAnswersCount(int index)
    {
        if (FindAINode(index) != null)
            return FindAINode(index).answers.Count;
        return 0;
    }

    public string GetAnswerText(int nodeIndex, int answerIndex)
    {
        if (FindAINode(nodeIndex) != null)
            if (FindAINode(nodeIndex).answers.Count > answerIndex)
                return FindAINode(nodeIndex).answers[answerIndex].text;
        return null;
    }

    public string GetAnswerSprite(int nodeIndex, int answerIndex)
    {
        if (FindAINode(nodeIndex) != null)
            if (FindAINode(nodeIndex).answers.Count > answerIndex)
                return FindAINode(nodeIndex).answers[answerIndex].spriteName;
        return null;
    }

    public int GetNextNodeIndex(int nodeIndex, int answerIndex)
    {
        if (FindAINode(nodeIndex) != null)
            if (FindAINode(nodeIndex).answers.Count > answerIndex)
                return FindAINode(nodeIndex).answers[answerIndex].nextNodeIndex;
        return -1;
    }

    public int GetAnswerInfluenceRequired(int nodeIndex, int answerIndex)
    {
        if (FindAINode(nodeIndex) != null)
            if (FindAINode(nodeIndex).answers.Count > answerIndex)
                return FindAINode(nodeIndex).answers[answerIndex].influenceRequired;
        return 0;
    }

    public int GetAnswerInfluenceGained(int nodeIndex, int answerIndex)
    {
        if (FindAINode(nodeIndex) != null)
            if (FindAINode(nodeIndex).answers.Count > answerIndex)
                return FindAINode(nodeIndex).answers[answerIndex].influenceGained;
        return 0;
    }
}
