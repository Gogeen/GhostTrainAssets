using UnityEngine;
using System.Collections;

public class SpeedWheelControllerScript : MonoBehaviour {
	
	public TrainScript trainScript;
	public UIScrollBar scrollBar;

	private float speedWheelValue;

	void Start () {
		float speedValue = Mathf.Abs (trainScript.minSpeed);
		float SpeedRange = Mathf.Abs (trainScript.minSpeed) + Mathf.Abs (trainScript.maxSpeed);
		scrollBar.scrollValue = speedValue/SpeedRange;
		trainScript.AccelerateTo (speedValue);
	}

	void Update () {
		if (speedWheelValue != scrollBar.scrollValue) 
		{
			speedWheelValue = scrollBar.scrollValue;
			float SpeedRange = Mathf.Abs (trainScript.minSpeed) + Mathf.Abs (trainScript.maxSpeed);
			float speedValue = speedWheelValue * SpeedRange - Mathf.Abs (trainScript.minSpeed);
			trainScript.AccelerateTo (speedValue);
		}
	}
}
