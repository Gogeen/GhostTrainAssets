using UnityEngine;
using System.Collections;
using System;

public class GlobalUI : MonoBehaviour {

	public GameObject gameUI;
	public GameObject StrategyMapUI;

	public enum States
	{
		Game,
		Inventory,
		StrategyMap
	}
	static States currentState;
	States lastState;
	public static void SetState(States state)
	{
		currentState = state;
	}

	public void SetInventoryState()
	{
		SetState(States.Inventory);
	}

	public void SetGameState()
	{
		SetState(States.Game);
	}

	public void PauseGame()
	{
		GameController.Pause ();
	}

	public void ResumeGame()
	{
		GameController.Resume ();
	}

	public void ExitGame()
	{
		GameController.Quit ();
	}

	void ActivateUI(bool isActivate, States state)
	{
		if (state == States.Game)
		{
			gameUI.SetActive(isActivate);
			if (isActivate)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
		else if (state == States.Inventory)
		{
			InventorySystem.reference.ToggleUI();
		}
		else if (state == States.StrategyMap)
		{
			StrategyMapUI.SetActive(isActivate);
		}
	}

	void Start()
	{
		SetState(States.Game);
		/*lastState = currentState;
		Array states = Enum.GetValues (typeof(States));
		foreach(States state in states)
		{
			ActivateUI(false, state);
		}*/
		ActivateUI(true, currentState);
	}

	void Update()
	{
		/*if (currentState != lastState)
		{
			ActivateUI(false, lastState);
			ActivateUI(true, currentState);
			lastState = currentState;
		}*/
	}
}
