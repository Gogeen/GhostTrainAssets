using UnityEngine;
using System.Collections;

public class SpeedWheelController : MonoBehaviour {

	public Camera uiCamera;
	public GameObject wheelCollider;
	public Transform arrow;

	void Update()
	{
		if (PlayerTrain.reference == null) 
		{
			return;
		}
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
		

	void RotateArrow()
	{
		Vector3 eulerAngles = arrow.localEulerAngles;
		if (PlayerTrain.reference.GetCurrentMaxSpeed() <= 0)
		{
			eulerAngles.z = -90;
			arrow.localEulerAngles = eulerAngles;
			return;
		}
		float SpeedRange = Mathf.Abs (PlayerTrain.reference.minSpeed) + Mathf.Abs (PlayerTrain.reference.GetCurrentMaxSpeed());
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
		
		PlayerTrain.reference.SetWheelValue((GetVectorRotation(hitPoint - arrow.position) - 90) / -180);

		PlayerTrain.reference.speedChangedManually = true;
		
	}
	
}
