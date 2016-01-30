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

	void PrepareNodeInfo(QuestsController.QuestNode node)
	{
		SetTextToShow (node);
		SetSpriteToShow(node);
	}

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

		PrepareNodeInfo (currentNode);
		UpdateUI();

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
		DialogueSystem.reference.currentLogIndex -= 1;
		UpdateUI ();
	}
	
	public void FlipLogForward()
	{
		DialogueSystem.reference.currentLogIndex += 1;
		UpdateUI ();
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
			if (!quest.settings.showUnavailableAnswers.value){
				if (!QuestsController.IsPassRequirements(answer)){
					continue;
				}
			}
			GameObject button = GenerateAnswerButton(answer.text, answer.imageName,indexInHierarchy);
			indexInHierarchy += 1;
			EventDelegate action = new EventDelegate(this, "SelectAnswer");
			action.parameters[0].value = answer;
			EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
			if (quest.settings.showUnavailableAnswers.value){
				if (!QuestsController.IsPassRequirements(answer)){
					Destroy(button.GetComponent<BoxCollider>());
				}
			}
		}
		answerButtonsGrid.GetComponent<UIGrid>().Reposition();
	}

	public void SelectAnswer(QuestsController.Answer answer)
	{
		DialogueSystem.reference.log.Add (new string[]{AITextLabel.text, AISprite.spriteName, answer.text});
		DialogueSystem.reference.currentLogIndex += 1;

		QuestsController.ApplyResults (answer);

		QuestsController.QuestNode nextNode = QuestsController.FindNode (quest, answer.pointer);
		if (nextNode == null)
		{
			DialogueSystem.reference.FinishQuest();
			return;
		}
		currentNode = nextNode;
		PrepareNodeInfo (currentNode);
		UpdateUI();
	}

	public void Continue(bool writeLog = false)
	{
		/*
		если текст слишком длинный, его надо разбить на несколько кусков
		берем первый кусок, оставшееся сохраняем
		показываем его, с единственным вариантом ответа - Далее
		повторяем эти действия, пока не останется текста для показа.
		когда его не останется, показываем варианты ответа как обычно.
		*/
		if (writeLog) {
			DialogueSystem.reference.log.Add (new string[]{AITextLabel.text, AISprite.spriteName, "Далее" });
			DialogueSystem.reference.currentLogIndex += 1;
		}
			
		UpdateTextToShow ();

		UpdateUI ();
	}

	string textToShow = "";
	string spriteToShow = "";
	void SetTextToShow(QuestsController.QuestNode node)
	{
		textToShow = "";
		foreach (QuestsController.TextPart part in node.textParts) {
			if (QuestsController.IsPassRequirements (part)) {
				textToShow += part.text;
			}
		}
		textToShow = textToShow.Replace("\\n", "\n");
	}

	void UpdateTextToShow()
	{
		if (textToShow.Contains ("@"))
			textToShow = textToShow.Substring (textToShow.IndexOf ('@') + 1);
		else
			textToShow = "";
	}

	string GetTextToShow()
	{
		string textPartToShow = "";
		for (int charIndex = 0; charIndex < textToShow.Length; charIndex++) {
			if (textToShow [charIndex] == '@') {
				// draw text and repeat
				//textToShow.CopyTo (0, textPartToShow, 0, charIndex + 1);
				break;
			} else {
				textPartToShow += textToShow [charIndex];
			}
			//textPartToShow += textToShow[charIndex];
		}
		return textPartToShow;
	}

	bool WillHaveTextToShow()
	{
		if (!textToShow.Contains ("@"))
			return false;
		return true;
	}

	void SetSpriteToShow(QuestsController.QuestNode node)
	{
		spriteToShow = node.imageName;
	}

	string GetSpriteToShow()
	{
		return spriteToShow;
	}

	public void UpdateUI()
	{
		for (int childIndex = answerButtonsGrid.transform.childCount - 1; childIndex >= 0; childIndex--)
		{
			Destroy(answerButtonsGrid.transform.GetChild(childIndex).gameObject);
		}

		if (DialogueSystem.reference.currentLogIndex > 0)
			logBackButton.SetActive(true);
		else
			logBackButton.SetActive(false);

		if (DialogueSystem.reference.currentLogIndex >= DialogueSystem.reference.log.Count)
		{
			logForwardButton.SetActive(false);
			logAnswerLabel.gameObject.SetActive(false);


			AITextLabel.text = GetTextToShow();
			AISprite.spriteName = GetSpriteToShow ();

			if (WillHaveTextToShow()) {
				GameObject button = GenerateAnswerButton ("Далее", "", 0);
				EventDelegate action = new EventDelegate (this, "Continue");
				action.parameters[0].value = true;
				EventDelegate.Add (button.GetComponent<UIButton> ().onClick, action);
				answerButtonsGrid.GetComponent<UIGrid>().Reposition();
			} else {
				DrawAnswerButtons(currentNode);
			}
		}
		else
		{
			logForwardButton.SetActive(true);
			logAnswerLabel.gameObject.SetActive(true);
			AITextLabel.text = DialogueSystem.reference.log [DialogueSystem.reference.currentLogIndex][0]; 
			AISprite.spriteName = DialogueSystem.reference.log [DialogueSystem.reference.currentLogIndex][1];
			logAnswerLabel.text = DialogueSystem.reference.log [DialogueSystem.reference.currentLogIndex][2];

		}
	}

	void Update()
	{
		
	}
}
