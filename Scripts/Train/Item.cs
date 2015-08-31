using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Item : MonoBehaviour {

	public bool developerAccess;
	public int slotIndex;
	public Vector2 size;
	public string spriteName;
	public string description;

	void Start()
	{
		GetComponent<UISprite>().spriteName = spriteName;
	}

	void Update()
	{
		if (developerAccess)
		{
			GetComponent<UISprite>().spriteName = spriteName;
		}
		transform.localScale = size;
	}
}
