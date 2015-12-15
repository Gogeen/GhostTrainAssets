using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Item : MonoBehaviour {

	public int wagonIndex;
	public int slotIndex;

	public InventoryItem reference = new InventoryItem();
	/*
	public Vector2 size;
	public string spriteName;
	public string itemName;
	public string description;
	public InventoryItem.Type type;
	*/
	void OnEnable()
	{
		GetComponent<UISprite>().spriteName = reference.uiInfo.spriteName;
	}

	void Update()
	{
		transform.localScale = reference.uiInfo.size;
	}
}
