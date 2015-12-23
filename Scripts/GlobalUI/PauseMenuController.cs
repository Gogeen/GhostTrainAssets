using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

	public static PauseMenuController reference;
	
	public void ToMainMenu()
	{
		GlobalUI.reference.SetState(GlobalUI.States.MainMenu);
	}

	public void Continue()
	{
		GlobalUI.reference.GoBack ();
	}

	public void LoadGame()
	{
		GlobalUI.reference.SetState(GlobalUI.States.LoadGame);
	}

	public void ToggleSettingsMenu()
	{
		GlobalUI.reference.SetState(GlobalUI.States.Settings);
	}
}
