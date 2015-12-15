using UnityEngine;
using System.Collections;

public class WaystationInfoPanel : MonoBehaviour {

	public StrategyMapRoadObject reference;
	public UISprite background;
	public UIToggle tradeUI;
	public UIWidget tradeCheckboxWidget;
	public UISprite tradeCheckbox;
	public UISprite tradeChecker;
	public UILabel tradeLabel;
	public UIToggle repairUI;
	public UIWidget repairCheckboxWidget;
	public UISprite repairCheckbox;
	public UISprite repairChecker;
	public UILabel repairLabel;

	public void SetReference(StrategyMapRoadObject objectReference, int depth)
	{
		reference = objectReference;

		background.depth = depth;
		tradeCheckboxWidget.depth = depth + 1;
		tradeCheckbox.depth = depth + 1;
		tradeChecker.depth = depth + 2;
		tradeLabel.depth = depth + 1;

		repairCheckboxWidget.depth = depth + 1;
		repairCheckbox.depth = depth + 1;
		repairChecker.depth = depth + 2;
		repairLabel.depth = depth + 1;

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
