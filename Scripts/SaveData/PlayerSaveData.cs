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

		public float GetSpeedPenalty()
		{
			return maxSpeed - GetCurrentSpeed ();
		}
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
	public class ItemData
	{
		public float currentDurabilityPercent;
		public int index;
		public int wagonIndex;
		public int slotIndex;
		
		public ItemData()
		{
			currentDurabilityPercent = 0;
			index = -1;
			slotIndex = 0;
		}
	}

	[Serializable]
	public class WagonData
	{
		public float attraction;
		public float synergyAttraction;
		public int currentPassengersCount;
		public int maxPassengersCount;
		public bool IsFull()
		{
			return currentPassengersCount >= maxPassengersCount;
		}
		public List<Item> items = new List<Item> ();
	}
	



	public void InitGewGame()
	{
		InventorySystem.reference.Clear ();
		time.minutes = 360;
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый двигатель"), true);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый парораспределительный механизм"), true);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый смазочный механизм"), true);

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("круг призыва"), false);

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), false,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), false,1);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("элитное купе"), false,2);

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,0, 30);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,0, 25);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,0, 32);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,1, 6);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,1, 7);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,1, 12);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), false,1, 13);

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), false,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), false,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стул"), false,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стол"), false,2);
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
		public ItemData[] equippedItems = new ItemData[3];
		public List<ItemData> items = new List<ItemData>();

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

		//save quests data
		foreach (JournalQuest quest in quests) 
		{
			QuestData data = new QuestData();
			data.index = JournalQuestsDatabase.reference.GetIndexOf(quest);
			data.currentStage = quest.currentStage;
			saveData.quests.Add(data);
		}

		//save time data
		saveData.time.minutes = time.minutes;

		//save passengers data
		saveData.passengerData.passengers = passengerData.passengers;

		//save train equipment data
		for(int itemIndex = 0; itemIndex < saveData.equippedItems.Length; itemIndex++)
		{
			ItemData equippedItemData = new ItemData();
			if (trainData.equippedItems[itemIndex] != null)
			{
				equippedItemData.index = trainData.equippedItems[itemIndex].databaseIndex;
				//Debug.Log("saving item index is "+equippedItemData.index);
				equippedItemData.currentDurabilityPercent = (trainData.equippedItems[itemIndex].durabilityInfo.current/trainData.equippedItems[itemIndex].durabilityInfo.max)*100;
			}
			saveData.equippedItems[itemIndex] = equippedItemData;
		}

		//save wagon items data
		for (int wagonIndex = 0; wagonIndex < wagonData.Count; wagonIndex++)
		{
			foreach (Item item in wagonData[wagonIndex].items)
			{
				ItemData itemData = new ItemData();
				Debug.Log ("item is "+item);
				itemData.index = item.reference.databaseIndex;
				itemData.currentDurabilityPercent = (item.reference.durabilityInfo.current/item.reference.durabilityInfo.max)*100;
				itemData.wagonIndex = item.wagonIndex;
				itemData.slotIndex = item.slotIndex;

				saveData.items.Add (itemData);
			}
		}

		bf.Serialize(file, saveData);
		file.Close();
		Debug.Log("Saved!" + " " + Application.persistentDataPath);
	}

	public void LoadData()
	{
		StartCoroutine ("Load");
	}

	public IEnumerator Load()
	{
		if (IsSaveExists())
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Save.sav", FileMode.Open);

			SaveData saveData = (SaveData)bf.Deserialize(file);

			//load quests data
			quests.Clear();
			foreach (QuestData data in saveData.quests) 
			{
				Debug.Log(data.index);
				JournalQuest quest = JournalQuestsDatabase.reference.FindByIndex(data.index);
				quest.currentStage = data.currentStage;
				quests.Add (quest);
			}

			//load time data
			time.minutes = saveData.time.minutes;

			//load passengers data
			passengerData.passengers = saveData.passengerData.passengers;

			//load train equipment data
			InventorySystem.reference.Clear ();
			yield return null;
			for(int itemIndex = 0; itemIndex < saveData.equippedItems.Length; itemIndex++)
			{
				ItemData equippedItemData = saveData.equippedItems[itemIndex];
				if (equippedItemData.index != -1)
				{
					Debug.Log("loading item index is "+equippedItemData.index);

					InventorySystem.reference.InitItem(ItemDatabase.reference.FindByIndex(equippedItemData.index), true);
					trainData.equippedItems[itemIndex].durabilityInfo.current = trainData.equippedItems[itemIndex].durabilityInfo.max*equippedItemData.currentDurabilityPercent/100;
				}
			}

			//load wagon items data
			foreach(WagonData data in wagonData)
			{
				data.items.Clear();
			}

			foreach (ItemData itemData in saveData.items)
			{
				InventorySystem.reference.InitItem(ItemDatabase.reference.FindByIndex(itemData.index), false, itemData.wagonIndex, itemData.slotIndex);
				InventoryItem itemInfo = wagonData[itemData.wagonIndex].items[wagonData[itemData.wagonIndex].items.Count - 1].reference;
				itemInfo.durabilityInfo.current = itemInfo.durabilityInfo.max*itemData.currentDurabilityPercent/100;
			}
			file.Close();
			Debug.Log("Loaded!");
		}
		yield return true;
	}
}
