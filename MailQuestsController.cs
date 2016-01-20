using UnityEngine;
using System.Collections;

public class MailQuestsController : MonoBehaviour {

	public static MailQuestsController reference;

	public bool canComplete = false;
	public bool inProgress = false;
	public int timeForQuest;

	public void StartQuestTimer(int value)
	{
		inProgress = true;
		timeForQuest = value;
	}

	void Start () {
		reference = this;
	}

	public bool IsTimeOut()
	{
		return timeForQuest <= 0;
	}

	public void EndQuest()
	{
		canComplete = false;
		if (InventorySystem.reference.FindItem ("письмо") != null) {
			Destroy (InventorySystem.reference.FindItem ("письмо").gameObject);
		}
		inProgress = false;

	}

	float timer = 0;
	void Update()
	{
		if (InventorySystem.reference.FindItem ("письмо") != null) {
			inProgress = true;
		} else {
			inProgress = false;
		}
		if (IsTimeOut ()) {
			if (inProgress) {
				EndQuest ();
			}
			return;
		}
		timer += Time.deltaTime;
		while (timer > 1) {
			timer -= 1;
			timeForQuest -= 1;
		}
	}
}
