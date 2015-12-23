using UnityEngine;
using System.Collections;

public class PlayerTrain : TrainController {

	public static PlayerTrain reference = null;

	public bool canControl = true;
	public bool ghostMode = false;

    public bool nearObject = false;

	public WagonScript GetWagon(int index)
	{
		return transform.GetChild(index).GetComponent<WagonScript>();
	}

	void OnEnable()
	{
		reference = this;
		for(int wagonIndex = 0; wagonIndex < InventorySystem.reference.wagonUIs.Count; wagonIndex++)
		{
			InventorySystem.reference.wagonUIs[wagonIndex].wagonGameObject = transform.GetChild(wagonIndex+1);
			InventorySystem.reference.wagonUIs[wagonIndex].CheckSigns();
		}
	}

	void OnDisable()
	{
		reference = null;
		for(int wagonIndex = 0; wagonIndex < InventorySystem.reference.wagonUIs.Count; wagonIndex++)
		{
			InventorySystem.reference.wagonUIs[wagonIndex].wagonGameObject = null;
		}
	}

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
	

	
	float wheelSpeedValue = 0;
	public void SetWheelValue(float speed)
	{
		wheelSpeedValue = speed;
	}

    public bool speedChangedManually = false;
   

    
    
    float GetWheelSpeed()
	{
		float SpeedRange = Mathf.Abs(minSpeed) + Mathf.Abs(GetCurrentMaxSpeed());
        
        float speedValue = wheelSpeedValue * SpeedRange - Mathf.Abs(minSpeed);

        return speedValue;
	}

	public override void Stop()
	{
		SetWheelValue(0);
	}

	void Update () 
	{
		maxSpeed = PlayerSaveData.reference.trainData.GetCurrentSpeed ();

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
		InventorySystem.reference.CheckSigns ();
	}
}
