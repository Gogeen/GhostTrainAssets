using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {

    public void PlayGame()
    {
        Application.LoadLevel(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
