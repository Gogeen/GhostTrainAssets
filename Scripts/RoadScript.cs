using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class RoadScript : MonoBehaviour {

	public Transform GetNextPoint(Transform point)
	{
		int nextPointIndex = roadPoints.IndexOf (point) + 1;
		if (nextPointIndex >= roadPoints.Count)
			nextPointIndex = roadPoints.Count - 1;
		return roadPoints [nextPointIndex];
	}

	public Transform GetPreviousPoint(Transform point)
	{
		int nextPointIndex = roadPoints.IndexOf (point) - 1;
		if (nextPointIndex < 0)
			nextPointIndex = 0;
		return roadPoints [nextPointIndex];
	}
	
	public GameObject roadPrefab;
	public Transform roadTransform;
	public enum RoadType
	{
		Straight,
		Bend
	}

	[System.Serializable]
	public class RoadPart
	{
		public RoadType type;
		public float bendDegree;
		public int miniPartsCount;
	}
	public List<RoadPart> road = new List<RoadPart>();
	public List<Transform> roadPoints = new List<Transform>();
	void GenerateRoad()
	{
		roadPoints.Clear ();
		for (int roadPartIndex = roadTransform.childCount-1; roadPartIndex >= 0; roadPartIndex--)
			DestroyImmediate (roadTransform.GetChild(roadPartIndex).gameObject);
		Vector3 generatePosition = roadTransform.position;
		Vector3 generateRotation = roadTransform.localEulerAngles;

		for(int roadPartIndex = 0; roadPartIndex < road.Count; roadPartIndex++)
		{
			for (int roadMiniPartIndex = 0; roadMiniPartIndex < road[roadPartIndex].miniPartsCount; roadMiniPartIndex++)
			{
				GameObject roadPart = Instantiate(roadPrefab) as GameObject;
				Vector3 roadPartLastPointPosition = roadPart.transform.GetChild(1).localPosition;
				roadPartLastPointPosition.x = Mathf.Tan(road[roadPartIndex].bendDegree*2*Mathf.PI/360)*0.1f;
				roadPart.transform.GetChild(1).localPosition = roadPartLastPointPosition;
				roadPart.transform.parent = roadTransform;
				roadPart.transform.position = generatePosition;
				roadPart.transform.localEulerAngles = generateRotation;

				roadPoints.Add(roadPart.transform.GetChild(1));

				generatePosition = roadPart.transform.GetChild(1).position;
				generateRotation = roadPart.transform.localEulerAngles;

				Vector3 partSpriteRotation = roadPart.transform.GetChild(2).localEulerAngles;
				partSpriteRotation.z = -road[roadPartIndex].bendDegree;
				roadPart.transform.GetChild(2).localEulerAngles = partSpriteRotation;
				switch (road[roadPartIndex].type)
				{
				case RoadType.Straight: {break;}
				case RoadType.Bend: {generateRotation.z -= road[roadPartIndex].bendDegree; break;}
				}
			}

		}
	}
	public Transform train;
	void SetTrainTo(Transform point)
	{
		Vector3 trainPos = point.position;
		trainPos.y -= 0.5f;
		train.position = trainPos;
		for(int childIndex = 0; childIndex < train.childCount; childIndex++)
		{
			if (train.GetChild(childIndex).GetComponent<WagonScript>())
				train.GetChild(childIndex).GetComponent<WagonScript>().nextPoint = GetNextPoint(point);
		}
	}
	public bool generateNow;
	void Start()
	{

		SetTrainTo (roadPoints [1]);

	}

	void Update()
	{
		if (generateNow)
		{
			GenerateRoad ();
			SetTrainTo (roadPoints [1]);
			generateNow = false;
		}
	}
}
