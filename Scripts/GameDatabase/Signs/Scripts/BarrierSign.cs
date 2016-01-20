using UnityEngine;
using System.Collections;

public class BarrierSign : Sign {

	public float duration;
	public float repairValue;
	public override IEnumerator Cast(PlayerTrain playerTrain)
	{

		if (roadFeature != null)
			roadFeature.stopFeature = true;

		PlayerSaveData.reference.trainData.conditions.Shield = true;
		InventorySystem.reference.RepairWholeInventory (repairValue, true);
		yield return new WaitForSeconds (duration);
		PlayerSaveData.reference.trainData.conditions.Shield = false;

	}
}
