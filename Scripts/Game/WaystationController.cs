using UnityEngine;
using System.Collections;

public class WaystationController : MonoBehaviour {

	public GameObject waystationPanel;
	public void Repair()
	{
		TrainTimeScript.reference.AddTime (-15);
		Debug.Log ("repair part here");
	}
	public void DeactivateButton(GameObject button)
	{
		button.GetComponent<BoxCollider> ().enabled = false;
	}

	public void ContinueTravel()
	{
		GameController.Resume();
		waystationPanel.SetActive(false);

	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			PlayerTrain.reference.nearObject = true;
			if (PlayerTrain.reference.speed == 0)
			{
				GameController.Pause();
				TrainTimeScript.reference.ComeInWaystation();
				waystationPanel.SetActive(true);
				Debug.Log ("open waystation menu");
				GetComponent<BoxCollider2D>().enabled = false;
			}
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
