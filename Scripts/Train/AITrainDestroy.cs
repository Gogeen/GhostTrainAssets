using UnityEngine;
using System.Collections;

public class AITrainDestroy : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("enemy"))
		{
			Destroy (coll.transform.parent.gameObject);
		}
	}
}
