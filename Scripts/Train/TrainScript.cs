using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainScript : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;
	public float acceleration;
	public static float currentSpeed;

	void Start()
	{
		currentSpeed = 0;
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
	/*
	void Update()
	{
		if (Input.GetAxis("Vertical") > 0)
		{
			currentSpeed += acceleration*Time.deltaTime;
			if (currentSpeed > maxSpeed)
				currentSpeed = maxSpeed;
		}
		else if (Input.GetAxis("Vertical") < 0)
		{
			currentSpeed -= acceleration*Time.deltaTime;
			if (currentSpeed < minSpeed)
				currentSpeed = minSpeed;
		}
	}*/
}
