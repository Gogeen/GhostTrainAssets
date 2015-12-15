using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public GameObject ContinueButton;
	
	void Start()
	{
		if (PlayerSaveData.reference.IsSaveExists ())
			ContinueButton.SetActive (true);
	}

	public void LoadGame()
	{
		PlayerSaveData.reference.LoadData ();
		//Application.LoadLevel(1);
		GlobalUI.reference.SetState(GlobalUI.States.Town);
	}

    public void NewGame()
    {
		PlayerSaveData.reference.InitGewGame ();
        //Application.LoadLevel(1);
		GlobalUI.reference.SetState(GlobalUI.States.Town);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


	public void ToggleSettingsMenu()
	{
		GlobalUI.reference.SetState(GlobalUI.States.Settings);
	}
}
