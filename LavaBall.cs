using UnityEngine;
using System.Collections;

public class LavaBall : MonoBehaviour {

	public Vector3 velocity;
	public float liveRange;

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("player")) {
			transform.parent.GetComponent<LavaObject> ().OnTrigger (coll);
		}
	}

	void Update()
	{
		transform.position += velocity * Time.deltaTime;
		liveRange -= Time.deltaTime;
		if (liveRange <= 0)
			Destroy (gameObject);
	}
}
