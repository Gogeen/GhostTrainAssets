using UnityEngine;
using System.Collections;

public class TrainEventManager : MonoBehaviour {
	
	public Camera trainCamera;

	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.GetRayIntersection(trainCamera.ScreenPointToRay(Input.mousePosition));
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("player"))
				{
					GameObject wagon = hit.collider.gameObject;
					if (!wagon.GetComponent<WagonScript>().isHead)
					{
						if (!GameController.IsPaused())
							wagon.GetComponent<Animator>().Play("wagonAnim");
					}
					else
					{
						GlobalUI.SetState(GlobalUI.States.Inventory);
					}
				}
			}
		}
	}
}
