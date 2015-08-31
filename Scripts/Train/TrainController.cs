using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainController : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;
	public float acceleration;
	public static float currentSpeed;

	void Start()
	{
		currentSpeed = 0;
		if (speedWheelScrollbar != null)
			SetWheelValue (currentSpeed);
	}

	public void AccelerateTo(float value)
	{
		StopCoroutine ("accelerateTo");
		StartCoroutine ("accelerateTo", value);
	}

	private IEnumerator accelerateTo(float value)
	{
		while(currentSpeed != value)
		{
			if (Mathf.Abs (currentSpeed - value) < acceleration*Time.deltaTime)
			{
				currentSpeed = value;
				break;
			}
			if (currentSpeed < value)
				currentSpeed += acceleration*Time.deltaTime;
			else
				currentSpeed -= acceleration*Time.deltaTime;

			if (currentSpeed < minSpeed)
			{
				currentSpeed = minSpeed;
				break;
			} else if (currentSpeed > maxSpeed)
			{
				currentSpeed = maxSpeed;
				break;
			}
			yield return null;

		}
		yield return null;
	}

	public UIScrollBar speedWheelScrollbar;
	public Transform speedWheelArrow;

	void RotateWheelArrow()
	{
		float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		float valueInRange = Mathf.Abs (minSpeed) + currentSpeed;
		Vector3 eulerAngles = speedWheelArrow.localEulerAngles;
		eulerAngles.z = -(2 * valueInRange/SpeedRange - 1) * 90;
		speedWheelArrow.localEulerAngles = eulerAngles;
	}

	void SetWheelValue(float speed)
	{
		float speedValue = Mathf.Abs (minSpeed) + speed;
		float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		speedWheelScrollbar.value = speedValue/SpeedRange;
	}

	float GetWheelSpeed()
	{
		float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		float speedValue = speedWheelScrollbar.value * SpeedRange - Mathf.Abs (minSpeed);
		return speedValue;
	}

	void Update()
	{
		if (speedWheelArrow!= null)
		{
			RotateWheelArrow();
		}
		if (speedWheelScrollbar != null)
		{
			AccelerateTo(GetWheelSpeed());
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetWheelValue(0);
		}
	}
}
