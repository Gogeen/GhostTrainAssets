using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WagonScript : MonoBehaviour {

	public RoadScript road;
	public bool isHead;
	public Transform trainCamera;
	public Transform sign;
	public SignsController.SignType signType;
	SignsController.SignType currentSign = SignsController.SignType.None;
	public Transform nextPoint;
	Transform nextUnreachedPoint;
	public float cameraOffset;

    public bool IsHead()
    {
        return isHead;
    }
    public bool IsLast()
    {
        if (GetComponent<WagonConnector>())
        {
            if (GetComponent<WagonConnector>().nextWagon != null)
            {
                return false;
            }
            return true;
        }
        return false;
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
		float currentRadianRotation = transform.localEulerAngles.z*Mathf.PI/180;
		Vector3 frontWheelPosition = transform.position;
		frontWheelPosition += new Vector3(-Mathf.Sin (currentRadianRotation)*GetComponent<BoxCollider2D> ().size.y,Mathf.Cos (currentRadianRotation)*GetComponent<BoxCollider2D> ().size.y,0);
		Vector3 frontWheelPotentialPosition = lastReachedPointPosition + (nextUnreachedPoint.position - lastReachedPointPosition).normalized*(lastReachedPointPosition - frontWheelPosition).magnitude;

		Vector3 localEulers = transform.localEulerAngles;
		localEulers.z = GetVectorRotation(frontWheelPotentialPosition - transform.position);
		transform.localEulerAngles = localEulers;
		if (sign != null)
			sign.eulerAngles = new Vector3 (0,0,0);

	}
	public void Move(float speed)
	{
		if ((nextPoint.position - transform.position).magnitude <= 0.01)
			nextPoint = road.GetNextPoint (nextPoint);
		GetComponent<Rigidbody2D>().velocity = (nextPoint.position - transform.position).normalized*speed;
		if (nextUnreachedPoint != null)
			Rotate();
		if (isHead && !IsAI ())
		{
			float rotation = transform.localEulerAngles.z*Mathf.PI/180;
			Vector3 camPos = transform.position + new Vector3(-cameraOffset*Mathf.Sin (rotation),cameraOffset*Mathf.Cos (rotation),0);
			camPos.z = -10;
			trainCamera.position = camPos;
		}
	}

	public void SetDestination(Transform point)
	{
		setNextPoint = true;;
		nextUnreachedPoint = null;
		nextPoint = point;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			nextUnreachedPoint = road.GetNextPoint(coll.transform); 
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (gameObject.layer == LayerMask.NameToLayer ("enemy"))
			return;
		if (IsGhostMode ())
			return;
		if (coll.gameObject.layer == LayerMask.NameToLayer("enemy"))
		{
			//!!!
			GameController.Quit();
		}
	}


	bool setNextPoint = false;
	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			//nextPoint = road.GetNextPoint (coll.transform);
			if (!setNextPoint)
			{
				nextPoint = road.GetNextPoint (nextPoint);

			}
			else
			{
				if (coll.transform == nextPoint)
				{
					setNextPoint = false;
					nextPoint = road.GetNextPoint (coll.transform);
				}
			}

		}
	}

	bool IsAI()
	{
		TrainController controller = transform.parent.GetComponent<TrainController>();
		return (controller is EnemyTrain);
	}

	bool IsGhostMode()
	{
		TrainController controller = transform.parent.GetComponent<TrainController>();
		return (((PlayerTrain)controller).ghostMode);
	}

	float GetTrainSpeed()
	{
		TrainController controller = transform.parent.GetComponent<TrainController>();
		return controller.speed;
	}

	void CheckSignAvailability()
	{
		if (gameObject.layer == LayerMask.NameToLayer ("enemy"))
			return;
		if (sign == null)
			return;
		if (currentSign == signType)
			return;
		if (currentSign != SignsController.SignType.None) 
		{
			GameObject signObject = sign.GetChild (0).gameObject;
			Destroy (signObject);
		}
		currentSign = signType;
		if (currentSign == SignsController.SignType.None)
			return;
		if (currentSign == SignsController.SignType.Triangle)
		{
			SignsController controller = transform.parent.GetComponent<SignsController>();
			GameObject signPrefab = ((Sign)controller.triangleSign).prefab;
			GameObject signObject = Instantiate (signPrefab) as GameObject;
			signObject.transform.parent = sign;
			signObject.transform.localPosition = new Vector3(0,0,0);
			signObject.transform.localScale = signPrefab.transform.localScale;
		}
		else if (currentSign == SignsController.SignType.Rectangle)
		{
			SignsController controller = transform.parent.GetComponent<SignsController>();
			GameObject signPrefab = ((Sign)controller.rectangleSign).prefab;
			GameObject signObject = Instantiate (signPrefab) as GameObject;
			signObject.transform.parent = sign;
			signObject.transform.localPosition = new Vector3(0,0,0);
			signObject.transform.localScale = signPrefab.transform.localScale;
		}
	}

	public bool CanCastSign()
	{
		return currentSign != SignsController.SignType.None;
	}

	public void CastSign()
	{
		SignsController controller = transform.parent.GetComponent<SignsController>();
		controller.CastSign(currentSign);
	}

	void Update()
	{
		Move(GetTrainSpeed());

		CheckSignAvailability ();

		if (isHead){
			Vector3 offset = transform.parent.position - transform.position;
			transform.parent.position = transform.position;
			for(int childIndex = 0; childIndex < transform.parent.childCount; childIndex++)
			{
				transform.parent.GetChild(childIndex).localPosition += offset;
			}
		}

	}
}
