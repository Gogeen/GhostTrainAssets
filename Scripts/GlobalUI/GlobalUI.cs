using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// CHECK !!!
public class GlobalUI : MonoBehaviour {

	public static GlobalUI reference;

	public GameObject MainMenu;
	public GameObject Settings;
	public GameObject Town;
	public GameObject PauseMenu;
	public GameObject StrategyMap;
	public GameObject Inventory;
	public GameObject Dialogue;
	public GameObject Road;
	public GameObject LoadingScreen;
	public GameObject Waystation;
	public GameObject LoadGame;

	public GameObject Timer;
	public GameObject PauseMenuButton;

	public enum States
	{
		MainMenu,
		Settings,
		Town,
		PauseMenu,
		StrategyMap,
		Inventory,
		Shop,
		Dialogue,
		Road,
		LoadingScreen,
		Waystation,
		LoadGame,
		Null
	}
	public States currentState;
	public List<States> lastStates = new List<States>();

	States GetLastState()
	{
		if (lastStates.Count > 0)
			return lastStates[lastStates.Count - 1];
		return States.Null;
	}
	void RemoveLastState()
	{
		lastStates.RemoveAt (lastStates.Count - 1);
	}
	void AddLastState(States state)
	{
		lastStates.Add(state);
	}
	void ClearLastStates()
	{
		lastStates.Clear ();
	}

	public bool IsState(States state)
	{
		return currentState == state;
	}

	public void SetState(States newState)
	{
		AddLastState(currentState);
		currentState = newState;
		ToggleUI (currentState, true);
	}

	public void GoBack()
	{
		ToggleUI (currentState, false);
		currentState = GetLastState();
		ToggleUI (currentState, true);
		RemoveLastState ();

	}

	public void ClosePreviousWindows()
	{
		foreach(States state in lastStates)
		{
			ToggleUI(state, false);
		}
		ClearLastStates ();
		ToggleUI(currentState, true);
		//lastStates.Add (currentState);
	}

