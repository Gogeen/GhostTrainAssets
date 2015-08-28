using UnityEngine;
using System.Collections;

public class WagonScript : MonoBehaviour {

	RoadScript road;
	public bool isHead;
	public Transform camera;
	public Transform nextPoint;
	public Transform frontWheel;
	public Transform backWheel;


	void Start()
	{
		road = transform.parent.GetComponent<RoadScript> ();
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

	float GetRotationPercent()
	{
		float percent = (frontWheel.position - road.GetPreviousPoint (nextPoint).position).magnitude / (frontWheel.position - backWheel.position).magnitude;
		if (percent>1)
			return 1;
		if (percent<0)
			return 0;
		return percent;
	}

	public void RotateFor(Vector2 lastDir, Vector2 newDir)
	{
		float rotateAngle = GetAngleBetween (lastDir, newDir);
		float rotationPercent = GetRotationPercent();
		float lastRotation = GetVectorRotation (lastDir);

		Vector3 localEulers = transform.localEulerAngles;
		localEulers.z = lastRotation - rotateAngle*rotationPercent;
		transform.localEulerAngles = localEulers;
	}
	public void Move(float speed)
	{
		GetComponent<Rigidbody2D>().velocity = (nextPoint.position-road.GetPreviousPoint (nextPoint).position).normalized*speed*Time.deltaTime;
		Transform previousPoint = road.GetPreviousPoint (nextPoint);
		Vector2 lastDirection = previousPoint.position - road.GetPreviousPoint (previousPoint).position;
		Vector2 newDirection = nextPoint.position - previousPoint.position;
		RotateFor(lastDirection,newDirection);
		if (isHead)
		{
			Vector3 camPos = transform.position;
			camPos.z = -10;
			camera.position = camPos;
		}

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (TrainScript.currentSpeed < 0)
			return;

		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			if (coll.transform == nextPoint)
			{
				transform.position = nextPoint.position;
				nextPoint = road.GetNextPoint (nextPoint);
			} 
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (TrainScript.currentSpeed > 0)
			return;

		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			Transform previousPoint = road.GetPreviousPoint (nextPoint);
			if (coll.transform == previousPoint)
			{
				transform.position = previousPoint.position;
				nextPoint = previousPoint;
			}
		}
	}

	void OnDrawGizmos()
	{
		if (!isHead)
			return;
		Transform previousPoint = transform.parent.GetComponent<RoadScript> ().GetPreviousPoint (nextPoint);
		Gizmos.color = new Color (0,0,0);
		Gizmos.DrawLine (transform.parent.GetComponent<RoadScript> ().GetPreviousPoint (previousPoint).position, previousPoint.position);
	}

	void Update()
	{
		Move(TrainScript.currentSpeed);
	}
}
