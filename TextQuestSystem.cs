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

    EventDelegate action;

    public void StartQuest () {
        dialogueUI.SetActive(true);
        if (townUI != null)
            townUI.SetActive(false);
        else
            GameController.Pause();
        currentInfluence = 0;
        currentNodeIndex = quest.startNodeIndex;

        log = new List<int>();
        currentLogIndex = 0;

        action = new EventDelegate(this, "FlipLogBack");
        EventDelegate.Add(logBackButton.GetComponent<UIButton>().onClick, action);

        action = new EventDelegate(this, "FlipLogForward");
        EventDelegate.Add(logForwardButton.GetComponent<UIButton>().onClick, action);

        UpdateUI(currentNodeIndex);
    }

    public void FinishQuest()
    {
        dialogueUI.SetActive(false);
        if (townUI != null)
            townUI.SetActive(true);
        else
            GameController.Resume();
        currentNodeIndex = quest.finishNodeIndex;

        action = new EventDelegate(this, "FlipLogBack");
        EventDelegate.Remove(logBackButton.GetComponent<UIButton>().onClick, action);

        action = new EventDelegate(this, "FlipLogForward");
        EventDelegate.Remove(logForwardButton.GetComponent<UIButton>().onClick, action);
        
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

    public void SelectAnswer(int index)
    {
        //int index = button.transform.GetSiblingIndex();
        currentInfluence += quest.FindAINode(currentNodeIndex).GetAnswer(index).GetInfluenceGained();
        Debug.Log(currentInfluence);
        log.Add(currentNodeIndex);
        log.Add(index);
        currentLogIndex = log.Count;
        currentNodeIndex = quest.FindAINode(currentNodeIndex).GetAnswer(index).GetNextNodeIndex();

        if (currentNodeIndex == quest.finishNodeIndex)
        {
            FinishQuest();
            return;
        }
        UpdateUI(currentNodeIndex);
    }

    void DrawAnswerButtons(int nodeIndex)
    {
        int answersCount = quest.FindAINode(nodeIndex).GetAnswersCount();
        int indexInHierarchy = 0;
        for (int childIndex = 0; childIndex < answersCount; childIndex++)
        {
            if (!quest.showUnavailableAnswers)
            {
                if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).checkInfluence)
                {
                    if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequired() > currentInfluence)
                    {
                        continue;
                    }
                    else if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequiredMax() < currentInfluence)
                    {
                        continue;
                    }
                }
            }
            GameObject button = Instantiate(answerButtonPrefab) as GameObject;
            button.transform.GetChild(0).GetComponent<UILabel>().text = quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetText();
            button.transform.GetChild(1).GetComponent<UISprite>().spriteName = quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetSprite();
            button.transform.parent = answerButtonsGrid.transform;
            button.transform.SetSiblingIndex(indexInHierarchy);
            indexInHierarchy += 1;
            button.transform.localScale = new Vector3(1, 1, 1);
            EventDelegate action = new EventDelegate(this, "SelectAnswer");
            action.parameters[0].value = childIndex;
            EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
            if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).checkInfluence)
            {
                if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequired() > currentInfluence)
                    Destroy(button.GetComponent<BoxCollider>());
                else if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequiredMax() < currentInfluence)
                    Destroy(button.GetComponent<BoxCollider>());
            }

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
            Destroy(answerButtonsGrid.transform.GetChild(childIndex).gameObject);
        }


        if (currentLogIndex >= log.Count)
        {
            logForwardButton.SetActive(false);
            logAnswerLabel.gameObject.SetActive(false);

            AITextLabel.text = quest.FindAINode(currentNodeIndex).GetText();
            if (quest.FindAINode(currentNodeIndex).GetResultIndex() != quest.zeroResultIndex)
                AITextLabel.text += "\n\n" + quest.FindResultNode(quest.FindAINode(currentNodeIndex).GetResultIndex()).GetText();
            AISprite.spriteName = quest.FindAINode(currentNodeIndex).GetSprite();
            
            DrawAnswerButtons(nodeIndex);
        }
        else
        {
            logForwardButton.SetActive(true);
            AITextLabel.text = quest.FindAINode(log[currentLogIndex]).GetText();
            AISprite.spriteName = quest.FindAINode(log[currentLogIndex]).GetSprite();
            logAnswerLabel.gameObject.SetActive(true);
            logAnswerLabel.text = quest.FindAINode(log[currentLogIndex]).GetAnswer(log[currentLogIndex + 1]).GetText();
        }
    }
}
