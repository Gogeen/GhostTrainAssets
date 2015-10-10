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
		SetWheelValue (speed);
	}

    public Camera UIcamera;
    public UISprite wheelImage;
    public GameObject wheelCollider;
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
        /*float speedValue = Mathf.Abs (minSpeed) + speed;
		float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		speedWheelScrollbar.value = speedValue/SpeedRange;*/
        wheelSpeedValue = speed;
    }
    bool speedChangedManually = false;
    //float scrollbarSpeedValue = 0;
    float GetAngleBetween(Vector2 first, Vector2 second)
    {
        float angle = Vector2.Angle(first, second);
        Vector3 cross = Vector3.Cross(first, second);
        if (cross.z > 0)
            angle = -angle;
        return angle;
    }

    float GetVectorRotation(Vector2 direction)
    {
        return -GetAngleBetween(Vector2.up, direction);
    }
    float wheelSpeedValue = 0;
    float GetWheelSpeed()
	{
        float SpeedRange = Mathf.Abs(minSpeed) + Mathf.Abs(maxSpeed);
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(UIcamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject == wheelCollider)
                {
                    wheelSpeedValue = (GetVectorRotation(hit.point - speedWheelArrow.position) - 90) / -180;
                    speedChangedManually = true;
                    
                }
            }
        }
        float speedValue = wheelSpeedValue * SpeedRange - Mathf.Abs(minSpeed);

        return speedValue;
		/*float SpeedRange = Mathf.Abs (minSpeed) + Mathf.Abs (maxSpeed);
		float speedValue = speedWheelScrollbar.value * SpeedRange - Mathf.Abs (minSpeed);
        if (scrollbarSpeedValue != speedWheelScrollbar.value)
        {
            scrollbarSpeedValue = speedWheelScrollbar.value;
            speedChangedManually = true;
            Debug.Log("manually changed speed");
        }
		return speedValue;*/
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
		if (canControl)
        {
            if (speedChangedManually)
            {
                StopCoroutine("stopNearNextObject");
                speedChangedManually = false;
            }
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
