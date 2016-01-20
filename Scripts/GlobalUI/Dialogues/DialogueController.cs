using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

	public static DialogueController reference = null;
	QuestsController.Quest quest = null;
	QuestsController.QuestNode currentNode = null;
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
		currentNode = quest.nodes [0];
		GlobalUI.reference.SetState (GlobalUI.States.Dialogue);

		EventDelegate action;

		action = new EventDelegate(this, "FlipLogBack");
		EventDelegate.Add(logBackButton.GetComponent<UIButton>().onClick, action);
		
		action = new EventDelegate(this, "FlipLogForward");
		EventDelegate.Add(logForwardButton.GetComponent<UIButton>().onClick, action);

		UpdateUI(currentNode);

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
		//UpdateUI(DialogueSystem.reference.currentNodeIndex);
	}
	
	public void FlipLogForward()
	{
		DialogueSystem.reference.currentLogIndex += 2;
		//UpdateUI(DialogueSystem.reference.currentNodeIndex);
	}

	GameObject GenerateAnswerButton(string text, string imageName, int index)
	{
		GameObject button = Instantiate(answerButtonPrefab) as GameObject;
		button.transform.GetChild(0).GetComponent<UILabel>().text = text;
		button.transform.GetChild(1).GetComponent<UISprite>().spriteName = imageName;
		button.transform.parent = answerButtonsGrid.transform;
		button.transform.SetSiblingIndex(index);
		button.transform.localScale = new Vector3(1, 1, 1);

		return button;
	}

	public void DrawAnswerButtons(QuestsController.QuestNode node)
	{
		int answersCount = node.answers.Count;
		int indexInHierarchy = 0;
		for (int childIndex = 0; childIndex < answersCount; childIndex++)
		{
			QuestsController.Answer answer = node.answers [childIndex];
			if (!quest.settings.showUnavailableAnswers){
				if (!QuestsController.IsPassRequirements(answer)){
					continue;
				}
			}
			GameObject button = GenerateAnswerButton(answer.text, answer.imageName,indexInHierarchy);
			indexInHierarchy += 1;
			EventDelegate action = new EventDelegate(this, "SelectAnswer");
			action.parameters[0].value = answer;
			EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
			if (quest.settings.showUnavailableAnswers){
				if (!QuestsController.IsPassRequirements(answer)){
					Destroy(button.GetComponent<BoxCollider>());
				}
			}
		}
		answerButtonsGrid.GetComponent<UIGrid>().Reposition();
	}

	public void SelectAnswer(QuestsController.Answer answer)
	{
		QuestsController.ApplyResults (answer);

		QuestsController.QuestNode nextNode = QuestsController.FindNode (quest, answer.pointer);
		if (nextNode == null)
		{
			DialogueSystem.reference.FinishQuest();
			return;
		}
		currentNode = nextNode;
		UpdateUI(currentNode);
	}

	public void UpdateUI(QuestsController.QuestNode node)
	{
		
		/*if (DialogueSystem.reference.currentLogIndex > 0)
			logBackButton.SetActive(true);
		else
			logBackButton.SetActive(false);
		*/

		for (int childIndex = answerButtonsGrid.transform.childCount - 1; childIndex >= 0; childIndex--)
		{
			Destroy(answerButtonsGrid.transform.GetChild(childIndex).gameObject);
		}

		/*
		если текст слишком длинный, его надо разбить на несколько кусков
		берем первый кусок, оставшееся сохраняем
		показываем его, с единственным вариантом ответа - Далее
		повторяем эти действия, пока не останется текста для показа.
		когда его не останется, показываем варианты ответа как обычно.
		*/
		
		if (DialogueSystem.reference.currentLogIndex >= DialogueSystem.reference.log.Count)
		{
			logForwardButton.SetActive(false);
			logAnswerLabel.gameObject.SetActive(false);

			string textToShow = "";
			foreach (QuestsController.TextPart part in node.textParts) {
				if (QuestsController.IsPassRequirements (part)) {
					textToShow += part.text;
				}
					
			}
			//AITextLabel.text = quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetText();
			//if (quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetResultIndex() != quest.zeroResultIndex)
			//	AITextLabel.text += "\n\n" + quest.FindResultNode(quest.FindAINode(DialogueSystem.reference.currentNodeIndex).GetResultIndex()).GetText();
			AISprite.spriteName = node.imageName;
			AITextLabel.text = textToShow;
			/*
			string textPartToShow = "";
			while (textToShow != "") {
				for (int charIndex = 0; charIndex < textToShow.Length; charIndex++) {
					if (textToShow[charIndex] == '@') {
						// draw text and repeat
						textPartToShow = "";
						textToShow.CopyTo(0,textPartToShow,0,charIndex+1);
						textToShow.Remove (0, charIndex+1);
						break;
					}
					//textPartToShow += textToShow[charIndex];
				}
			}
			GameObject button = GenerateAnswerButton(answer.text, answer.imageName,indexInHierarchy);
			indexInHierarchy += 1;
			EventDelegate action = new EventDelegate(this, "SelectAnswer");
			action.parameters[0].value = answer;
			EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
			*/

			DrawAnswerButtons(node);
		}
		else
		{
			/*
			logForwardButton.SetActive(true);
			AITextLabel.text = quest.FindAINode(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex]).GetText();
			AISprite.spriteName = quest.FindAINode(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex]).GetSprite();
			logAnswerLabel.gameObject.SetActive(true);
			logAnswerLabel.text = quest.FindAINode(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex]).GetAnswer(DialogueSystem.reference.log[DialogueSystem.reference.currentLogIndex + 1]).GetText();
			*/
		}
	}
}
