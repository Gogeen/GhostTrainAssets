using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextQuestSystem : MonoBehaviour {

    public TextQuest quest;
    public GameObject dialogueUI;
    public GameObject townUI;
    public UILabel AITextLabel;
    public UISprite AISprite;

    public GameObject logBackButton;
    public GameObject logForwardButton;
    public UILabel logAnswerLabel;

    public GameObject answerButtonsGrid;
    public GameObject answerButtonPrefab;

    List<int> log = new List<int>();
    int currentLogIndex;

    int currentNodeIndex;
    int currentInfluence;
    public void StartQuest () {
        dialogueUI.SetActive(true);
        townUI.SetActive(false);

        currentInfluence = 0;
        currentNodeIndex = quest.startNodeIndex;

        log = new List<int>();
        currentLogIndex = 0;

        UpdateUI(currentNodeIndex);
    }

    public void FinishQuest()
    {
        dialogueUI.SetActive(false);
        townUI.SetActive(true);
        currentNodeIndex = quest.finishNodeIndex;
        //UpdateUI(currentNodeIndex);
    }

    public void FlipLogBack()
    {
        currentLogIndex -= 2;
        UpdateUI(currentNodeIndex);
    }

    public void FlipLogForward()
    {
        currentLogIndex += 2;
        UpdateUI(currentNodeIndex);
    }

    public void SelectAnswer(GameObject button)
    {
        int index = button.transform.GetSiblingIndex();
        currentInfluence = quest.GetAnswerInfluenceGained(currentNodeIndex, index);
        log.Add(currentNodeIndex);
        log.Add(index);
        currentLogIndex = log.Count;
        currentNodeIndex = quest.GetNextNodeIndex(currentNodeIndex, index);
        
        UpdateUI(currentNodeIndex);
        if (currentNodeIndex == quest.finishNodeIndex)
            FinishQuest();
    }

    void DrawAnswerButtons(int nodeIndex)
    {
        int answersCount = quest.GetAnswersCount(nodeIndex);
        for (int childIndex = 0; childIndex < answersCount; childIndex++)
        {
            GameObject button = Instantiate(answerButtonPrefab) as GameObject;
            button.transform.GetChild(0).GetComponent<UILabel>().text = quest.GetAnswerText(nodeIndex, childIndex);
            button.transform.GetChild(1).GetComponent<UISprite>().spriteName = quest.GetAnswerSprite(nodeIndex, childIndex);
            button.transform.parent = answerButtonsGrid.transform;
            button.transform.localScale = new Vector3(1, 1, 1);
            EventDelegate action = new EventDelegate(this, "SelectAnswer");
            action.parameters[0] = new EventDelegate.Parameter(button.GetComponent<UIButton>(), "gameObject");
            EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
            if (quest.GetAnswerInfluenceRequired(nodeIndex, childIndex) > currentInfluence)
                DestroyImmediate(button.GetComponent<BoxCollider>());

        }
        answerButtonsGrid.GetComponent<UIGrid>().Reposition();
    }

    public void UpdateUI(int nodeIndex)
    {
        
        if (currentLogIndex > 0)
            logBackButton.SetActive(true);
        else
            logBackButton.SetActive(false);

        for (int childIndex = answerButtonsGrid.transform.childCount - 1; childIndex >= 0; childIndex--)
        {
            DestroyImmediate(answerButtonsGrid.transform.GetChild(childIndex).gameObject);
        }


        if (currentLogIndex >= log.Count)
        {
            logForwardButton.SetActive(false);
            logAnswerLabel.gameObject.SetActive(false);

            AITextLabel.text = quest.GetAIText(currentNodeIndex);
            AISprite.spriteName = quest.GetAISprite(currentNodeIndex);
            
            DrawAnswerButtons(nodeIndex);
        }
        else
        {
            logForwardButton.SetActive(true);
            AITextLabel.text = quest.GetAIText(log[currentLogIndex]);
            AISprite.spriteName = quest.GetAISprite(log[currentLogIndex]);
            logAnswerLabel.gameObject.SetActive(true);
            logAnswerLabel.text = quest.GetAnswerText(log[currentLogIndex], log[currentLogIndex + 1]);
        }
    }
}
