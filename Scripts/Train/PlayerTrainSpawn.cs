using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RoadScript))]
public class PlayerTrainSpawn : MonoBehaviour {

	public Transform train;
	public Transform spawnPoint;

	public void SetTrainTo(Transform point)
	{
		for(int childIndex = 0; childIndex < train.childCount; childIndex++)
		{
			if (train.GetChild(childIndex).GetComponent<WagonScript>())
				train.GetChild(childIndex).GetComponent<WagonScript>().SetDestination(point);
		}

		train.eulerAngles = point.eulerAngles - train.GetChild(0).eulerAngles;

		List<Transform> list = new List<Transform> ();
		for(int childIndex = train.childCount-1; childIndex >= 0; childIndex--)
		{
			list.Add (train.GetChild(childIndex));
			train.GetChild(childIndex).parent = null;

		}
		train.eulerAngles = new Vector3 (0,0,0);
		for(int childIndex = list.Count-1; childIndex >= 0; childIndex--)
		{
			list[childIndex].parent = train;

		}

		Vector3 trainPos = point.position;
		//trainPos.x += 0.5f*Mathf.Sin (train.eulerAngles.z*Mathf.PI/180);
		//trainPos.y -= 0.5f*Mathf.Cos (train.eulerAngles.z*Mathf.PI/180);
		train.position = trainPos;

	}

	void Start()
	{
		SetTrainTo (spawnPoint);
	}

}
