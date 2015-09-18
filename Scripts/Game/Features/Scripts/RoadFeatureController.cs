using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadFeatureController : MonoBehaviour {

	public PlayerTrain playerTrain;
	public List<FeatureInfo> usedFeatures = new List<FeatureInfo>();

	Features activeFeatures = Features.None;
	
	public enum Features
	{
		None = 0,
		BreakControl = 1,
		AnotherFeature = 2,
		AnotherOneFeature = 4
	}
	[System.Serializable]
	public class FeatureInfo
	{
		public Features type;
		public RoadFeature reference;
	}

	public bool IsFeatureActive(Features feature)
	{
		return ((activeFeatures & feature) == feature);
	}

	public void ToggleFeatures(bool value)
	{
		foreach (FeatureInfo feature in usedFeatures) 
		{
			if (IsFeatureActive(feature.type))
				feature.reference.ToggleFeature(value);
		}
	}

	void Start()
	{
		foreach (FeatureInfo feature in usedFeatures) 
		{
			if (feature.reference != null)
			{
				activeFeatures = activeFeatures | feature.type;
				feature.reference.OnStart();
			}
		}
	}

	void Update()
	{
		foreach (FeatureInfo feature in usedFeatures)
		{
			if (!IsFeatureActive(feature.type))
				continue;
			feature.reference.OnUpdate();
			if (feature.reference.CanCast())
			{
				StartCoroutine (feature.reference.Cast(playerTrain));
			}
		}
	}
}
