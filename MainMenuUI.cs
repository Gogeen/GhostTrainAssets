using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {
	public GameObject ContinueButton;
	void Start()
	{
		if (PlayerSaveData.reference.IsSaveExists ())
			ContinueButton.SetActive (true);
	}

    public void PlayGame()
    {
        Application.LoadLevel(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

	void OnEnable()
	{
		if (TrainTimeScript.reference != null)
			TrainTimeScript.reference.gameObject.SetActive (false);
	}

	void OnDisable()
	{
		if (TrainTimeScript.reference != null)
			TrainTimeScript.reference.gameObject.SetActive (true);
	}
}
