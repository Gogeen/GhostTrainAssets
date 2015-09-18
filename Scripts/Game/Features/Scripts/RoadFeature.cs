using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoadFeature : ScriptableObject {

	public bool turnedOn = true;
	public bool stopFeature = false;

	public void ToggleFeature(bool value)
	{
		turnedOn = value;
	}

	public virtual void OnUpdate()
	{

	}

	public virtual void OnStart()
	{
		
	}

	public virtual bool CanCast()
	{
		return false;
	}

	public virtual IEnumerator Cast(PlayerTrain playerTrain)
	{
		yield return null;
	}

	public virtual void Stop()
	{
		stopFeature = true;
	}
}
