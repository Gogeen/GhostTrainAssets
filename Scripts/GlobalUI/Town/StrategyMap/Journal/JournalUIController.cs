using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JournalUIController : MonoBehaviour {

	public int activeWindowIndex = 0;
    
	public List<Window> windows = new List<Window>();
	[System.Serializable]
	public class Window
	{
		public GameObject reference;
		public GameObject buttonReference;
	}

	public void ActivateNextWindow()
	{
		DeactivateWindow (activeWindowIndex);
		activeWindowIndex += 1;
		if (activeWindowIndex >= windows.Count)
		{
			activeWindowIndex = 0;
		}
		ActivateWindow (activeWindowIndex);

	}

    void Start()
    {
		ActivateWindow(windows[0].buttonReference);
    }

    public void OpenJournal()
    {
        gameObject.SetActive(true);
    }

    public void CloseJournal()
    {
        gameObject.SetActive(false);
    }

	public void ActivateWindow(GameObject button)
    {
		if (activeWindowIndex >= 0)
		{
			DeactivateWindow(activeWindowIndex);
		}
		int index = activeWindowIndex;
		foreach (Window window in windows)
		{
			if (button == window.buttonReference)
			{
				index = windows.IndexOf(window);
				break;
			}
		}
        windows [index].reference.SetActive (true);
        windows [index].buttonReference.GetComponent<UISprite>().depth = 2;
		activeWindowIndex = index;
    }

	public void ActivateWindow(int index)
	{
		windows [index].reference.SetActive (true);
		windows [index].buttonReference.GetComponent<UISprite>().depth = 2;
	}

    public void DeactivateWindow(int index)
    {
        windows[index].reference.SetActive(false);
		windows[index].buttonReference.GetComponent<UISprite>().depth = 0;
    }
}
