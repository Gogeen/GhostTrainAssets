using UnityEngine;
using System.Collections;
public class TrainTimeScript : MonoBehaviour {

	public static TrainTimeScript reference = null;



	bool IsTimeOut()
	{
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		return (time.minutes <= 0 && time.hours == 0);
	}

	public float waitingTimeSpeedMultiplier;
	public int passengerTickTime;
	public int passengerTickMinPercent;
	public int passengerTickMaxPercent;
	public int passengerWaystationOutMinPercent;
	public int passengerWaystationOutMaxPercent;
	public int passengerTownOutMinPercent;
	public int passengerTownOutMaxPercent;
	public int basePassengerTimeIncome;
	public float passengerTravelMultiplier;

	float passengersCame = 0;
	int waitingTime = 0;
	float passengersCameOut = 0;
	IEnumerator WaitForPassengers()
	{
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		if (passengersData.IsFull())
		{
			ToggleWaitForPassengers();
			return true;
		}
		float gameSeconds = 0;
		while(true)
		{
			gameSeconds -= Time.deltaTime * waitingTimeSpeedMultiplier;
			while (gameSeconds < 0)
			{
				gameSeconds += 1;
				AddTime (-1);
				waitingTime += 1;
			}
			if (waitingTime >= passengerTickTime)
			{
				waitingTime -= passengerTickTime;
				float passengersComing = ((float)passengersData.GetMaxPassengers() * Random.Range (passengerTickMinPercent, passengerTickMaxPercent)) / 100;
				passengersCame += passengersComing;
				Debug.Log("passengers +"+(int)passengersCame);
				while(passengersCame >= 1)
				{
					passengersData.AddPassenger();
					passengersCame -= 1;
					if (passengersData.IsFull())
					{
						ToggleWaitForPassengers();
						break;
					}
				}
				if (passengersData.IsFull())
				{
					ToggleWaitForPassengers();
					break;
				}

			}
			yield return null;
		}
	}

	void UpdatePassengersTravelTime()
	{
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		foreach(PlayerSaveData.Passenger passenger in passengersData.passengers)
		{
			passenger.stationsTravelled += 1;
		}
	}

	void RemovePassengersComingOut(float count)
	{
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		passengersCameOut += count;
		while(passengersCameOut >= 1)
		{
			if (passengersData.passengers.Count <= 0)
			{
				break;
			}
			PlayerSaveData.Passenger passengerComingOut = passengersData.GetPassenger(Random.Range(0, passengersData.passengers.Count-1));
			float attractionMod = (float)PlayerSaveData.reference.wagonData[passengerComingOut.wagonIndex].attraction/100;
			if (attractionMod < 0)
			{
				attractionMod = 1/(Mathf.Abs (attractionMod) + 1);
			}
			else
			{
				attractionMod = attractionMod + 1;
			}
			AddTime((int)(basePassengerTimeIncome*attractionMod*Mathf.Pow(passengerTravelMultiplier,passengerComingOut.stationsTravelled)));
			passengersData.RemovePassenger(passengerComingOut);
			passengersCameOut -= 1;
		}
	}

	public void ComeInTown()
	{
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		UpdatePassengersTravelTime ();
		RemovePassengersComingOut (((float)passengersData.GetMaxPassengers() * Random.Range (passengerTownOutMinPercent, passengerTownOutMaxPercent)) / 100);
	}

	public void ComeInWaystation()
	{
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		UpdatePassengersTravelTime ();
		RemovePassengersComingOut (((float)passengersData.GetMaxPassengers() * Random.Range (passengerWaystationOutMinPercent, passengerWaystationOutMaxPercent)) / 100);
	}

	public void AddTime(int minutes)
	{
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		time.minutes += minutes;
		if (IsTimeOut())
		{
			time.minutes -= minutes;
			Debug.Log ("Not Enough Time!");
			return;
		}
		if (time.minutes < 0)
		{
			time.minutes += 60;
			time.hours -= 1;
		}
		else if (time.minutes >= 60)
		{
			time.minutes -= 60;
			time.hours += 1;
		}
	}

	bool waitForPassengers = false;
	public void  ToggleWaitForPassengers()
	{
		waitForPassengers = !waitForPassengers;
		if (waitForPassengers)
		{
			StartCoroutine("WaitForPassengers");
		}
		else
		{
			StopCoroutine("WaitForPassengers");
		}
	}

	IEnumerator GameTimer()
	{
		float gameSeconds = 0;
		while (true) 
		{
			if (IsTimeOut())
			{
				break;
			}
			if (PlayerTrain.reference == null)
			{
				yield return null;
				continue;
			}
			else
			{
				if (PlayerTrain.reference.ghostMode) 
				{
					gameSeconds -= Time.deltaTime;

				}
			}
			gameSeconds -= Time.deltaTime;
			while (gameSeconds < 0)
			{
				gameSeconds += 1;
				AddTime (-1);
			}
			yield return null;
		}
	}

	void Start()
	{
		StartCoroutine ("GameTimer");
	}

	void Update()
	{
		UpdateUI ();

		/*if (Input.GetKeyDown("h"))
		{
			ComeInTown();
		}*/
	}

	public UILabel timeLabel;
	void UpdateUI()
	{
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		string timeString = "";
		if (time.hours < 10)
			timeString += "0";
		timeString += time.hours + " ";
		if (time.minutes < 10)
			timeString += "0";
		timeString += time.minutes;
		timeLabel.text = timeString;
	}
}
