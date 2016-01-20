using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class ClockTime : MonoBehaviour {

	public GameObject arrow;
	public GameObject timeWheel;
	public float currentTimeInHours;



	public void ShowTime(float hours)
	{
		if (hours < 0)
			return;
		if (hours > 12)
			hours %= 12;

		Vector3 arrowRotation = arrow.transform.localEulerAngles;
		arrowRotation.z = 90 - (hours/12) * 360;
		arrow.transform.localEulerAngles = arrowRotation;

		Vector3 wheelRotation = timeWheel.transform.localEulerAngles;
		wheelRotation.z = 0;
		timeWheel.transform.localEulerAngles = wheelRotation;

		timeWheel.GetComponent<UISprite> ().fillAmount = 0;
	}

	public void ShowActionTime(TownAction action)
	{
		ShowTime (currentTimeInHours);
		int minutes = 0;
		if (action is TownRepairAction)
			minutes = ((TownRepairAction)action).GetTime ();
		else
			minutes = action.GetTime ();
		Vector3 wheelRotation = timeWheel.transform.localEulerAngles;
		wheelRotation.z = arrow.transform.localEulerAngles.z - 90;
		timeWheel.transform.localEulerAngles = wheelRotation;

		Vector3 arrowRotation = arrow.transform.localEulerAngles;
		arrowRotation.z -= (float)minutes / 4;
		arrow.transform.localEulerAngles = arrowRotation;


		timeWheel.GetComponent<UISprite> ().fillAmount = ((float)minutes / 4)/360;
	}

	void Start()
	{
		ShowTime (currentTimeInHours);
	}
}
