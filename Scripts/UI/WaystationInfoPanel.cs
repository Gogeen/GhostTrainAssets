using UnityEngine;
using System.Collections;

public class WaystationInfoPanel : MonoBehaviour {

	public StrategyMapRoadObject reference;
	public UIToggle tradeUI;
	public UIToggle repairUI;

	public void SetReference(StrategyMapRoadObject objectReference)
	{
		reference = objectReference;
		SetTradeUI (reference.isTrade);
		SetRepairUI (reference.isRepair);

	}

	void SetTradeUI(bool value)
	{
		tradeUI.value = value;
	}

	void SetRepairUI(bool value)
	{
		repairUI.value = value;
	}

	void Update()
	{
		if (reference != null)
		{
			reference.SetTrade(tradeUI.value);
			reference.SetRepair(repairUI.value);

		}
	}
}
