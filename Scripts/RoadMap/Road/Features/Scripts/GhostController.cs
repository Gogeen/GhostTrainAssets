using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public GhostsFeature feature;

	public void FinishMove()
	{
		feature.FinishGhostMove ();
	}
}
