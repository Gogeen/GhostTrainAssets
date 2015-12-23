using UnityEngine;
using System.Collections;
public class TrainTimeScript : MonoBehaviour {

	public static TrainTimeScript reference = null;

	int passengersCameShown = 0;

	bool IsTimeOut()
	{
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		return (time.minutes <= 0);
	}

	public float waitingTimeSpeedMultiplier;
	public int passengerTickTime;
	public int basePassengerTimeIncome;
	public float passengerTravelMultiplier;

	public WorldTowns.PassengerInfo communityTimeInfo;

	float passengersCame = 0;
	int waitingTime = 0;
	float passengersCameOut = 0;
	IEnumerator WaitForPassengers()
	{
		int minPercent = 0;
		int maxPercent = 0;
		if (GlobalUI.reference.IsState (GlobalUI.States.Town)) {
			minPercent = TownController.reference.passengerInfo.MinPercentIn;
			maxPercent = TownController.reference.passengerInfo.MaxPercentIn;
		} else if (GlobalUI.reference.IsState (GlobalUI.States.Waystation)) {
			minPercent = WaystationController.reference.passengerInfo.MinPercentIn;
			maxPercent = WaystationController.reference.passengerInfo.MaxPercentIn;
		}

		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		if (passengersData.IsFull())
		{
			ToggleWaitForPassengers();
			return true;
		}
		float waitinGameSeconds = 0;
		while(true)
		{
			Debug.Log ("waiting");
			waitinGameSeconds -= Time.unscaledDeltaTime * waitingTimeSpeedMultiplier;
			while (waitinGameSeconds < 0)
			{
				waitinGameSeconds += 1;
				AddTime (-1);
				waitingTime += 1;
			}
			if (waitingTime >= passengerTickTime)
			{
				waitingTime -= passengerTickTime;

				float passengersComing = ((float)passengersData.GetMaxPassengers() * Random.Range (minPercent, maxPercent)) / 100;
				passengersCame += passengersComing;

				passengersCameShown += (int)passengersCame;
				
				Color color = passengersLabel.color;
				color.a = 255;
				passengersLabel.color = color;
				passengersLabel.text = "passengers +" + passengersCameShown;

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

	public void SimulateWaitForPassengers(float time)
	{
		int minPercent = 0;
		int maxPercent = 0;
		minPercent = communityTimeInfo.MinPercentIn;
		maxPercent = communityTimeInfo.MaxPercentIn;

		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		if (passengersData.IsFull())
		{
			return;
		}
		float waitingTime = time;
		while(true)
		{
			if (waitingTime >= passengerTickTime) {
				waitingTime -= passengerTickTime;

				float passengersComing = ((float)passengersData.GetMaxPassengers () * Random.Range (minPercent, maxPercent)) / 100;
				passengersCame += passengersComing;

				passengersCameShown += (int)passengersCame;

				Color color = passengersLabel.color;
				color.a = 255;
				passengersLabel.color = color;
				passengersLabel.text = "passengers +" + passengersCameShown;

				while (passengersCame >= 1) {
					passengersData.AddPassenger ();
					passengersCame -= 1;
					if (passengersData.IsFull ()) {
						break;
					}
				}
				if (passengersData.IsFull ()) {
					break;
				}

			} else
				return;
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
		int timeIncome = 0;
		int passengersOut = 0;
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

			timeIncome += (int)(basePassengerTimeIncome*attractionMod*Mathf.Pow(passengerTravelMultiplier,passengerComingOut.stationsTravelled));

			passengersData.RemovePassenger(passengerComingOut);

			passengersCameOut -= 1;
			passengersOut += 1;
		}
		Color color = timeIncomeLabel.color;
		color.a = 255;
		timeIncomeLabel.color = color;
		timeIncomeLabel.text = "minutes +" + timeIncome;

		color = passengersLabel.color;
		color.a = 255;
		passengersLabel.color = color;
		passengersLabel.text = "passengers -" + passengersOut;

	}

	public void ComeInCommunity()
	{
		int minPercent = 0;
		int maxPercent = 0;
		if (GlobalUI.reference.IsState (GlobalUI.States.Town)) {
			minPercent = TownController.reference.passengerInfo.MinPercentOut;
			maxPercent = TownController.reference.passengerInfo.MaxPercentOut;
		} else if (GlobalUI.reference.IsState (GlobalUI.States.Waystation)) {
			minPercent = WaystationController.reference.passengerInfo.MinPercentOut;
			maxPercent = WaystationController.reference.passengerInfo.MaxPercentOut;
		}
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		UpdatePassengersTravelTime ();
		RemovePassengersComingOut (((float)passengersData.GetMaxPassengers() * Random.Range (minPercent, maxPercent)) / 100);
	}
	/*
	public void ComeInWaystation()
	{
		PlayerSaveData.PassengerData passengersData = PlayerSaveData.reference.passengerData;
		UpdatePassengersTravelTime ();
		RemovePassengersComingOut (((float)passengersData.GetMaxPassengers() * Random.Range (minPercent, maxPercent)) / 100);
	}
	*/
	public bool HaveEnoughTime(int minutes)
	{
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		time.minutes -= minutes;
		bool result = true;
		if (IsTimeOut())
		{
			result = false;
		}
		time.minutes += minutes;
		return result;
	}

	public void AddTime(int minutes)
	{
		if (minutes == 0)
			return;
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		time.minutes += minutes;
	}

	bool waitForPassengers = false;
	public void  ToggleWaitForPassengers()
	{
		// player can wait for passengers only in towns and waystations
		if (!GlobalUI.reference.IsState (GlobalUI.States.Town) && !GlobalUI.reference.IsState (GlobalUI.States.Waystation)) {
			waitForPassengers = false;
		} else {
			waitForPassengers = !waitForPassengers;
		}
		//waitForPassengers = !waitForPassengers;
		if (waitForPassengers)
		{
			StartCoroutine("WaitForPassengers");
		}
		else
		{
			StopCoroutine("WaitForPassengers");
		}
	}

	bool continueTicking = true;
	float gameSeconds = 0;
	void GameTimerTick()
	{
		if (!continueTicking)
			return;
		if (IsTimeOut())
		{
			Debug.Log ("time is out");
			continueTicking = false;
			return;
		}
		if (PlayerTrain.reference == null)
		{
			return;
		}
		gameSeconds -= Time.deltaTime;
		if (PlayerTrain.reference.ghostMode) 
		{
			gameSeconds -= Time.deltaTime;
		}
		while (gameSeconds < 0)
		{
			gameSeconds += 1;
			AddTime (-1);
		}
	}

	IEnumerator GameTimer()
	{
		Debug.Log ("start timer");
		while (true) 
		{
			if (IsTimeOut())
			{
				//Debug.Log ("time is out");
				break;
			}
			if (PlayerTrain.reference == null)
			{
				//Debug.Log ("player not initialized");
				yield return null;
				//continue;
			}
			else
			{
				if (PlayerTrain.reference.ghostMode) 
				{
					gameSeconds -= Time.deltaTime;

				}
			}
			gameSeconds -= Time.deltaTime;
			Debug.Log ("timer works");
			while (gameSeconds < 0)
			{
				gameSeconds += 1;
				AddTime (-1);
			}
			yield return null;
		}
		Debug.Log ("finished timer");

	}

	void Start()
	{
		//StartCoroutine ("GameTimer");
	}

	void Update()
	{
		UpdateUI ();
		GameTimerTick ();
		if (IsTimeOut())
		{
			GameController.reference.GameOver ();
		}
	}

	public UILabel timeLabel;
	void UpdateUI()
	{
		PlayerSaveData.TimeData time = PlayerSaveData.reference.time;
		string timeString = "";
		if (time.minutes/60 < 10)
			timeString += "0";
		timeString += time.minutes/60 + " ";
		if (time.minutes%60 < 10)
			timeString += "0";
		timeString += time.minutes%60;
		timeLabel.text = timeString;

		Color passengersColor = passengersLabel.color;
		Color timeIncomeColor = timeIncomeLabel.color;
		passengersColor.a -= 1;
		timeIncomeColor.a -= 1;
		passengersLabel.color = passengersColor;
		timeIncomeLabel.color = timeIncomeColor;
		if (passengersColor.a == 0)
			passengersCameShown = 0;
	}

	public UILabel passengersLabel;
	public UILabel timeIncomeLabel;
}
