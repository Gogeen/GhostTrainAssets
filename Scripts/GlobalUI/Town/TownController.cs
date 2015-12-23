using UnityEngine;
using System.Collections;

public class TownController : MonoBehaviour {

	public static TownController reference;

	public GameObject TravelButtonPanel;
	public string name;
	public WorldTowns.PassengerInfo passengerInfo;
	public VendorShop shopInfo;
	public TextQuest quest;

    public int levelToLoadIndex = 2;
    
	public void StartTravel(int mapIndex)
    {
		DeactivateTravelButton ();
		PlayerSaveData.reference.Save ();
		PlayerSaveData.reference.LoadTownInfo (StrategyMapUIController.reference.GetDestinationTownName ());
		GameController.reference.LoadMap(mapIndex);
	}

	public void ActivateTravelButton()
	{
		TravelButtonPanel.SetActive (true);
	}

	public void DeactivateTravelButton()
	{
		TravelButtonPanel.SetActive (false);
	}


    public void OpenStrategyMap()
    {
		GlobalUI.reference.SetState (GlobalUI.States.StrategyMap);
    }
    
	public void OpenInventory()
	{
		GlobalUI.reference.SetState (GlobalUI.States.Inventory);
	}

	public void StartQuest()
	{
		if (quest != null)
		{
			DialogueSystem.reference.StartQuest (quest);
			quest = null;
		}
	}

	// !!! need to rewrite shop opening part
	public void OpenShop()
	{
		GlobalUI.reference.SetState (GlobalUI.States.Shop);
	}
}
