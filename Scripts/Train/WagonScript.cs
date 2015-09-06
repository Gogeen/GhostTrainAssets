using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WagonScript : MonoBehaviour {

	RoadScript road;
	public bool isHead;
	public Transform trainCamera;
	public Transform nextPoint;
	Transform nextUnreachedPoint;
	public Transform frontWheel;

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
	
	public void Rotate()
	{
		Vector3 lastReachedPointPosition = road.GetPreviousPoint (nextUnreachedPoint).position;
		Vector3 frontWheelPotentialPosition = lastReachedPointPosition + (nextUnreachedPoint.position - lastReachedPointPosition).normalized*(lastReachedPointPosition - frontWheel.position).magnitude;

		Vector3 localEulers = transform.localEulerAngles;
		localEulers.z = GetVectorRotation(frontWheelPotentialPosition - transform.position);
		transform.localEulerAngles = localEulers;

	}
	public void Move(float speed)
	{
		GetComponent<Rigidbody2D>().velocity = (nextPoint.position - transform.position).normalized*speed;
		if (nextUnreachedPoint != null)
			Rotate();
		if (isHead)
		{
			Vector3 camPos = transform.position;
			camPos.z = -10;
			trainCamera.position = camPos;
		}

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (TrainController.currentSpeed <= 0)
			return;

		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			nextUnreachedPoint = road.GetNextPoint(coll.transform); 
		}
	}




	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			nextPoint = road.GetNextPoint (nextPoint);
		}
	}
	
	void Update()
	{
		Move(TrainController.currentSpeed);
	}
}
