using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainController : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;
	public float acceleration;
	public float speedDebuffPercent = 0;

	public float speed;

	//float startMaxSpeed;

	public float GetStartMaxSpeed()
	{
		return maxSpeed;
	}

	public float GetCurrentMaxSpeed()
	{
		return maxSpeed*(100 - speedDebuffPercent)/100;
	}

	public virtual void Start()
	{
		//startMaxSpeed = maxSpeed;

		if (minSpeed > 0)
			speed = minSpeed;
		else
			speed = 0;
	}

	public void AccelerateTo(float value)
	{
		StopCoroutine ("accelerateTo");
		StartCoroutine ("accelerateTo", value);
	}

	private IEnumerator accelerateTo(float value)
	{
		while(speed != value)
		{
			if (Mathf.Abs (speed - value) < acceleration*Time.deltaTime)
			{
				speed = value;
				break;
			}
			if (speed < value)
				speed += acceleration*Time.deltaTime;
			else
				speed -= acceleration*Time.deltaTime;

			if (speed < minSpeed)
			{
				speed = minSpeed;
				break;
			} else if (speed > GetCurrentMaxSpeed())
			{
				speed = GetCurrentMaxSpeed();
				break;
			}

			yield return null;

		}
		yield return null;
	}



	public virtual void Stop()
	{
		AccelerateTo(0);
	}

	void Update()
	{

	}
}
