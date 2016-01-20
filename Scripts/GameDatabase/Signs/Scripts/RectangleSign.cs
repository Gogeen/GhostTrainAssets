using UnityEngine;
using System.Collections;

public class RectangleSign : Sign {

	public override IEnumerator Cast(PlayerTrain playerTrain)
	{
		if (roadFeature != null)
			roadFeature.stopFeature = true;
		foreach (GameObject pile in playerTrain.GetPilesNear()) {
			pile.GetComponent<RoadPile>().hitPoints -= 1;
		}
		playerTrain.ClearPilesNear ();
		yield return null;
	}
}
