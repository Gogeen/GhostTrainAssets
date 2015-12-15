using UnityEngine;
using System.Collections;

public class WaystationController : MonoBehaviour {

	public void Repair()
	{
		//TrainTimeScript.reference.AddTime (-15);
		InventorySystem.reference.RepairWholeInventory ();
	}

	public void Trade()
	{
		GlobalUI.reference.SetState (GlobalUI.States.Shop);
	}

	public void DeactivateButton(GameObject button)
	{
		button.GetComponent<BoxCollider> ().enabled = false;
	}

	public void ContinueTravel()
	{
		GlobalUI.reference.GoBack ();
	}


}
