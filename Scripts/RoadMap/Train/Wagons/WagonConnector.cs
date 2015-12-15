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
		if ((transform.position - nextWagon.position).magnitude != range)
		{
			float additionalRange = (transform.position - nextWagon.position).magnitude - range;
			nextWagon.position += (transform.position - nextWagon.position).normalized * additionalRange;
		}

	}
}
