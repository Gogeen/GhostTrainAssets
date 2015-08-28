using UnityEngine;
using System.Collections;

public class WagonConnector : MonoBehaviour {

	public Transform nextWagon;
	public float range;
	void Update()
	{
		if (nextWagon == null)
		{
			return;
		}
		if ((GetComponent<WagonScript>().backWheel.position - nextWagon.position).magnitude != range)
		{
			float additionalRange = (GetComponent<WagonScript>().backWheel.position - nextWagon.position).magnitude - range;
			nextWagon.position += (GetComponent<WagonScript>().backWheel.position - nextWagon.position).normalized * additionalRange;
		}

	}
}
