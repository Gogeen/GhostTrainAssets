﻿using UnityEngine;
using System.Collections;
public class MapTextEventController : MonoBehaviour {

	public TextQuest quest;
	public GameObject target;
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
		{
			if (!coll.gameObject.GetComponent<WagonScript>().IsHead())
				return;
			Debug.Log(DialogueController.reference);
			DialogueSystem.reference.StartQuest(quest);
		}
	}

	void Start()
	{
		if (target != null && quest != null) {
			quest.target = target;
		}
	}
}