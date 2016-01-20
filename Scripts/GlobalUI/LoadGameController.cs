using UnityEngine;
using System.Collections;

public class LoadGameController : MonoBehaviour {

	public GameObject LoadButtonPrefab;
	public Transform ButtonsGrid;

	public void UpdateInfo()
	{
		for (int childIndex = ButtonsGrid.childCount - 1; childIndex >= 0; childIndex--) {
			Destroy (ButtonsGrid.GetChild(childIndex).gameObject);
		}
		ButtonsGrid.GetComponent<UIGrid> ().Reposition ();	
		if (PlayerSaveData.reference.IsSaveExists (1)) {
			CreateLoadButton (1);
		}
		if (PlayerSaveData.reference.IsSaveExists (2)) {
			CreateLoadButton (2);
		}
		if (PlayerSaveData.reference.IsSaveExists (3)) {
			CreateLoadButton (3);
		}
		ButtonsGrid.GetComponent<UIGrid> ().Reposition ();
	}

	void CreateLoadButton (int index)
	{
		GameObject button = Instantiate (LoadButtonPrefab) as GameObject;
		button.transform.parent = ButtonsGrid;
		button.transform.localScale = new Vector3 (1,1,1);
		button.transform.SetSiblingIndex (index);
		button.transform.GetChild (0).GetComponent<UILabel> ().text = "Сохранение "+index;

		EventDelegate action;

		action = new EventDelegate(this, "LoadGame");
		action.parameters [0].value = index;
		EventDelegate.Add(button.GetComponent<UIButton>().onClick, action);
	}

	void LoadGame(int index)
	{
		PlayerSaveData.reference.LoadData (index);
		GlobalUI.reference.ClosePreviousWindows ();
		GlobalUI.reference.SetState (GlobalUI.States.Town);
	}

	public void GoBack()
	{
		GlobalUI.reference.GoBack();
	}
}
