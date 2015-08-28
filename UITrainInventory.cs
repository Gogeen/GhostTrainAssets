using UnityEngine;
using System.Collections;

public class UITrainInventory : MonoBehaviour {

	public GameObject DescriptionWindow;
	public void ShowItemDescriptionWindow(string description)
	{
		DescriptionWindow.SetActive (true);
		DescriptionWindow.transform.GetChild (0).GetComponent<UILabel> ().text = description;
	}

	public void HideItemDescriptionWindow()
	{
		DescriptionWindow.SetActive (false);
	}
}
