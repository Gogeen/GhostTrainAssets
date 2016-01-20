using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class AssetCreator : MonoBehaviour {

	public bool Create = false;
	public string assetDataName;

	void CreateAsset()
	{
		if (assetDataName != "") {
			ScriptableObject asset = ScriptableObject.CreateInstance (assetDataName);
			Debug.Log (assetDataName);
//			UnityEditor.AssetDatabase.CreateAsset (asset, "Assets/AssetCreator/newAsset.asset");
		}
	}

	void Update()
	{
		if (Create) {
			Create = false;
			CreateAsset ();
		}
	}
}
