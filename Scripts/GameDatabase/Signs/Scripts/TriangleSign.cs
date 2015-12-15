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
		//playerTrain.wheelImage.color = Color.red;
		playerTrain.speedDebuffPercent -= strength;

		playerTrain.AccelerateTo (playerTrain.GetCurrentMaxSpeed());
		yield return new WaitForSeconds (duration);

		playerTrain.speedDebuffPercent += strength;
		//playerTrain.wheelImage.color = Color.white;
		playerTrain.canControl = true;

		if (roadFeature != null)
			roadFeature.stopFeature = false;

	}
}
