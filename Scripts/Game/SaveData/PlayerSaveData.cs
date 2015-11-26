using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerSaveData : MonoBehaviour {

    public static PlayerSaveData reference = null;

	public List<JournalQuest> quests = new List<JournalQuest>();
	public TimeData time = new TimeData();
	public PassengerData passengerData = new PassengerData();
	public TrainData trainData = new TrainData ();
	public List<WagonData> wagonData = new List<WagonData> ();
	[Serializable]
	public class TrainData
	{
		public float power;
		public float magicPower;
		public float currentWeight;
		public float maxWeight;
		public int currentCrewCount;
		public float maxCrewCount;
		public float maxSpeed;
		public InventoryItem[] equippedItems = new InventoryItem[3];

		public float GetCurrentSpeed()
		{
			if (maxWeight <= 0)
				return 0;
			if (maxWeight <= currentWeight)
				return 0;
			if (power >= 100)
				return maxSpeed;
			float speedMod = (1 - currentWeight / maxWeight) / ((100 - power) / 100);
			if (speedMod > 1)
				speedMod = 1;
			return maxSpeed * speedMod;
		}
	}

	[Serializable]
	public class WagonData
	{
		public float attraction;
		public int currentPassengersCount;
		public int maxPassengersCount;
		public bool IsFull()
		{
			return currentPassengersCount >= maxPassengersCount;
		}
	}
	



	public void InitGewGame()
	{
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый двигатель"));
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый парораспределительный механизм"));
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый смазочный механизм"));

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("круг призыва"));

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"),0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"),1);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("элитное купе"),2);

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),0, 30);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),0, 25);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),0, 32);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),1, 6);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),1, 7);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),1, 12);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"),1, 13);

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"),2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"),2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стул"),2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стол"),2);
	}

	void Awake()
    {
        if (reference == null)
        {
            DontDestroyOnLoad(gameObject);
            reference = this;
			Debug.Log ("player save data init");
        }
        else if (reference != this)
        {
            Destroy(gameObject);
        }
    }

	[Serializable]
	public class TimeData
	{
		public int hours;
		public int minutes;
	}

	[Serializable]
	public class PassengerData
	{
		public bool IsFull()
		{
			return passengers.Count >= GetMaxPassengers ();
		}
		//public int maxPassengers;
		public int GetMaxPassengers()
		{
			int maxPassengers = 0;
			foreach(PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData)
			{
				maxPassengers += wagon.maxPassengersCount;
			}
			return maxPassengers;
		}
		public List<Passenger> passengers = new List<Passenger> ();
		public void AddPassenger()
		{
			Passenger newPassenger = new Passenger ();
			int freeWagonCount = 0;
			foreach(PlayerSaveData.WagonData wagon in PlayerSaveData.reference.wagonData)
			{
				if (!wagon.IsFull())
				{
					freeWagonCount += 1;
				}
			}
			int selectedWagonIndex = UnityEngine.Random.Range (0,freeWagonCount);
			for (int wagonIndex = selectedWagonIndex; wagonIndex < PlayerSaveData.reference.wagonData.Count; wagonIndex++)
			{
				if (PlayerSaveData.reference.wagonData[wagonIndex].IsFull())
					continue;
				PlayerSaveData.reference.wagonData[wagonIndex].currentPassengersCount += 1;
				newPassenger.wagonIndex = wagonIndex;
				break;
			}
			passengers.Add (newPassenger);

		}
		public void RemovePassenger(Passenger passenger)
		{
			PlayerSaveData.reference.wagonData [passenger.wagonIndex].currentPassengersCount -= 1;
			passengers.Remove (passenger);
		}
		public Passenger GetPassenger(int index)
		{
			return passengers[index];
		}
	}

	[Serializable]
	public class Passenger
	{
		public int wagonIndex;
		public int stationsTravelled;
	}

    [Serializable]
	public class QuestData
    {
		public int index;
		public int currentStage;
	}
	[Serializable]
	class SaveData
	{
		public List<QuestData> quests = new List<QuestData>();
		public TimeData time = new TimeData ();
		public PassengerData passengerData = new PassengerData();
	}

	public bool IsSaveExists()
	{
		if (File.Exists (Application.persistentDataPath + "/Save.sav"))
			return true;
		return false;
	}

    public void Save()
    {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/Save.sav");

		SaveData saveData = new SaveData();

		foreach (JournalQuest quest in quests) 
		{
			QuestData data = new QuestData();
			data.index = JournalQuestsDatabase.reference.GetIndexOf(quest);
			data.currentStage = quest.currentStage;
			saveData.quests.Add(data);
		}

		saveData.time.hours = time.hours;
		saveData.time.minutes = time.minutes;

		saveData.passengerData.passengers = passengerData.passengers;

		bf.Serialize(file, saveData);
		file.Close();
		Debug.Log("Saved!" + " " + Application.persistentDataPath);
	}

	public void Load()
	{
		if (IsSaveExists())
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Save.sav", FileMode.Open);

			SaveData saveData = (SaveData)bf.Deserialize(file);
			quests.Clear();
			foreach (QuestData data in saveData.quests) 
			{
				Debug.Log(data.index);
				JournalQuest quest = JournalQuestsDatabase.reference.FindByIndex(data.index);
				quest.currentStage = data.currentStage;
				quests.Add (quest);
			}

			time.hours = saveData.time.hours;
			time.minutes = saveData.time.minutes;

			passengerData.passengers = saveData.passengerData.passengers;

			file.Close();
			Debug.Log("Loaded!");
		}
	}
	/*
	IEnumerator tmp()
	{
		while (true) 
		{
			Debug.Log ("speed: "+trainData.GetCurrentSpeed ());
			yield return new WaitForSeconds(1);
		}
	}
	void Start()
	{
		StartCoroutine ("tmp");
	}
	*/
}
