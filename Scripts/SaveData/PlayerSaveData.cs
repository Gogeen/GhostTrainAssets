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
	public string townName;

	public BasicEngine basicEngine;

	[Serializable]
	public class BasicEngine
	{
		public bool OnUse;
		public float speed;
		public float power;
		public float maxWeight;
	}

	[Serializable]
	public class Conditions
	{
		public bool Invulnerable = false;
		public bool Shield = false;
		public bool LostControl = false;
		public bool CanManageInventory = true;
		public bool HasScales = false;
	}

	[Serializable]
	public class TrainData
	{
		public Conditions conditions;
		public float power;
		public float magicPower;
		public float currentWeight;
		public float maxWeight;
		public int currentCrewCount;
		public float maxCrewCount;
		public float maxSpeed;
		public float bonusEquipmentDurability;
		public float bonusTradePricePercent;
		public float bonusRepairPricePercent;
		public InventoryItemObject[] equippedItems = new InventoryItemObject[3];

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
		public float synergyMagicPower;
		public int currentPassengersCount;
		public int maxPassengersCount;
		public bool IsFull()
		{
			return currentPassengersCount >= maxPassengersCount;
		}
		public List<InventoryItemObject> items = new List<InventoryItemObject> ();
	}
	

	public void LoadTownInfo(string name)
	{
		WorldTowns.TownInfo info = WorldTowns.reference.FindByName (name);
		if (info != null){
			TownController.reference.name = info.name;
			TownController.reference.passengerInfo = info.passengerInfo;
			TownController.reference.shopInfo = info.shopInfo;
			TownController.reference.questName = info.questName;
			TrainTimeScript.reference.communityTimeInfo = info.passengerInfo;
		}
		InventorySystem.reference.LoadShopInfo (info.shopInfo);
		StartCoroutine (InventorySystem.reference.InitShop());
		townName = name;
	}

	public void LoadWaystationInfo(string name)
	{
		WorldWaystations.WaystationInfo info = WorldWaystations.reference.FindByName (name);
		if (info != null){
			WaystationController.reference.name = info.name;
			WaystationController.reference.passengerInfo = info.passengerInfo;
			WaystationController.reference.shopInfo = info.shopInfo;
			//WaystationController.reference.quest = info.quest;
			TrainTimeScript.reference.communityTimeInfo = info.passengerInfo;
		}
		InventorySystem.reference.LoadShopInfo (info.shopInfo);
		StartCoroutine (InventorySystem.reference.InitShop());
	}

	public void InitGewGame()
	{
		quests.Clear ();

		InventorySystem.reference.Clear ();
		LoadTownInfo ("town1");
		time.minutes = 1200;
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый двигатель"), InventorySystem.SlotType.Equipment);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый парораспределительный механизм"), InventorySystem.SlotType.Equipment);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовый смазочный механизм"), InventorySystem.SlotType.Equipment);

		// first wagon
		/*
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), InventorySystem.SlotType.Wagon,0,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), InventorySystem.SlotType.Wagon,0,12);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), InventorySystem.SlotType.Wagon,0,24);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), InventorySystem.SlotType.Wagon,0,4);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), InventorySystem.SlotType.Wagon,0,16);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("отстойное купе"), InventorySystem.SlotType.Wagon,0,28);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), InventorySystem.SlotType.Wagon,0,30);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), InventorySystem.SlotType.Wagon,0,36);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), InventorySystem.SlotType.Wagon,0,42);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), InventorySystem.SlotType.Wagon,0,31);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), InventorySystem.SlotType.Wagon,0,37);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("дерево"), InventorySystem.SlotType.Wagon,0,43);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), InventorySystem.SlotType.Wagon,0,34);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), InventorySystem.SlotType.Wagon,0,40);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), InventorySystem.SlotType.Wagon,0,46);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), InventorySystem.SlotType.Wagon,0,35);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), InventorySystem.SlotType.Wagon,0,41);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("сахар"), InventorySystem.SlotType.Wagon,0,47);
		*/

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,0);

		// second wagon
		/*
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), InventorySystem.SlotType.Wagon,1,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), InventorySystem.SlotType.Wagon,1,12);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), InventorySystem.SlotType.Wagon,1,24);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), InventorySystem.SlotType.Wagon,1,4);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), InventorySystem.SlotType.Wagon,1,16);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("базовое купе"), InventorySystem.SlotType.Wagon,1,28);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("элитное купе"), InventorySystem.SlotType.Wagon,1,42);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("элитное купе"), InventorySystem.SlotType.Wagon,1,46);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стул"), InventorySystem.SlotType.Wagon,1,36);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стул"), InventorySystem.SlotType.Wagon,1,40);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стол"), InventorySystem.SlotType.Wagon,1,37);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("стол"), InventorySystem.SlotType.Wagon,1,41);
		*/

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,1);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,1);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,1);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,1);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,1);


		// third wagon
		/*
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("круг призыва"), InventorySystem.SlotType.Wagon,2,0);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("круг призыва"), InventorySystem.SlotType.Wagon,2,4);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("круг призыва"), InventorySystem.SlotType.Wagon,2,36);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("круг призыва"), InventorySystem.SlotType.Wagon,2,40);
		*/

		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,2);
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName ("фонарь"), InventorySystem.SlotType.Wagon,2);
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
		/// <summary>
		/// Removes the passenger.
		/// </summary>
		/// <param name="passenger">Passenger.</param>
		public void RemovePassenger(Passenger passenger)
		{
			PlayerSaveData.reference.wagonData [passenger.wagonIndex].currentPassengersCount -= 1;
			passengers.Remove (passenger);
		}
		/// <summary>
		/// Removes random passenger from specific wagon.
		/// </summary>
		/// <param name="wagonIndex">Wagon index.</param>
		public void RemovePassenger(int wagonIndex)
		{
			int passengersCount = 0;
			foreach (Passenger passenger in passengers) {
				if (passenger.wagonIndex == wagonIndex)
					passengersCount += 1;
			}
			int passengerIndexToRemove = UnityEngine.Random.Range (0, passengersCount);
			int passengerIndex = 0;
			foreach (Passenger passenger in passengers) {
				if (passenger.wagonIndex == wagonIndex) {
					if (passengerIndex == passengerIndexToRemove) {
						RemovePassenger (passenger);
						return;
					}
					passengerIndex += 1;
				}
			}
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
		public string townName;
		public WorldData worldData = new WorldData();
		public Dictionary<string, int> questParameters = new Dictionary<string, int> ();
	}

	[Serializable]
	class WorldData
	{
		public MailData mailData = new MailData();
	}

	[Serializable]
	class MailData
	{
		public int remainingTime = 0;
		public bool canComplete = false;
		public bool inProgress = false;
	}

	public bool IsSaveExists(int index)
	{
		if (File.Exists (Application.persistentDataPath + "/Save"+index.ToString()+".sav"))
			return true;
		return false;
	}

    public void Save()
    {
		BinaryFormatter bf = new BinaryFormatter();
		if (IsSaveExists (3))
			File.Delete (Application.persistentDataPath + "/Save3.sav");
		if (IsSaveExists (2))
			File.Move (Application.persistentDataPath + "/Save2.sav", Application.persistentDataPath + "/Save3.sav");
		if (IsSaveExists (1))
			File.Move (Application.persistentDataPath + "/Save1.sav", Application.persistentDataPath + "/Save2.sav");
		
		FileStream file = File.Create(Application.persistentDataPath + "/Save1.sav");
		
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
			InventoryItemObject item = trainData.equippedItems [itemIndex];
			if (item != null)
			{
				equippedItemData.index = item.info.databaseIndex;
				equippedItemData.currentDurabilityPercent = (item.info.durabilityInfo.current/item.info.durabilityInfo.max)*100;
			}
			saveData.equippedItems[itemIndex] = equippedItemData;
		}

		//save wagon items data
		for (int wagonIndex = 0; wagonIndex < wagonData.Count; wagonIndex++)
		{
			foreach (InventoryItemObject item in wagonData[wagonIndex].items)
			{
				ItemData itemData = new ItemData();
				itemData.index = item.info.databaseIndex;
				itemData.currentDurabilityPercent = (item.info.durabilityInfo.current/item.info.durabilityInfo.max)*100;
				itemData.wagonIndex = item.wagonIndex;
				itemData.slotIndex = item.slotIndex;

				saveData.items.Add (itemData);
			}
		}

		saveData.townName = townName;

		saveData.worldData.mailData.remainingTime = MailQuestsController.reference.timeForQuest;
		saveData.worldData.mailData.canComplete = MailQuestsController.reference.canComplete;
		saveData.worldData.mailData.inProgress = MailQuestsController.reference.inProgress;

		saveData.questParameters = QuestsController.globalParameters;

		bf.Serialize(file, saveData);
		file.Close();
		Debug.Log("Saved!" + " " + Application.persistentDataPath);
	}

	public void LoadData(int index)
	{
		StrategyMapUIController.reference.LoadInfo ();
		StartCoroutine ("Load", index);
	}

	public IEnumerator Load(int index)
	{
		if (IsSaveExists(index))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/Save"+index.ToString()+".sav", FileMode.Open);

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

					InventorySystem.reference.InitItem(ItemDatabase.reference.FindByIndex(equippedItemData.index), InventorySystem.SlotType.Equipment);
					trainData.equippedItems[itemIndex].info.durabilityInfo.current = trainData.equippedItems[itemIndex].info.durabilityInfo.max*equippedItemData.currentDurabilityPercent/100;
				}
			}

			//load wagon items data
			foreach(WagonData data in wagonData)
			{
				data.items.Clear();
			}

			foreach (ItemData itemData in saveData.items)
			{
				InventorySystem.reference.InitItem(ItemDatabase.reference.FindByIndex(itemData.index), InventorySystem.SlotType.Wagon, itemData.wagonIndex, itemData.slotIndex);
				InventoryItem itemInfo = wagonData[itemData.wagonIndex].items[wagonData[itemData.wagonIndex].items.Count - 1].info;
				itemInfo.durabilityInfo.current = itemInfo.durabilityInfo.max*itemData.currentDurabilityPercent/100;
			}

			townName = saveData.townName;
			LoadTownInfo (townName);

			MailQuestsController.reference.timeForQuest = saveData.worldData.mailData.remainingTime;
			MailQuestsController.reference.canComplete = saveData.worldData.mailData.canComplete;
			MailQuestsController.reference.inProgress = saveData.worldData.mailData.inProgress;

			QuestsController.globalParameters = saveData.questParameters;


			file.Close();
			Debug.Log("Loaded!");
		}
		yield return true;
	}

	void Update()
	{
		if (trainData.equippedItems [0] == null || trainData.equippedItems [0].IsBroken()) {
			if (!basicEngine.OnUse) {
				basicEngine.OnUse = true;
				trainData.maxSpeed += basicEngine.speed;
				trainData.maxWeight += basicEngine.maxWeight;
				trainData.power += basicEngine.power;
			}
		} else {
			if (basicEngine.OnUse) {
				basicEngine.OnUse = false;
				trainData.maxSpeed -= basicEngine.speed;
				trainData.maxWeight -= basicEngine.maxWeight;
				trainData.power -= basicEngine.power;
			}
		}
	}
}
