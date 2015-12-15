using UnityEngine;
using System.Collections;

public class RectangleSign : Sign {

	public override IEnumerator Cast(PlayerTrain playerTrain)
	{
		if (roadFeature != null)
			roadFeature.stopFeature = true;
		yield return null;
	}
}
