using UnityEngine;
using System.Collections;

public class ItemEfficiencyWithTimeSystem : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		foreach (InventoryItemObject item in PlayerSaveData.reference.trainData.equippedItems) {
			if (item == null)
				continue;
			if (item.info.extraInfo.MoreEffectiveWithTime) {
				
				item.AddEfficiency(Time.deltaTime);
			}
		}
	}
}
