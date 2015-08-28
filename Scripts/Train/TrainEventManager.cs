using UnityEngine;
using System.Collections;

public class TrainEventManager : MonoBehaviour {
	
	public Camera trainCamera;
	public GameObject inventoryUI;
	public GameObject gameUI;
	void PauseGame()
	{
		Time.timeScale = 0;
	}

	bool IsGamePaused()
	{
		return Time.timeScale == 0;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
	}

	void OpenInventory()
	{
		inventoryUI.SetActive (true);
		gameUI.SetActive (false);
	}

	public void CloseInventory()
	{
		inventoryUI.SetActive (false);
		gameUI.SetActive (true);
	}

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
						if (!IsGamePaused())
							wagon.GetComponent<Animator>().Play("wagonAnim");
					}
					else
					{
						PauseGame();
						OpenInventory();
					}
				}
			}
		}
	}
}
