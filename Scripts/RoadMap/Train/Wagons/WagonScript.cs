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
	public Transform nextUnreachedPoint;
	Transform lastReachedPoint;
	public float cameraOffset;

	public GameObject signObject;

	public Transform ghostPoints;

	public Transform GetGhostPoint()
	{
		if (ghostPoints == null)
			return null;
		int freeGhostPointsCount = 0;
		for (int pointIndex = 0; pointIndex < ghostPoints.childCount; pointIndex++) {
			Transform point = ghostPoints.GetChild (pointIndex);
			if (point.childCount > 0)
				continue;
			freeGhostPointsCount += 1;
		}
		if (freeGhostPointsCount == 0)
			return null;
		int ghostPointIndex = Random.Range (0, freeGhostPointsCount);
		for (int pointIndex = 0; pointIndex < ghostPoints.childCount; pointIndex++) {
			Transform point = ghostPoints.GetChild (pointIndex);
			if (point.childCount > 0)
				continue;
			if (ghostPointIndex == 0)
				return point;
			ghostPointIndex -= 1;
		}
		return null;
	}

    public bool IsHead()
    {
        return isHead;
    }

	public int GetIndex()
	{
		return transform.GetSiblingIndex ();
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
	void Move(float speed)
	{
		
		//if (nextUnreachedPoint != null)
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
		nextUnreachedPoint = point;
		lastReachedPoint = null;
	}

	List<Transform> railPointsEntered = new List<Transform>();

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("road"))
		{
			//if (isHead)
			//	Debug.Log ("OnTriggerEnter2D hit road");
			railPointsEntered.Add (coll.transform);
			while (nextUnreachedPoint == null || railPointsEntered.Contains (nextUnreachedPoint)) {
				if (nextUnreachedPoint == null)
					nextUnreachedPoint = road.GetNextPoint (coll.transform);
				else
					nextUnreachedPoint = road.GetNextPoint (nextUnreachedPoint);
			}

		}

		if (gameObject.layer == LayerMask.NameToLayer ("enemy"))
			return;
		if (coll.gameObject.layer == LayerMask.NameToLayer ("enemy")) {
			if (!isDead){
				isDead = true;
			}
		}
	}

	bool isDead = false;
	void Die()
	{
		GameController.reference.GameOver ();
	}


	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer("road")){
			railPointsEntered.Remove (coll.transform);
			lastReachedPoint = coll.transform;
		}
	}

	bool IsAI()
	{
		TrainController controller = transform.parent.GetComponent<TrainController>();
		return (controller is EnemyTrain);
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
			Destroy (signObject);
		}
		currentSign = signType;
		if (currentSign == SignsController.SignType.None)
			return;

		SignsController controller = transform.parent.GetComponent<SignsController>();
		GameObject signPrefab = null;
		if (currentSign == SignsController.SignType.Triangle)
		{
			signPrefab = ((Sign)controller.triangleSign).prefab;
			signObject = Instantiate (signPrefab) as GameObject;
		}
		else if (currentSign == SignsController.SignType.Rectangle)
		{
			signPrefab = ((Sign)controller.rectangleSign).prefab;
			signObject = Instantiate (signPrefab) as GameObject;
		}
		else if (currentSign == SignsController.SignType.Barrier)
		{
			signPrefab = ((Sign)controller.barrierSign).prefab;
			signObject = Instantiate (signPrefab) as GameObject;
		}
		signObject.transform.parent = sign;
		signObject.transform.localPosition = new Vector3(0,0,0);
		signObject.transform.localEulerAngles = new Vector3(0,0,0);
		signObject.transform.localScale = signPrefab.transform.localScale;
	}

	public void CastSign()
	{
		SignsController controller = transform.parent.GetComponent<SignsController>();
		controller.CastSign(currentSign);
	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = (nextUnreachedPoint.position - transform.position).normalized*GetTrainSpeed();
		//Debug.Log (GetComponent<Rigidbody2D>().velocity);

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

		if (PlayerSaveData.reference.trainData.equippedItems [0] == null || PlayerSaveData.reference.trainData.equippedItems [0].IsBroken ())
			isDead = true;
		if (isDead) {
			Die ();
		}
	}
}
