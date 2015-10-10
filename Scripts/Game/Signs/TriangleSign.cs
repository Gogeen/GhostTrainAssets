using UnityEngine;
using System.Collections;

public class TriangleSign : Sign {

	public float duration;
	[Range(1,100)]public int strength;
	public override IEnumerator Cast(PlayerTrain playerTrain)
	{
        
		if (roadFeature != null)
			roadFeature.stopFeature = true;

		playerTrain.canControl = false;
		playerTrain.wheelImage.color = Color.red;
		playerTrain.maxSpeed += playerTrain.GetStartMaxSpeed() * (float)strength / 100;

		playerTrain.AccelerateTo (playerTrain.maxSpeed);
		yield return new WaitForSeconds (duration);

		playerTrain.maxSpeed -= playerTrain.GetStartMaxSpeed() * (float)strength / 100;
		playerTrain.wheelImage.color = Color.white;
		playerTrain.canControl = true;

		if (roadFeature != null)
			roadFeature.stopFeature = false;

	}
}
