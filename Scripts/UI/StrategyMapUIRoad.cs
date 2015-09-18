using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class StrategyMapUIRoad : MonoBehaviour {

	public bool developerAccess;
	public bool repositionNow;
	public Transform firstTown;
	public Transform secondTown;
	public float travelTime;
	public GameObject anomalyPrefab;
	public GameObject pilePrefab;
	public GameObject waystationPrefab;

	public float objectLabelRange;

			
	public List<TravelObject> objects = new List<TravelObject>();
	public enum TravelObject
	{
		Anomaly,
		Pile,
		Waystation
	}
	float GetAngleBetween(Vector2 first, Vector2 second)
	{
		float angle = Vector2.Angle (first, second);
		Vector3 cross = Vector3.Cross (first, second);
		if (cross.z > 0)
			angle = -angle;
		return angle;
	}
	
	float GetVectorRotation(Vector2 direction)
	{
		return -GetAngleBetween (Vector2.up, direction);
	}

	void GenerateObject(TravelObject objectType, float position)
	{
		GameObject objectPrefab = null;
		switch(objectType)
		{
		case TravelObject.Anomaly: {objectPrefab = anomalyPrefab; break;}
		case TravelObject.Pile: {objectPrefab = pilePrefab; break;}
		case TravelObject.Waystation: {objectPrefab = waystationPrefab; break;}
		}
		GameObject generatedObject = Instantiate (objectPrefab) as GameObject;
		generatedObject.transform.parent = transform;
		generatedObject.transform.localPosition = new Vector3(position,0,0);
		generatedObject.transform.localScale = new Vector3 (1,1,1);

		Vector3 roadRotation = transform.localEulerAngles;

		Transform objectTimeLabel = generatedObject.transform.GetChild (0);
		objectTimeLabel.localPosition = new Vector3 (Mathf.Sin (roadRotation.z*Mathf.PI/180)*objectLabelRange,-Mathf.Cos (roadRotation.z*Mathf.PI/180)*objectLabelRange,0);
		objectTimeLabel.gameObject.SetActive (false);
	}

	void PlaceRoad()
	{
		transform.position = firstTown.position;
		Vector3 roadRotation = transform.localEulerAngles;
		roadRotation.z = GetVectorRotation(secondTown.position - firstTown.position) + 90;
		transform.localEulerAngles = roadRotation;
		float roadLength = (secondTown.localPosition - firstTown.localPosition).magnitude;
		GetComponent<UISprite>().width = (int)roadLength;

		for (int childIndex = transform.childCount-1; childIndex >= 0; childIndex--)
		{
			DestroyImmediate(transform.GetChild(childIndex).gameObject);
		}
		int objectIndex = 0;
		foreach(TravelObject travelObject in objects)
		{
			GenerateObject(travelObject, GetComponent<UISprite>().width * (objectIndex + 1)/(objects.Count + 1));
			objectIndex += 1;
		}
	}

	void Update()
	{
		if (firstTown == null || secondTown == null)
			return;
		if (repositionNow)
		{
			repositionNow = false;
			PlaceRoad();
		}
		if (developerAccess)
		{
			PlaceRoad();
		}
	}
}
