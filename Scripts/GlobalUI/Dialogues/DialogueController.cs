using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

	public static DialogueController reference = null;
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
		quest = DialogueSystem.reference.quest;
		GlobalUI.reference.SetState (GlobalUI.States.Dialogue);

		EventDelegate action;

		action = new EventDelegate(this, "FlipLogBack");
		EventDelegate.Add(logBackButton.GetComponent<UIButton>().onClick, action);
		
		action = new EventDelegate(this, "FlipLogForward");
		EventDelegate.Add(logForwardButton.GetComponent<UIButton>().onClick, action);

		UpdateUI(DialogueSystem.reference.currentNodeIndex);

	}
	
	public void Deactivate()
	{
		GlobalUI.reference.GoBack();

		EventDelegate action;

		action = new EventDelegate(this, "FlipLogBack");
		EventDelegate.Remove(logBackButton.GetComponent<UIButton>().onClick, action);
		
		action = new EventDelegate(this, "FlipLogForward");
		EventDelegate.Remove(logForwardButton.GetComponent<UIButton>().onClick, action);
	}

	public void FlipLogBack()
	{
		DialogueSystem.reference.currentLogIndex -= 2;
		UpdateUI(DialogueSystem.reference.currentNodeIndex);
	}
	
	public void FlipLogForward()
	{
		DialogueSystem.reference.currentLogIndex += 2;
		UpdateUI(DialogueSystem.reference.currentNodeIndex);
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
					if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequired() > DialogueSystem.reference.currentInfluence)
					{
						continue;
					}
					else if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequiredMax() < DialogueSystem.reference.currentInfluence)
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
				if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequired() > DialogueSystem.reference.currentInfluence)
					Destroy(button.GetComponent<BoxCollider>());
				else if (quest.FindAINode(nodeIndex).GetAnswer(childIndex).GetInfluenceRequiredMax() < DialogueSystem.reference.currentInfluence)
					Destroy(button.GetComponent<BoxCollider>());
			}
			
		}
		answerButtonsGrid.GetComponent<UIGrid>().Reposition();
	}

	public void SelectAnswer(int index)
	{
		DialogueSystem.reference.currentInfluence += quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetAnswer(index).GetInfluenceGained();
		DialogueSystem.reference.log.Add(DialogueSystem.reference.currentNodeIndex);
		DialogueSystem.reference.log.Add(index);
		DialogueSystem.reference.currentLogIndex = DialogueSystem.reference.log.Count;
		
		if (quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetAnswer(index).GetNextNodeIndex() == quest.finishNodeIndex)
		{
			DialogueSystem.reference.FinishQuest();
			return;
		}
		DialogueSystem.reference.currentNodeIndex = quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetAnswer(index).GetNextNodeIndex();
		
		UpdateUI(DialogueSystem.reference.currentNodeIndex);
	}

	public void UpdateUI(int nodeIndex)
	{
		
		if (DialogueSystem.reference.currentLogIndex > 0)
			logBackButton.SetActive(true);
		else
			logBackButton.SetActive(false);
		
		for (int childIndex = answerButtonsGrid.transform.childCount - 1; childIndex >= 0; childIndex--)
		{
			Destroy(answerButtonsGrid.transform.GetChild(childIndex).gameObject);
		}
		
		
		if (DialogueSystem.reference.currentLogIndex >= DialogueSystem.reference.log.Count)
		{
			logForwardButton.SetActive(false);
			logAnswerLabel.gameObject.SetActive(false);
			
			AITextLabel.text = quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetText();
			if (quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetResultIndex() != quest.zeroResultIndex)
				AITextLabel.text += "\n\n" + quest.FindResultNode(quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetResultIndex()).GetText();
			AISprite.spriteName = quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetSprite();
			
			DrawAnswerButtons(nodeIndex);
		}
		else
		{
			logForwardButton.SetActive(true);
			AITextLabel.text = quest.FindAINode(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex]).GetText();
			AISprite.spriteName = quest.FindAINode(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex]).GetSprite();
			logAnswerLabel.gameObject.SetActive(true);
			logAnswerLabel.text = quest.FindAINode(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex]).GetAnswer(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex + 1]).GetText();
		}
	}
}
