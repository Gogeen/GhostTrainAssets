using UnityEngine;
using System.Collections;

public class InventoryUI : MonoBehaviour {

	public GameObject DescriptionWindow;
	public void ShowItemDescription(string description)
	{
		DescriptionWindow.SetActive (true);
		UILabel descriptionLabel = DescriptionWindow.transform.GetChild (0).GetComponent<UILabel> ();
		descriptionLabel.text = description;
	}

	public void HideItemDescription()
	{
		DescriptionWindow.SetActive (false);
	}
}
