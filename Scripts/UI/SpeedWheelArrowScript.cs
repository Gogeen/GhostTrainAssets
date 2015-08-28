using UnityEngine;
using System.Collections;

public class SpeedWheelArrowScript : MonoBehaviour {

	public TrainScript trainScript;
	// Update is called once per frame
	void Update () {
		float SpeedRange = Mathf.Abs (trainScript.minSpeed) + Mathf.Abs (trainScript.maxSpeed);
		float valueInRange = Mathf.Abs (trainScript.minSpeed) + TrainScript.currentSpeed;
		Vector3 eulerAngles = transform.localEulerAngles;
		eulerAngles.z = -(2 * valueInRange/SpeedRange - 1) * 90;
		transform.localEulerAngles = eulerAngles;
	}
}
