using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedWheelController : MonoBehaviour {

	public static SpeedWheelController reference;

	public Camera uiCamera;
	public GameObject wheelCollider;
	public Transform arrow;

	public List<UISlider> hpScrollbars = new List<UISlider> ();

	float GetWagonHP(int index)
	{
		float currentHP = 0;
		float maxHP = 0;
		if (index == 0) {
			foreach (InventoryItemObject item in PlayerSaveData.reference.trainData.equippedItems) {
				if (item == null)
					continue;
				currentHP += item.info.durabilityInfo.current;
				maxHP += item.info.durabilityInfo.max;
			}
			if (maxHP == 0)
				return 0;
			return currentHP / maxHP;
		} else {
			foreach (InventoryItemObject item in PlayerSaveData.reference.wagonData[index-1].items) {
				currentHP += item.info.durabilityInfo.current;
				maxHP += item.info.durabilityInfo.max;
			}
			if (maxHP == 0)
				return 0;
			return currentHP / maxHP;
		}
	}

	void UpdateScrollbars()
	{
		for (int wagonIndex = 0; wagonIndex < PlayerSaveData.reference.wagonData.Count + 1; wagonIndex++) {
			hpScrollbars [wagonIndex].value = GetWagonHP (wagonIndex);
		}
	}

	void Update()
	{
		if (PlayerTrain.reference == null) 
		{
			return;
		}
		UpdateScrollbars ();
		RotateArrow ();
		RaycastHit hit;
		if (Input.GetMouseButton (0)) 
		{
			if (Physics.Raycast (uiCamera.ScreenPointToRay (Input.mousePosition), out hit)) 
			{
				if (hit.collider.gameObject == wheelCollider) {
					SetPlayerSpeed (hit.point);
				}

			}
		}
		if (UICamera.hoveredObject == wheelCollider) 
		{
			
		}
	}

	public void ColorWheel(Color color)
	{
		GetComponent<UISprite> ().color = color;
	}

	void RotateArrow()
	{
		Vector3 eulerAngles = arrow.localEulerAngles;
		if (PlayerTrain.reference.GetCurrentMaxSpeed() <= 0)
		{
			eulerAngles.z = -90;
			arrow.localEulerAngles = eulerAngles;
			return;
		}
		float SpeedRange = Mathf.Abs (PlayerTrain.reference.minSpeed) + Mathf.Abs (PlayerTrain.reference.GetStartMaxSpeed());
		float valueInRange = Mathf.Abs (PlayerTrain.reference.minSpeed) + PlayerTrain.reference.speed;
		eulerAngles.z = -(2 * Mathf.Clamp(valueInRange/SpeedRange,0,1) - 1) * 90;
		arrow.localEulerAngles = eulerAngles;
	}
	
	float GetAngleBetween(Vector2 first, Vector2 second)
	{
		float angle = Vector2.Angle(first, second);
		Vector3 cross = Vector3.Cross(first, second);
		if (cross.z > 0) 
		{
			angle = -angle;
		}
		return angle;
	}

	
	float GetVectorRotation(Vector2 direction)
	{
		return -GetAngleBetween(Vector2.up, direction);
	}
	
	void SetPlayerSpeed(Vector3 hitPoint)
	{
		
		PlayerTrain.reference.SetWheelValue( (GetVectorRotation (hitPoint - arrow.position) - 90) / (-180 * ( (100 - PlayerTrain.reference.speedDebuffPercent) / 100f ) ) );

		PlayerTrain.reference.speedChangedManually = true;
		
	}

	public void OpenInventory()
	{
		GlobalUI.reference.SetState (GlobalUI.States.Inventory);
	}
}
