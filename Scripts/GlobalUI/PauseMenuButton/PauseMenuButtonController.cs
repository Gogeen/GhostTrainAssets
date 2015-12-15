using UnityEngine;
using System.Collections;

public class PauseMenuButtonController : MonoBehaviour {

	public void OpenPauseMenu()
	{
		GlobalUI.reference.SetState (GlobalUI.States.PauseMenu);
	}
}
