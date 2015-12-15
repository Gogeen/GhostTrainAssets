using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class StrategyMapTownInfo : MonoBehaviour {

	public string name;
	public UILabel nameLabel;

	void Update()
	{
		if (nameLabel != null)
			nameLabel.text = name;
	}

}
