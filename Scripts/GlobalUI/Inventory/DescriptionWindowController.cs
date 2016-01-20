using UnityEngine;
using System.Collections;

public class DescriptionWindowController : MonoBehaviour {

	public bool IsCompare = false;
	public UIGrid grid;

	public UIStat Name;
	public UIStat Price;
	public UIStat Weight;
	public UIStat PassengerSpace;
	public UIStat Corruption;
	public UIStat BonusPower;
	public UIStat BonusMaxWeight;
	public UIStat BonusLuxury;
	public UIStat BonusMaxPassengerSpace;
	public UIStat BonusMagicPower;
	public UIStat BonusMaxSpeed;
	public UIStat BonusEquipmentCorruption;
	public UIStat BonusTradePrices;
	public UIStat BonusRepairPrices;
	public UIStat Description;

	bool IsShowStatInfo(float stat)
	{
		if (stat != 0)
			return true;
		return false;
	}

	bool HaveBonusDurability(InventoryItemObject item)
	{
		if (PlayerSaveData.reference.trainData.bonusEquipmentDurability <= 0)
			return false;
		if (InventorySystem.reference.GetSlotInfo (item.GetLastSlot ()).type == InventorySystem.SlotType.Shop)
			return false;
		return true;
	}

	bool HavePenaltyDurability(InventoryItemObject item)
	{
		if (PlayerSaveData.reference.trainData.bonusEquipmentDurability >= 0)
			return false;
		if (InventorySystem.reference.GetSlotInfo (item.GetSlot ()).type == InventorySystem.SlotType.Shop)
			return false;
		return true;
	}

	public void ShowItemDescription (InventoryItemObject itemObject, Vector3 offset = new Vector3())
	{
		gameObject.SetActive (true);
		InventoryItem info = itemObject.info;

		int siblingIndex = 0;

		Name.value.text = info.name;
		if (Name.gameObject.activeSelf) {
			Name.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		//Price.gameObject.SetActive (IsShowStatInfo (InventorySystem.reference.GetItemTotalPrice (itemObject)));
		Price.value.text = InventorySystem.reference.GetItemTotalPrice (itemObject).ToString ();
		if (Price.gameObject.activeSelf) {
			Price.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}
			
		Weight.value.text = info.costInfo.weight.ToString ();
		if (Weight.gameObject.activeSelf) {
			Weight.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		PassengerSpace.gameObject.SetActive (IsShowStatInfo (info.costInfo.passengerSpace));
		PassengerSpace.value.text = info.costInfo.passengerSpace.ToString ();
		if (PassengerSpace.gameObject.activeSelf) {
			PassengerSpace.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		Corruption.value.text = Mathf.Round((info.durabilityInfo.current*10f))/10f + " /";
		Corruption.penalty.text = info.durabilityInfo.max.ToString();
		if (HaveBonusDurability(itemObject))
			Corruption.penalty.color = Color.green;
		else if (HavePenaltyDurability(itemObject))
			Corruption.penalty.color = Color.red;
		else
			Corruption.penalty.color = Color.black;
		if (Corruption.gameObject.activeSelf) {
			Corruption.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusPower.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.power));
		BonusPower.value.text = info.bonusInfo.power.ToString ();
		if (BonusPower.gameObject.activeSelf) {
			BonusPower.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusMaxWeight.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.maxWeight));
		BonusMaxWeight.value.text = info.bonusInfo.maxWeight.ToString ();
		if (BonusMaxWeight.gameObject.activeSelf) {
			BonusMaxWeight.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusLuxury.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.attraction));
		BonusLuxury.value.text = info.bonusInfo.attraction.ToString ();
		if (BonusLuxury.gameObject.activeSelf) {
			BonusLuxury.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusMaxPassengerSpace.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.maxPassengerSpace));
		BonusMaxPassengerSpace.value.text = info.bonusInfo.maxPassengerSpace.ToString ();
		if (BonusMaxPassengerSpace.gameObject.activeSelf) {
			BonusMaxPassengerSpace.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusMagicPower.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.magicPower));
		BonusMagicPower.value.text = info.bonusInfo.magicPower.ToString ();
		if (BonusMagicPower.gameObject.activeSelf) {
			BonusMagicPower.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusMaxSpeed.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.maxSpeed));
		BonusMaxSpeed.value.text = info.bonusInfo.maxSpeed.ToString ();
		if (BonusMaxSpeed.gameObject.activeSelf) {
			BonusMaxSpeed.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusEquipmentCorruption.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.equipmentDurability));
		BonusEquipmentCorruption.value.text = info.bonusInfo.equipmentDurability.ToString ()+"%";
		if (BonusEquipmentCorruption.gameObject.activeSelf) {
			BonusEquipmentCorruption.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusTradePrices.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.tradePricePercent));
		BonusTradePrices.value.text = info.bonusInfo.tradePricePercent.ToString ()+"%";
		if (BonusTradePrices.gameObject.activeSelf) {
			BonusTradePrices.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		BonusRepairPrices.gameObject.SetActive (IsShowStatInfo (info.bonusInfo.repairPricePercent));
		BonusRepairPrices.value.text = info.bonusInfo.repairPricePercent.ToString ()+"%";
		if (BonusRepairPrices.gameObject.activeSelf) {
			BonusRepairPrices.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}

		Description.value.text = info.uiInfo.description;
		if (Description.gameObject.activeSelf) {
			Description.transform.SetSiblingIndex (siblingIndex);
			siblingIndex += 1;
		}
			

		grid.Reposition ();




		/*
		if (isCompareWindow)
		{
			CompareWindow.transform.position = DescriptionWindow.transform.position;
			CompareWindow.transform.localPosition -= new Vector3(CompareWindow.transform.FindChild("Background").GetComponent<UISprite>().localSize.x,0,0);
		}
		else
		{*/
		// set window near mouse
		transform.position = UICamera.lastHit.point;
			

		transform.localPosition += new Vector3 (10, 10, 0);
		transform.localPosition += new Vector3 (0,transform.FindChild("Background").GetComponent<UISprite>().localSize.y,0);

		if (IsCompare) {
			transform.localPosition = offset;
			transform.localPosition -= new Vector3 (transform.FindChild ("Background").GetComponent<UISprite> ().localSize.x, 0, 0);
			return;
		}

		UIRoot root = null;
		Transform currentObject = transform;
		while (true) {
			if (currentObject.GetComponent<UIRoot> ()) {
				root = currentObject.GetComponent<UIRoot> ();
				break;
			}
			currentObject = currentObject.parent;
		}
		//Debug.Log (root.manualWidth);
		if (transform.localPosition.x - 10 + transform.FindChild ("Background").GetComponent<UISprite> ().localSize.x > root.manualWidth / 2) {
			transform.localPosition += new Vector3 (-transform.FindChild ("Background").GetComponent<UISprite> ().localSize.x, 0, 0);
		}
		if (transform.localPosition.y + 10 > root.manualHeight / 2) {
			transform.localPosition += new Vector3 (0, -transform.FindChild ("Background").GetComponent<UISprite> ().localSize.y - 60, 0);
		}
	}

	public void HideItemDescription()
	{
		gameObject.SetActive (false);
		//CompareWindow.SetActive(false);

	}
}