	void ToggleUI(States state, bool isActivate)
	{
		if (state == States.Null)
		{
			Debug.Log ("Null global UI State occured!");
			return;
		}
		//Debug.Log ("trying to change "+state+" to "+isActivate);
		// change ui visibility depending on state
		switch (state)
		{
		case States.MainMenu: 		{MainMenu.SetActive(isActivate);		break;}
		case States.Settings:		{Settings.SetActive(isActivate);		break;}
		case States.Town: 			{Town.SetActive(isActivate);			break;}
		case States.PauseMenu: 		{PauseMenu.SetActive(isActivate);		break;}
		case States.StrategyMap: 	{StrategyMap.SetActive(isActivate);		break;}
		case States.Inventory: 		{Inventory.SetActive(isActivate);		break;}
		case States.Shop: 			{Inventory.SetActive(isActivate);		break;}
		case States.Dialogue: 		{Dialogue.SetActive(isActivate);		break;}
		case States.Road: 			{Road.SetActive(isActivate);			break;}
		case States.LoadingScreen: 	{LoadingScreen.SetActive(isActivate);	break;}
		case States.Waystation: 	{Waystation.SetActive(isActivate);		break;}
		case States.LoadGame: 		{LoadGame.SetActive(isActivate);		break;}
		}
		if (!isActivate)
			return;

		// change pause menu button visibility depending on state
		switch (state)
		{
		case States.MainMenu: 		{PauseMenuButton.SetActive(false);	break;}
		case States.Settings:		{PauseMenuButton.SetActive(false);	break;}
		case States.Town: 			{PauseMenuButton.SetActive(true);	break;}
		case States.PauseMenu: 		{PauseMenuButton.SetActive(false);	break;}
		case States.StrategyMap: 	{PauseMenuButton.SetActive(true);	break;}
		case States.Inventory: 		{PauseMenuButton.SetActive(true);	break;}
		case States.Shop: 			{PauseMenuButton.SetActive(true);	break;}
		case States.Dialogue: 		{PauseMenuButton.SetActive(true);	break;}
		case States.Road: 			{PauseMenuButton.SetActive(true);	break;}
		case States.LoadingScreen: 	{PauseMenuButton.SetActive(false);	break;}
		case States.Waystation: 	{PauseMenuButton.SetActive(true);	break;}
		case States.LoadGame: 		{PauseMenuButton.SetActive(false);	break;}
		}

		// change timer visibility depending on state
		switch (state)
		{
		case States.MainMenu: 		{Timer.SetActive(false);	break;}
		case States.Settings:		{Timer.SetActive(false);	break;}
		case States.Town: 			{Timer.SetActive(true);		break;}
		case States.PauseMenu: 		{Timer.SetActive(false);	break;}
		case States.StrategyMap: 	{Timer.SetActive(true);		break;}
		case States.Inventory: 		{Timer.SetActive(true);		break;}
		case States.Shop: 			{Timer.SetActive(true);		break;}
		case States.Dialogue: 		{Timer.SetActive(true);		break;}
		case States.Road: 			{Timer.SetActive(true);		break;}
		case States.LoadingScreen: 	{Timer.SetActive(false);	break;}
		case States.Waystation: 	{Timer.SetActive(true);		break;}
		case States.LoadGame: 		{Timer.SetActive(false);	break;}
		}

		// change time speed depending on state
		if (state == States.Road)
			GameController.reference.Resume ();
		else
			GameController.reference.Pause ();

		// change time speed depending on state
		if (state == States.Shop)
			InventorySystem.reference.OpenShop ();
		else
			InventorySystem.reference.CloseShop ();

		if (state == States.LoadGame) {
			LoadGame.GetComponent<LoadGameController> ().UpdateInfo ();
		}

		if (state == States.Town)
			GameController.reference.UnloadMap ();
		
		// if activating window, need to do something with previous window

		// do nothing if there is no last state
		if (GetLastState() == States.Null)
			return;

		// do nothing if there is the same state
		if (GetLastState() == state)
			return;

		if (state == States.MainMenu)
		{
			// main menu can't be opened with any other window at the same time 
			ClosePreviousWindows();
		}
		else if (state == States.Settings)
		{
			// settings can't be opened with any other window at the same time 
			ToggleUI(GetLastState(), false);
		}
		else if (state == States.Town)
		{
			// town can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not town 
			ToggleUI(GetLastState(), false);
		}
		else if (state == States.PauseMenu)
		{
			// pause menu can be opened with TOWN, STRATEGY_MAP, INVENTORY, SHOP, DIALOGUE and ROAD 
			if (GetLastState() != States.Town 		&&
			    GetLastState() != States.StrategyMap &&
			    GetLastState() != States.Inventory 	&&
				GetLastState() != States.Shop 	&&
				GetLastState() != States.Dialogue	&&
			    GetLastState() != States.Road)
			{
				ToggleUI(GetLastState(), false);
			}
		}
		else if (state == States.StrategyMap)
		{
			// strategy map can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not strategy map 
			ToggleUI(GetLastState(), false);
			// !!! need to remove old panels on strategy map panel
		}
		else if (state == States.Inventory)
		{
			// inventory can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not inventory
			ToggleUI(GetLastState(), false);
			// !!! need to remove old panels on inventory panel
		}
		else if (state == States.Shop)
		{
			// shop can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not shop
			ToggleUI(GetLastState(), false);
			// !!! need to remove old panels on inventory panel
		}
		else if (state == States.Dialogue)
		{
			// dialogue can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not inventory
			ToggleUI(GetLastState(), false);
			// !!! need to remove old panels on inventory panel
		}
		else if (state == States.Road)
		{
			// road can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not road
			ToggleUI(GetLastState(), false);
		}
		else if (state == States.LoadingScreen)
		{
			// loading screen can't be opened with any other window at the same time 
			ToggleUI(GetLastState(), false);
		}
		else if (state == States.Waystation)
		{
			// waystation can be opened with PAUSE_MENU, but only if we are opening PAUSE_MENU, not waystation
			ToggleUI(GetLastState(), false);
		}
		else if (state == States.LoadGame)
		{
			// load game can't be opened with any other window at the same time 
			ToggleUI(GetLastState(), false);
		}
	}
	
	void Awake()
	{
		if (reference == null) 
		{
			reference = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
