﻿using UnityEngine;
using System.Collections;

public class PlayerTrain : TrainController {

	public bool canControl = true;
	public bool ghostMode = false;

	public void ToggleGhostMode(bool value)
	{
		ghostMode = value;

		for(int wagonIndex = 0; wagonIndex < transform.childCount; wagonIndex++)
		{
			Transform wagon = transform.GetChild(wagonIndex);
			SpriteRenderer wagonSprite = wagon.GetChild(0).GetComponent<SpriteRenderer>();
			Color wagonColor = wagonSprite.color;
			if (ghostMode)
			{	
				wagonColor.a = 0.5f;
			}
			else
			{
				wagonColor.a = 1f;
			}
			wagonSprite.color = wagonColor;

			if (wagon.GetComponent<WagonScript>().sign == null)
				continue;

			SpriteRenderer signSprite = wagon.GetComponent<WagonScript>().sign.GetComponent<SpriteRenderer>();
			Color signColor = signSprite.color;
			if (ghostMode)
			{	
				signColor.a = 0.5f;
			}
			else
			{
				signColor.a = 1f;
			}
			signSprite.color = signColor;
		}
		
	}

	public override void Start () {
		base.Start ();
		if (speedWheelScrollbar != null)
			SetWheelValue (speed);
	}

	public UIScrollBar speedWheelScrollbar;
	public Transform speedWheelArrow;
	
	void RotateWheelArrow()
	{
		float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		float valueInRange = Mathf.Abs (minSpeed) + speed;
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

	public override void Stop()
	{
		SetWheelValue(0);
	}

	void Update () {
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

		if(Input.GetKeyDown("z"))
		{
			ToggleGhostMode(!ghostMode);
		}
	}
}