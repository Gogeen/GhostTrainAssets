using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadScript : MonoBehaviour {

	public bool isCircle;
	public List<Transform> points = new List<Transform>();
	void OnDrawGizmos()
	{
		for (int pointID = 0; pointID < points.Count-1; pointID++) {
			Gizmos.DrawLine(points[pointID].position, points[pointID+1].position);
		}
		if (isCircle)
		{
			Gizmos.DrawLine(points[points.Count-1].position, points[0].position);
		}
	}

	public Transform GetNextPoint(Transform point)
	{
		int nextPointIndex = points.IndexOf (point) + 1;
		if (nextPointIndex >= points.Count)
			nextPointIndex = 0;
		return points [nextPointIndex];
	}

	public Transform GetPreviousPoint(Transform point)
	{
		int nextPointIndex = points.IndexOf (point) - 1;
		if (nextPointIndex < 0)
			nextPointIndex = points.Count-1;
		return points [nextPointIndex];
	}
}
