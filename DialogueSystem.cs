using UnityEngine;
using System.Collections;

public class DialogueSystem : MonoBehaviour {

	public static DialogueSystem reference = null;
	TextQuest quest;
	public GameObject answerButtonPrefab;

	public UILabel AITextLabel;
	public UISprite AISprite;
	
	public GameObject logBackButton;
	public GameObject logForwardButton;
	public UILabel logAnswerLabel;
	
	public GameObject answerButtonsGrid;

	public void Activate()
	{
		quest = DialogueController.reference.quest;
		gameObject.SetActive (true);

		EventDelegate action;

		action = new EventDelegate(this, "FlipLogBack");
		EventDelegate.Add(logBackButton.GetComponent<UIButton>().onClick, action);
		
		action = new EventDelegate(this, "FlipLogForward");
		EventDelegate.Add(logForwardButton.GetComponent<UIButton>().onClick, action);

		UpdateUI(DialogueController.reference.currentNodeIndex);

	}
	
	public void Deactivate()
	{
		gameObject.SetActive (false);

		EventDelegate action;

		action = new EventDelegate(this, "FlipLogBack");
		EventDelegate.Remove(logBackButton.GetComponent<UIButton>().onClick, action);
		
		action = new EventDelegate(this, "FlipLogForward");
		EventDelegate.Remove(logForwardButton.GetComponent<UIButton>().onClick, action);
	}

	public void FlipLogBack()
	{
		DialogueController.reference.currentLogIndex -= 2;
		UpdateUI(DialogueController.reference.currentNodeIndex);
	}
	
	public void FlipLogForward()
	{
		DialogueController.reference.currentLogIndex += 2;
		UpdateUI(DialogueController.reference.currentNodeIndex);
	}

	public void DrawAnswerButtons(int nodeIndex)
	{
		int answersCount = quest.FindAINode(nodeIndex).GetAnswersCount();
		int indexInHierarchy = 0;
		for (int childIndex = 0; childIndex < answersCount; childIndex++)
		{
			if (!quest.showUnavailableAnswers)
			{
				if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).checkInfluence)
				{
					if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequired() > DialogueController.reference.currentInfluence)
					{
						continue;
					}
					else if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequiredMax() < DialogueController.reference.currentInfluence)
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
				if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequired() > DialogueController.reference.currentInfluence)
					Destroy(button.GetComponent<BoxCollider>());
				else if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequiredMax() < DialogueController.reference.currentInfluence)
					Destroy(button.GetComponent<BoxCollider>());
			}
			
		}
		answerButtonsGrid.GetComponent<UIGrid>().Reposition();
	}

	public void SelectAnswer(int index)
	{
		DialogueController.reference.currentInfluence += quest.FindAINode(DialogueController.reference.currentNodeIndex).GetAnswer(index).GetInfluenceGained();
		DialogueController.reference.log.Add(DialogueController.reference.currentNodeIndex);
		DialogueController.reference.log.Add(index);
		DialogueController.reference.currentLogIndex = DialogueController.reference.log.Count;
		
		if (quest.FindAINode(DialogueController.reference.currentNodeIndex).GetAnswer(index).GetNextNodeIndex() == quest.finishNodeIndex)
		{
			DialogueController.reference.FinishQuest();
			return;
		}
		DialogueController.reference.currentNodeIndex = quest.FindAINode(DialogueController.reference.currentNodeIndex).GetAnswer(index).GetNextNodeIndex();
		
		UpdateUI(DialogueController.reference.currentNodeIndex);
	}

	public void UpdateUI(int nodeIndex)
	{
		
		if (DialogueController.reference.currentLogIndex > 0)
			logBackButton.SetActive(true);
		else
			logBackButton.SetActive(false);
		
		for (int childIndex = answerButtonsGrid.transform.childCount - 1; childIndex >= 0; childIndex--)
		{
			Destroy(answerButtonsGrid.transform.GetChild(childIndex).gameObject);
		}
		
		
		if (DialogueController.reference.currentLogIndex >= DialogueController.reference.log.Count)
		{
			logForwardButton.SetActive(false);
			logAnswerLabel.gameObject.SetActive(false);
			
			AITextLabel.text = quest.FindAINode(DialogueController.reference.currentNodeIndex).GetText();
			if (quest.FindAINode(DialogueController.reference.currentNodeIndex).GetResultIndex() != quest.zeroResultIndex)
				AITextLabel.text += "\n\n" + quest.FindResultNode(quest.FindAINode(DialogueController.reference.currentNodeIndex).GetResultIndex()).GetText();
			AISprite.spriteName = quest.FindAINode(DialogueController.reference.currentNodeIndex).GetSprite();
			
			DrawAnswerButtons(nodeIndex);
		}
		else
		{
			logForwardButton.SetActive(true);
			AITextLabel.text = quest.FindAINode(DialogueController.reference.log[DialogueController.reference.currentLogIndex]).GetText();
			AISprite.spriteName = quest.FindAINode(DialogueController.reference.log[DialogueController.reference.currentLogIndex]).GetSprite();
			logAnswerLabel.gameObject.SetActive(true);
			logAnswerLabel.text = quest.FindAINode(DialogueController.reference.log[DialogueController.reference.currentLogIndex]).GetAnswer(DialogueController.reference.log[DialogueController.reference.currentLogIndex + 1]).GetText();
		}
	}
}
