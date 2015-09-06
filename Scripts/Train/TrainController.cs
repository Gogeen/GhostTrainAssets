using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainController : MonoBehaviour {
	public bool canControl = true;
	public float minSpeed;
	public float maxSpeed;
	public float acceleration;
	public static float currentSpeed;
	public static float currentAcceleration;

	float startMinSpeed;
	float startMaxSpeed;

	public bool IsSlowed()
	{
		return (maxSpeed < startMaxSpeed);
	}

	void Start()
	{
		startMinSpeed = minSpeed;
		startMaxSpeed = maxSpeed;

		currentSpeed = 0;
		currentAcceleration = acceleration;
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
			if (IsSlowed())
			{
				if (currentSpeed < startMinSpeed)
				{
					currentSpeed = startMinSpeed;
					break;
				} else if (currentSpeed > startMaxSpeed)
				{
					currentSpeed = startMaxSpeed;
					break;
				}
			}
			else 
			{
				if (currentSpeed < minSpeed)
				{
					currentSpeed = minSpeed;
					break;
				} else if (currentSpeed > maxSpeed)
				{
					currentSpeed = maxSpeed;
					break;
				}
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
		eulerAngles.z = -(2 * Mathf.Clamp(valueInRange/SpeedRange,0,1) - 1) * 90;
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

	public void Stop()
	{
		SetWheelValue(0);
	}

	void Update()
	{
		if (speedWheelArrow!= null)
		{
			RotateWheelArrow();
		}
		if (speedWheelScrollbar != null)
		{
			if (canControl)
				AccelerateTo(GetWheelSpeed());
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Stop();
		}
	}
}
