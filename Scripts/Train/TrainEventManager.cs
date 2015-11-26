using UnityEngine;
using System.Collections;
//using UnityEditor;

public class TrainEventManager : MonoBehaviour {
	
	public Camera trainCamera;
	public float ghostModeClickTime;
	float clickTimer;
	void Start()
	{
		//AssetDatabase.CreateAsset (new Sign(), "Assets/Sign.asset");
		clickTimer = 0;
	}

	bool ghostModeToggled = false;
	void ToggleGhostMode()
	{
		ghostModeToggled = true;
		GetComponent<PlayerTrain>().ToggleGhostMode(!GetComponent<PlayerTrain>().ghostMode);
	}

	void Update () 
	{
		if (GameController.IsPaused ())
			return;
		if (Input.GetMouseButtonDown(0))
		{
			clickTimer = 0;


		}
		if (Input.GetMouseButton(0))
		{
			RaycastHit2D hit = Physics2D.GetRayIntersection(trainCamera.ScreenPointToRay(Input.mousePosition));
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("player"))
				{
					GameObject wagon = hit.collider.gameObject;
					if (wagon.GetComponent<WagonScript>().isHead)
					{
						clickTimer += Time.deltaTime;
						if (clickTimer >= ghostModeClickTime)
						{
							clickTimer = 0;
							if (!ghostModeToggled)
								ToggleGhostMode();
						}

					}
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
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
                        {
                            if (wagon.GetComponent<WagonScript>().CanCastSign() && GetComponent<SignsController>().CanCast())
                            {
                                wagon.GetComponent<Animator>().Play("wagonAnim");
                                wagon.GetComponent<WagonScript>().CastSign();
                            }
                        }
                    }
                    else
                    {
                        if (!ghostModeToggled)
                        {
							InventorySystem.reference.ToggleUI();
                        }
                        else
                        {
                            ghostModeToggled = false;
                        }
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("objectToStop"))
                {
                    GetComponent<PlayerTrain>().StopNearNextObject();
                }
			}


			
		}
	}
}
