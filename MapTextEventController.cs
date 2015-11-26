using UnityEngine;
using System.Collections;
public class MapTextEventController : MonoBehaviour {

	public TextQuest quest;
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (!coll.gameObject.GetComponent<WagonScript>().IsHead())
				return;
			Debug.Log(DialogueController.reference);
			DialogueController.reference.StartQuest(quest);
		}
	}
}
