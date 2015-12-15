using UnityEngine;
using System.Collections;

public class TownController : MonoBehaviour {

	public VendorShop shopInfo;
	public TextQuest quest;

    public int levelToLoadIndex = 2;
    
	public void StartTravel(int mapIndex)
    {
		PlayerSaveData.reference.Save ();
		shopInfo = WorldTowns.reference.FindByName(StrategyMapUIController.reference.GetDestinationTownName ()).shopInfo;
		quest = WorldTowns.reference.FindByName(StrategyMapUIController.reference.GetDestinationTownName ()).quest;
		GameController.reference.LoadMap(mapIndex);
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
		InventorySystem.reference.LoadShopInfo (shopInfo);
		GlobalUI.reference.SetState (GlobalUI.States.Shop);
	}
}
