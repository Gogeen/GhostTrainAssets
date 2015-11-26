using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem  {

	public enum Type
	{
		NonEquippable,
		Slot1,
		Slot2,
		Slot3
	}

	public string name;
	public Type type;
	public Durability durabilityInfo;
	public Cost costInfo;
	public Bonus bonusInfo;
	public UIInfo uiInfo;

	[System.Serializable]
	public class Durability
	{
		public float current;
		public float max;

		public Durability()
		{

		}

		public Durability(Durability another)
		{
			current = another.current;
			max = another.max;
		}
	}

	[System.Serializable]
	public class Cost
	{
		public float weight;
		public float crewSpace;
		public int passengerSpace;

		public Cost()
		{
			
		}
		
		public Cost(Cost another)
		{
			weight = another.weight;
			crewSpace = another.crewSpace;
			passengerSpace = another.passengerSpace;
		}
	}
	[System.Serializable]
	public class Bonus
	{
		public float power;
		public float magicPower;
		public float maxWeight;
		public int maxCrewSpace;

		public float attraction;
		public int maxPassengerSpace;
		public float maxSpeed;
		public float equipmentDurability;

		public Bonus()
		{
			
		}
		
		public Bonus(Bonus another)
		{
			power = another.power;
			magicPower = another.magicPower;
			maxWeight = another.maxWeight;
			maxCrewSpace = another.maxCrewSpace;
			attraction = another.attraction;
			maxPassengerSpace = another.maxPassengerSpace;
			maxSpeed = another.maxSpeed;
			equipmentDurability = another.equipmentDurability;
		}
	}

	[System.Serializable]
	public class UIInfo
	{
		public string spriteName;
		public Vector2 size;
		public string description;

		public UIInfo()
		{
			
		}
		
		public UIInfo(UIInfo another)
		{
			spriteName = another.spriteName;
			size = another.size;
			description = another.description;
		}
	}

	public InventoryItem()
	{
		
	}

	public InventoryItem(InventoryItem another)
	{
		name = another.name;
		type = another.type;
		durabilityInfo = new Durability(another.durabilityInfo);
		costInfo = new Cost(another.costInfo);
		bonusInfo = new Bonus(another.bonusInfo);
		uiInfo = new UIInfo(another.uiInfo);
	}
}
