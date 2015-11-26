using UnityEngine;
using System.Collections;

public class UIThroughEntireGame : MonoBehaviour {

	public static UIThroughEntireGame reference;
	public DialogueController dialogueController;
	public DialogueSystem dialogueSystem;
	public TrainTimeScript timeController;
	public InventorySystem inventorySystem;
	void Awake()
	{
		if (reference == null) 
		{
			reference = this;
			DontDestroyOnLoad (gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		DialogueController.reference = dialogueController;
		Debug.Log ("Dialogue Controller ref is now " + dialogueController);
		DialogueSystem.reference = dialogueSystem;
		//DialogueSystem.reference.Deactivate ();
		TrainTimeScript.reference = timeController;
		InventorySystem.reference = inventorySystem;
	}
}
