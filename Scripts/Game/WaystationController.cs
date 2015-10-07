using UnityEngine;
using System.Collections;

public class WaystationController : MonoBehaviour {

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain controller = coll.transform.parent.GetComponent<PlayerTrain>();
            controller.nearObject = true;
            if (controller.speed == 0)
				Debug.Log ("open waystation menu");
		}

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
        if (coll.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            if (coll.gameObject.GetComponent<WagonScript>().isHead)
            {
                EnemyTrain controller = coll.transform.parent.GetComponent<EnemyTrain>();
                controller.StopFor(controller.waystationTime);
            }
        }
    }
}
