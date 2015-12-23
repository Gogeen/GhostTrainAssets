using UnityEngine;
using System.Collections;

public class TownRepairAction : TownAction {

	public override int GetTime()
	{
		return InventorySystem.reference.GetTotalRepairCost ();
	}
}
