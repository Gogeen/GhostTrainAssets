using UnityEngine;
using System.Collections;

public class TownController : MonoBehaviour {

	public static TownController reference;

	public GameObject TravelButtonPanel;
	new public string name;
	public WorldTowns.PassengerInfo passengerInfo;
	public VendorShop shopInfo;
	public string questName;
	public TextQuest mailQuest;
	public TextQuest mailCompleteQuest;

    public int levelToLoadIndex = 2;
    
	public void StartTravel(int mapIndex)
    {
		DeactivateTravelButton ();
		PlayerSaveData.reference.Save ();
		PlayerSaveData.reference.LoadTownInfo (StrategyMapUIController.reference.GetDestinationTownName ());
		GameController.reference.LoadMap(mapIndex);
		PlayerSaveData.reference.trainData.conditions.LostControl = false;
		MailQuestsController.reference.canComplete = true;

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
		if (questName != "")
			DialogueSystem.reference.StartQuest (questName);
		questName = "";

	}

	public void StartMailQuest()
	{
		/*
		if (!MailQuestsController.reference.inProgress) {
			if (mailQuest != null) {
				DialogueSystem.reference.StartQuest (mailQuest);
			}
		} else if (MailQuestsController.reference.canComplete) {
			if (mailCompleteQuest != null) {
				DialogueSystem.reference.StartQuest (mailCompleteQuest);
			}
		}*/
	}

	// !!! need to rewrite shop opening part
	public void OpenShop()
	{
		GlobalUI.reference.SetState (GlobalUI.States.Shop);
	}

	void Update()
	{
		
	}
}
