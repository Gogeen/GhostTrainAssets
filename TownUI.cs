using UnityEngine;
using System.Collections;

public class TownUI : MonoBehaviour {

    public GameObject strategyMapUI;
    public GameObject townUI;

    public int levelToLoadIndex = 2;
    public void StartTravel(int mapIndex)
    {
        Application.LoadLevel(mapIndex);
    }

    public void ToggleStrategyMapUI()
    {
        strategyMapUI.SetActive(!strategyMapUI.activeSelf);
    }
    public void ToggleTownUI()
    {
        townUI.SetActive(!townUI.activeSelf);
    }
}
