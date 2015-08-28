using UnityEngine;
using System.Collections;

public class TrainTimeScript : MonoBehaviour {

	public static int hours;
	public static int minutes;
	public static float seconds;

	public int startHours;
	public int startMinutes;
	public int startSeconds;

	void Start()
	{
		hours = startHours;
		minutes = startMinutes;
		seconds = startSeconds;

	}

	void Update()
	{
		if (TrainScript.currentSpeed == 0)
			return;
		if (seconds < 0 && minutes == 0 && hours == 0)
		{
			Debug.Log ("Time out");
			return;
		}
		if (seconds < 0)
		{
			seconds += 60;
			minutes -= 1;
			if (minutes < 0)
			{
				minutes += 60;
				hours -= 1;
			}
		}
		seconds -= Time.deltaTime;

	}
}
