using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerTrain))]
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

	bool IsTimeOut()
	{
		return (seconds <= 0 && minutes == 0 && hours == 0);
	}

	void Update()
	{
		UpdateUI ();
		if (GetComponent<PlayerTrain>().speed == 0)
			return;
		if (IsTimeOut())
		{
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
		if (GetComponent<PlayerTrain> ().ghostMode) 
		{
			seconds -= Time.deltaTime;
		}
		seconds -= Time.deltaTime;
	}

	public UILabel timeLabel;
	void UpdateUI()
	{
		string time = "";
		if (hours < 10)
			time += "0";
		time += hours + ":";
		if (minutes < 10)
			time += "0";
		time += minutes + ":";
		if (seconds < 10)
			time += "0";
		time += (int)seconds;
		timeLabel.text = time;
	}
}
