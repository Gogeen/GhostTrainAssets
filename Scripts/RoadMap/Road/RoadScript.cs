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


	public GameObject roadPrefab;
	public GameObject railPrefab;
	Transform leftRailPoint = null;
	Transform rightRailPoint = null;

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

				GameObject rail = null;
				if (leftRailPoint != null)
				{
					rail = Instantiate(railPrefab) as GameObject;
					Transform currentLeftRailPoint = roadPart.transform.GetChild(2).GetChild(0);
					rail.transform.position = (leftRailPoint.position + currentLeftRailPoint.position)/2;
					rail.transform.localScale = new Vector3(rail.transform.localScale.x, (leftRailPoint.position - currentLeftRailPoint.position).magnitude*5, rail.transform.localScale.z);
					rail.transform.localEulerAngles = new Vector3(rail.transform.localEulerAngles.x, rail.transform.localEulerAngles.y, GetVectorRotation(currentLeftRailPoint.position - leftRailPoint.position));
					rail.transform.parent = roadPart.transform;
					//rail.transform.GetChild(0).localScale = new Vector3((leftRailPoint.position - currentLeftRailPoint.position).magnitude*5, railPrefab.transform.GetChild(0).localScale.y, railPrefab.transform.GetChild(0).localScale.z);
					
				}
				if (rightRailPoint != null)
				{
					rail = Instantiate(railPrefab) as GameObject;
					Transform currentRightRailPoint = roadPart.transform.GetChild(2).GetChild(1);
					rail.transform.position = (rightRailPoint.position + currentRightRailPoint.position)/2;
					rail.transform.localScale = new Vector3(rail.transform.localScale.x, (rightRailPoint.position - currentRightRailPoint.position).magnitude*5, rail.transform.localScale.z);
					rail.transform.localEulerAngles = new Vector3(rail.transform.localEulerAngles.x, rail.transform.localEulerAngles.y, GetVectorRotation(currentRightRailPoint.position - rightRailPoint.position));
					rail.transform.parent = roadPart.transform;
				}
				leftRailPoint = roadPart.transform.GetChild(2).GetChild(0);
				rightRailPoint = roadPart.transform.GetChild(2).GetChild(1);

			}

		}
	}

	public bool generateNow;

	void Update()
	{
		if (generateNow)
		{
			GenerateRoad ();
			generateNow = false;
		}
	}
}
