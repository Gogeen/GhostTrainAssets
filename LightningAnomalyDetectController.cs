using UnityEngine;
using System.Collections;

public class LightningAnomalyDetectController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			transform.parent.GetComponent<LightningAnomaly>().Activate();
		}
	}
}
