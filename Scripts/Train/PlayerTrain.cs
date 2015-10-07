using UnityEngine;
using System.Collections;

public class PlayerTrain : TrainController {

	public bool canControl = true;
	public bool ghostMode = false;

    public bool nearObject = false;

    public void StopNearNextObject()
    {
        StopCoroutine("stopNearNextObject");
        StartCoroutine("stopNearNextObject");
    }

    IEnumerator stopNearNextObject()
    {
        RoadFeatureController featuresController = GameObject.Find("Map").GetComponent<RoadFeatureController>();
        featuresController.ToggleFeatures(false);
        nearObject = false;
        while (!nearObject)
        {
            yield return null;
        }
        nearObject = false;
        Stop();
    }

    public void ToggleGhostMode(bool value)
	{
		ghostMode = value;

		for(int wagonIndex = 0; wagonIndex < transform.childCount; wagonIndex++)
		{
            Transform wagon = transform.GetChild(wagonIndex);
            if (wagon.GetComponent<WagonScript>() == null)
                continue;
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
    bool speedChangedManually = false;
    float scrollbarSpeedValue = 0;
	float GetWheelSpeed()
	{
		float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		float speedValue = speedWheelScrollbar.value * SpeedRange - Mathf.Abs (minSpeed);
        if (scrollbarSpeedValue != speedWheelScrollbar.value)
        {
            scrollbarSpeedValue = speedWheelScrollbar.value;
            speedChangedManually = true;
            Debug.Log("manually changed speed");
        }
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
            {
                if (speedChangedManually)
                {
                    StopCoroutine("stopNearNextObject");
                    speedChangedManually = false;
                }
                AccelerateTo(GetWheelSpeed());
            }
		    
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
