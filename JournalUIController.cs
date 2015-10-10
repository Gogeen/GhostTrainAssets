using UnityEngine;
using System.Collections;

public class JournalUIController : MonoBehaviour {

    public GameObject activeWindow = null;
    public UISprite activeButton = null;

    void Start()
    {
        ActivateWindow(activeWindow, activeButton);
    }

    public void OpenJournal()
    {
        gameObject.SetActive(true);
    }

    public void CloseJournal()
    {
        gameObject.SetActive(false);
    }

    public void ActivateWindow(GameObject window, UISprite button)
    {
        if (activeWindow != null && activeButton != null)
        {
            DeactivateWindow(activeWindow, activeButton);
            
        }
        window.SetActive(true);
        button.depth = 2;

        activeWindow = window;
        activeButton = button;
    }

    public void DeactivateWindow(GameObject window, UISprite button)
    {
        window.SetActive(false);
        button.depth = 0;
    }
}
