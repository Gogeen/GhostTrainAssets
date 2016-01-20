using UnityEngine;
using System.Collections;
//using UnityEditor;

public class TrainEventManager : MonoBehaviour {
	
	public Camera trainCamera;
	public float ghostModeClickTime;
	public float ghostModeCooldown;
	public float ghostModeDuration;
	public int ghostModeCost;

	float ghostModeCooldownTimer = 0;
	public string ghostModeKey;
	public string stopKey;
	float clickTimer = 0;

	bool CanUseGhostMode()
	{
		return ghostModeCooldownTimer <= 0;
	}

	void UseGhostMode()
	{
		PlayerTrain.reference.UseGhostMode (ghostModeDuration);
		ghostModeCooldownTimer = ghostModeCooldown;
		TrainTimeScript.reference.AddTime (-ghostModeCost);
	}

	void Update () 
	{
		if (ghostModeCooldownTimer > 0)
			ghostModeCooldownTimer -= Time.deltaTime;

		if(Input.GetKeyDown(ghostModeKey))
		{
			if (CanUseGhostMode ())
				UseGhostMode ();
		}

		if (Input.GetKeyDown(stopKey))
		{
			GetComponent<PlayerTrain>().Stop();
		}

		if (Input.GetMouseButtonDown(0))
		{
			clickTimer = 0;
		}

		if (Input.GetMouseButton (0)) {
			RaycastHit2D hit = Physics2D.GetRayIntersection (trainCamera.ScreenPointToRay (Input.mousePosition),100, 1 << LayerMask.NameToLayer("player"));
			if (hit.collider != null) {
				GameObject wagon = hit.collider.gameObject;
				if (wagon.GetComponent<WagonScript> ().isHead) {
					clickTimer += Time.deltaTime;
					if (clickTimer >= ghostModeClickTime){
						if (!GlobalUI.reference.IsState(GlobalUI.States.Inventory))
							GlobalUI.reference.SetState (GlobalUI.States.Inventory);
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.GetRayIntersection(trainCamera.ScreenPointToRay(Input.mousePosition),100, 1 << LayerMask.NameToLayer("player"));
			if (hit.collider != null) {
				GameObject wagon = hit.collider.gameObject;
				if (!wagon.GetComponent<WagonScript> ().isHead) {
					if (GetComponent<SignsController> ().CanCast (wagon.GetComponent<WagonScript> ().signType)) {
						wagon.GetComponent<Animator> ().Play ("wagonAnim");
						wagon.GetComponent<WagonScript> ().CastSign ();
					}
				} 
				else if (clickTimer < ghostModeClickTime) {
					if (CanUseGhostMode ())
						UseGhostMode ();
				}
			}
			hit = Physics2D.GetRayIntersection(trainCamera.ScreenPointToRay(Input.mousePosition),100, 1 << LayerMask.NameToLayer("objectToStop"));
			if (hit.collider != null){
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("objectToStop")){
					GetComponent<PlayerTrain>().StopNearNextObject();
				}
			}
		}
	}
}
