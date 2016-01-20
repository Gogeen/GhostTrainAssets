using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class RoadScript : MonoBehaviour {

	public Transform GetNextPoint(Transform point)
	{
		int nextPointIndex = roadPoints.IndexOf (point) + 1;
		if (nextPointIndex >= roadPoints.Count)
			nextPointIndex = roadPoints.Count - 1;
		return roadPoints [nextPointIndex];
	}

	public Transform GetPreviousPoint(Transform point)
	{
		int nextPointIndex = roadPoints.IndexOf (point) - 1;
		if (nextPointIndex < 0)
			nextPointIndex = 0;
		return roadPoints [nextPointIndex];
	}


	float GetAngleBetween(Vector2 first, Vector2 second)
	{
		float angle = Vector2.Angle (first, second);
		Vector3 cross = Vector3.Cross (first, second);
		if (cross.z > 0)
			angle = -angle;
		return angle;
	}
	
	float GetVectorRotation(Vector2 direction)
	{
		return -GetAngleBetween (Vector2.up, direction);
	}

	public GameObject townObject;
	public GameObject waystationObject;
	public List<ObjectSettings> objectsList = new List<ObjectSettings>();
	public List<ObjectSettings> anomaliesList = new List<ObjectSettings>();

	public DifficultySettings CasualSettings;
	public DifficultySettings EasySettings;
	public DifficultySettings NormalSettings;
	public DifficultySettings HardSettings;
	public List<PileSettings> pileSettings = new List<PileSettings> ();
	public TeleportSettings teleportSettings;
	[System.Serializable]
	public class DifficultySettings
	{
		public bool isGenerateObjects;
		public int partsBetweenObjects;
		public bool isGenerateAnomalies;
		public int partsBetweenAnomalies;
	}

	[System.Serializable]
	public class ObjectSettings
	{
		public GameObject reference;
		public int chance;
	}

	[System.Serializable]
	public class PileSettings
	{
		public bool invisible;
		public int hitPoints;
		public int chance;
		public List<Difficulty> appearance = new List<Difficulty>();
	}

	[System.Serializable]
	public class TeleportSettings
	{
		public int minRangeInParts;
		public int maxRangeInParts;
	}

	public GameObject roadPrefab;
	public GameObject railPrefab;
	Transform leftRailPoint = null;
	Transform rightRailPoint = null;

	public Transform roadTransform;
	public Transform roadObjectsTransform;
	public enum RoadType
	{
		Straight,
		Bend
	}

	public enum CommunityType
	{
		None,
		StartTown,
		FinishTown,
		Waystation
	}

	public enum Difficulty
	{
		Casual,
		Easy,
		Normal,
		Hard
	}

	DifficultySettings GetDifficultySettings(Difficulty type)
	{
		switch (type) {
		case Difficulty.Casual: {return CasualSettings;}
		case Difficulty.Easy: {return EasySettings;}
		case Difficulty.Normal: {return NormalSettings;}
		case Difficulty.Hard: {return HardSettings;}
		}
		return null;
	}

	[System.Serializable]
	public class RoadPart
	{
		public RoadType type;
		public Difficulty difficulty;
		public CommunityType community;
		public float bendDegree;
		public int miniPartsCount;
	}
	public List<RoadPart> road = new List<RoadPart>();
	public List<Transform> roadPoints = new List<Transform>();

	GameObject GetRandomObject()
	{
		int chanceSum = 0;
		foreach (ObjectSettings settings in objectsList) {
			chanceSum += settings.chance;
		}
		if (chanceSum > 0) {
			int settingsValue = Random.Range (0, chanceSum);
			foreach (ObjectSettings settings in objectsList) {
				settingsValue -= settings.chance;
				if (settingsValue < 0) {
					return settings.reference;
				}
			}
		}
		return null;
	}

	GameObject GetRandomAnomaly()
	{
		int chanceSum = 0;
		foreach (ObjectSettings settings in anomaliesList) {
			chanceSum += settings.chance;
		}
		if (chanceSum > 0) {
			int settingsValue = Random.Range (0, chanceSum);
			foreach (ObjectSettings settings in anomaliesList) {
				settingsValue -= settings.chance;
				if (settingsValue < 0) {
					return settings.reference;
				}
			}
		}
		return null;
	}

	public Transform GetTeleportPoint(Transform fromPoint)
	{
		int partIndex = roadPoints.IndexOf (fromPoint);
		int nextPartRange = Random.Range (teleportSettings.minRangeInParts, teleportSettings.maxRangeInParts);
		bool isForward = Random.Range (0, 2) == 0;
		if (isForward)
			partIndex += nextPartRange;
		else
			partIndex -= nextPartRange;
		return roadPoints[partIndex];

	}

	void GenerateRoad()
	{
		roadPoints.Clear ();
		for (int roadPartIndex = roadTransform.childCount-1; roadPartIndex >= 0; roadPartIndex--)
			DestroyImmediate (roadTransform.GetChild(roadPartIndex).gameObject);
		for (int objectIndex = roadObjectsTransform.childCount-1; objectIndex >= 0; objectIndex--)
			DestroyImmediate (roadObjectsTransform.GetChild(objectIndex).gameObject);
		Vector3 generatePosition = roadTransform.position;
		Vector3 generateRotation = roadTransform.localEulerAngles;

		int partsToLastObject = 0;
		int partsToLastAnomaly = 0;

		for(int roadPartIndex = 0; roadPartIndex < road.Count; roadPartIndex++)
		{
			// generate road part container
			GameObject roadPart = new GameObject ("roadPart" + roadPartIndex);
			roadPart.transform.parent = roadTransform;
			roadPart.transform.localPosition = new Vector3 (0,0,0);

			DifficultySettings currentDifficulty = GetDifficultySettings(road [roadPartIndex].difficulty);

			for (int roadMiniPartIndex = 0; roadMiniPartIndex < road[roadPartIndex].miniPartsCount; roadMiniPartIndex++)
			{
				// generate road mini part
				GameObject roadMiniPart = Instantiate(roadPrefab) as GameObject;
				Vector3 roadMiniPartLastPointPosition = roadMiniPart.transform.GetChild(1).localPosition;
				roadMiniPartLastPointPosition.x = Mathf.Tan(road[roadPartIndex].bendDegree*2*Mathf.PI/360)*0.1f;
				roadMiniPart.transform.GetChild(1).localPosition = roadMiniPartLastPointPosition;
				roadMiniPart.transform.parent = roadPart.transform;
				roadMiniPart.transform.position = generatePosition;
				roadMiniPart.transform.localEulerAngles = generateRotation;



				roadPoints.Add(roadMiniPart.transform.GetChild(1));

				generatePosition = roadMiniPart.transform.GetChild(1).position;
				generateRotation = roadMiniPart.transform.localEulerAngles;

				Vector3 partSpriteRotation = roadMiniPart.transform.GetChild(2).localEulerAngles;
				partSpriteRotation.z = -road[roadPartIndex].bendDegree;
				roadMiniPart.transform.GetChild(2).localEulerAngles = partSpriteRotation;
				switch (road[roadPartIndex].type)
				{
				case RoadType.Straight: {break;}
				case RoadType.Bend: {generateRotation.z -= road[roadPartIndex].bendDegree; break;}
				}

				// generate rails
				GameObject rail = null;
				if (leftRailPoint != null)
				{
					rail = Instantiate(railPrefab) as GameObject;
					Transform currentLeftRailPoint = roadMiniPart.transform.GetChild(2).GetChild(0);
					rail.transform.position = (leftRailPoint.position + currentLeftRailPoint.position)/2;
					rail.transform.localScale = new Vector3(rail.transform.localScale.x, (leftRailPoint.position - currentLeftRailPoint.position).magnitude*5, rail.transform.localScale.z);
					rail.transform.localEulerAngles = new Vector3(rail.transform.localEulerAngles.x, rail.transform.localEulerAngles.y, GetVectorRotation(currentLeftRailPoint.position - leftRailPoint.position));
					rail.transform.parent = roadMiniPart.transform;
				}
				if (rightRailPoint != null)
				{
					rail = Instantiate(railPrefab) as GameObject;
					Transform currentRightRailPoint = roadMiniPart.transform.GetChild(2).GetChild(1);
					rail.transform.position = (rightRailPoint.position + currentRightRailPoint.position)/2;
					rail.transform.localScale = new Vector3(rail.transform.localScale.x, (rightRailPoint.position - currentRightRailPoint.position).magnitude*5, rail.transform.localScale.z);
					rail.transform.localEulerAngles = new Vector3(rail.transform.localEulerAngles.x, rail.transform.localEulerAngles.y, GetVectorRotation(currentRightRailPoint.position - rightRailPoint.position));
					rail.transform.parent = roadMiniPart.transform;
				}
				leftRailPoint = roadMiniPart.transform.GetChild(2).GetChild(0);
				rightRailPoint = roadMiniPart.transform.GetChild(2).GetChild(1);

				// after generating mini part, generate objects if needed
				partsToLastObject += 1;
				partsToLastAnomaly += 1;

				if (currentDifficulty.isGenerateObjects && partsToLastObject >= currentDifficulty.partsBetweenObjects) {
					// generate random object

					//int objectIndexInList = Random.Range(0,objectsList.Count);
					GameObject generatedObject = Instantiate (GetRandomObject()) as GameObject;
					generatedObject.transform.parent = roadObjectsTransform;
					generatedObject.transform.position = roadMiniPart.transform.position;

					partsToLastObject = 0;

					// if object is pile, set it up
					if (generatedObject.GetComponent<RoadPile>() != null){
						int chanceSum = 0;
						foreach (PileSettings settings in pileSettings) {
							foreach (Difficulty difficulty in settings.appearance) {
								if (road [roadPartIndex].difficulty == difficulty) {
									chanceSum += settings.chance;
									break;
								}
							}
						}
						if (chanceSum > 0) {
							int settingsValue = Random.Range (0, chanceSum);
							foreach (PileSettings settings in pileSettings) {
								foreach (Difficulty difficulty in settings.appearance) {
									if (road [roadPartIndex].difficulty == difficulty) {
										settingsValue -= settings.chance;
										break;
									}
								}
								if (settingsValue < 0) {
									generatedObject.GetComponent<RoadPile> ().isInvisible = settings.invisible;
									generatedObject.GetComponent<RoadPile> ().hitPoints = settings.hitPoints;
									break;
								}
							}
						}
					}
				}

				if (currentDifficulty.isGenerateAnomalies && partsToLastAnomaly >= currentDifficulty.partsBetweenAnomalies) {
					// generate random object

					//int objectIndexInList = Random.Range(0,objectsList.Count);
					GameObject generatedAnomaly = Instantiate (GetRandomAnomaly ()) as GameObject;
					generatedAnomaly.transform.parent = roadObjectsTransform;
					generatedAnomaly.transform.position = roadMiniPart.transform.position;

					partsToLastAnomaly = 0;


				}

				if (roadMiniPartIndex != road [roadPartIndex].miniPartsCount / 2) {
					continue;
				}
				if (road [roadPartIndex].community == CommunityType.None)
					continue;
				GameObject community = null;
				if (road [roadPartIndex].community == CommunityType.StartTown) {
					community = Instantiate (townObject) as GameObject;
					community.transform.FindChild ("Collider").gameObject.SetActive (false);
					GetComponent<PlayerTrainSpawn> ().spawnPoint = roadMiniPart.transform.GetChild(1);
				}
				else if (road [roadPartIndex].community == CommunityType.FinishTown) {
					community = Instantiate (townObject) as GameObject;
				}
				else if (road [roadPartIndex].community == CommunityType.Waystation) {
					community = Instantiate (waystationObject) as GameObject;
				}
				community.transform.parent = roadObjectsTransform;
				community.transform.position = roadMiniPart.transform.position;
			}
		}
		GetComponent<AITrainSpawn> ().spawnPoint = roadPoints[1];

		/*for (int roadObjectIndex = 0; roadObjectIndex < roadObjectsTransform.childCount; roadObjectIndex++) {
			GameObject obj = roadObjectsTransform.GetChild (roadObjectIndex).gameObject;

		}*/
	}

	public bool generateNow;

	void Update()
	{
		if (generateNow)
		{
			GenerateRoad ();
			generateNow = false;
		}
	}
}
