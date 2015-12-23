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
	public int databaseIndex;
	public Durability durabilityInfo;
	public Cost costInfo;
	public Bonus bonusInfo;
	public UIInfo uiInfo;
	public Extra extraInfo;

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
		public int timePrice;

		public Cost()
		{
			
		}
		
		public Cost(Cost another)
		{
			weight = another.weight;
			crewSpace = another.crewSpace;
			passengerSpace = another.passengerSpace;
			timePrice = another.timePrice;
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

		public float repairPricePercent;
		public float tradePricePercent;

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
			repairPricePercent = another.repairPricePercent;
			tradePricePercent = another.tradePricePercent;
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

	[System.Serializable]
	public class Extra
	{
		public bool Unbreakable;
		public bool MoreEffectiveWithTime;
		[Range(0,1)]public float EfficiencyModifier;
		public float maxPotentialModifier;
		public int TimeToMaxPotential;

		public Extra(Extra another)
		{
			Unbreakable = another.Unbreakable;
			MoreEffectiveWithTime = another.MoreEffectiveWithTime;
			EfficiencyModifier = another.EfficiencyModifier;
			maxPotentialModifier = another.maxPotentialModifier;
			TimeToMaxPotential = another.TimeToMaxPotential;
		}
	}

	public InventoryItem()
	{
		
	}

	public InventoryItem(InventoryItem another)
	{
		name = another.name;
		type = another.type;
		databaseIndex = another.databaseIndex;
		durabilityInfo = new Durability(another.durabilityInfo);
		costInfo = new Cost(another.costInfo);
		bonusInfo = new Bonus(another.bonusInfo);
		uiInfo = new UIInfo(another.uiInfo);
		extraInfo = new Extra (another.extraInfo);
	}
}
