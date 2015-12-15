using UnityEngine;
using System.Collections;

public class UIThroughEntireGame : MonoBehaviour {

	public static UIThroughEntireGame reference;
	public DialogueController dialogueController;
	public DialogueSystem dialogueSystem;
	public TrainTimeScript timeController;
	public InventorySystem inventorySystem;
	public PauseMenuController pauseMenu;
	public GameSettingsController gameSettings;
	void Awake()
	{
		if (reference == null) 
		{
			reference = this;
			DontDestroyOnLoad (gameObject);

			DialogueController.reference = dialogueController;
			Debug.Log ("Dialogue Controller ref is now " + dialogueController);
			DialogueSystem.reference = dialogueSystem;
			//DialogueSystem.reference.Deactivate ();
			TrainTimeScript.reference = timeController;
			InventorySystem.reference = inventorySystem;
			PauseMenuController.reference = pauseMenu;
			GameSettingsController.reference = gameSettings;
			GameSettingsController.reference.Load();
			GameSettingsController.reference.Update();
		}
		else
		{
			Destroy(gameObject);
		}

	}

	void OnDisable()
	{
		GameSettingsController.reference.Save();

	}
}
