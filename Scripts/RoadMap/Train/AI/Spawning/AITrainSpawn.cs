using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AITrainSpawn : MonoBehaviour {

	public GameObject enemyPrefab;
	public float startSpawnDelay;
	public float spawnTime;
	public Transform spawnPoint;

	public void SetTrainTo(Transform train, Transform point)
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
			if (list[childIndex].GetComponent<WagonScript>())
			{
				list[childIndex].GetComponent<WagonScript>().road = GetComponent<RoadScript>();
			}
			
		}
		
		Vector3 trainPos = point.position;
		//trainPos.x += 0.5f*Mathf.Sin (train.eulerAngles.z*Mathf.PI/180);
		//trainPos.y -= 0.5f*Mathf.Cos (train.eulerAngles.z*Mathf.PI/180);
		train.position = trainPos;
		
	}

	void Spawn()
	{
		GameObject enemy = Instantiate (enemyPrefab) as GameObject;
		SetTrainTo (enemy.transform, spawnPoint);
	}

	IEnumerator Spawner()
	{
		yield return new WaitForSeconds (startSpawnDelay);
		Spawn ();
		while (true) 
		{
			yield return new WaitForSeconds (spawnTime);
			Spawn ();
		}
	}

	void Start () {
		StartCoroutine ("Spawner");
	}
}
