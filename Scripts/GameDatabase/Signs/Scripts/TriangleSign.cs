using UnityEngine;
using System.Collections;

public class TriangleSign : Sign {

	public float duration;
	[Range(1,100)]public int strength;
	public override IEnumerator Cast(PlayerTrain playerTrain)
	{
        
		if (roadFeature != null)
			roadFeature.stopFeature = true;

		PlayerSaveData.reference.trainData.conditions.LostControl = true;
		SpeedWheelController.reference.ColorWheel(Color.red);
		playerTrain.speedDebuffPercent -= strength;

		playerTrain.AccelerateTo (playerTrain.GetCurrentMaxSpeed());
		yield return new WaitForSeconds (duration);

		playerTrain.speedDebuffPercent += strength;
		SpeedWheelController.reference.ColorWheel(Color.white);
		PlayerSaveData.reference.trainData.conditions.LostControl = false;

		if (roadFeature != null)
			roadFeature.stopFeature = false;

	}
}
