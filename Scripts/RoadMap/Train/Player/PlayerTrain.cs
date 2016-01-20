using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTrain : TrainController {

	public static PlayerTrain reference = null;

	public GameObject GhostModeSign;

    public bool nearObject = false;

	List<GameObject> pilesNear = new List<GameObject> ();

	public List<GameObject> GetPilesNear()
	{
		for(int index = pilesNear.Count-1; index >= 0; index--)
			if (pilesNear[index] == null)
				pilesNear.RemoveAt (index);
		return pilesNear;
	}

	public void AddPileNear(GameObject pile)
	{
		pilesNear.Add (pile);
	}

	public void RemovePileNear(GameObject pile)
	{
		if (pilesNear.Contains(pile))
			pilesNear.Remove (pile);
	}

	public void ClearPilesNear()
	{
		pilesNear.Clear ();
	}

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

	public void UseGhostMode(float duration)
	{
		GetComponent<SignsController> ().RemoveCooldowns (duration);

		GhostModeSign.GetComponent<Animator> ().Play ("cooldown");
		GhostModeSign.GetComponent<Animator> ().speed = 1 / GetComponent<TrainEventManager>().ghostModeCooldown;
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

		if (!PlayerSaveData.reference.trainData.conditions.LostControl)
        {
            if (speedChangedManually)
            {
                StopCoroutine("stopNearNextObject");
                speedChangedManually = false;
            }
            AccelerateTo(GetWheelSpeed());
        }

		InventorySystem.reference.CheckSigns ();
		acceleration = GetCurrentMaxSpeed ();

	}
}
