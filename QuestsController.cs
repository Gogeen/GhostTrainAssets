using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class QuestsController : MonoBehaviour {

	public static QuestsController reference;

	public List<string> questFileName = new List<string>();
	static List<Quest> quests = new List<Quest>();

	public static Dictionary<string, int> localParameters = new Dictionary<string, int> ();
	public static Dictionary<string, int> globalParameters = new Dictionary<string, int> ();

	public static void ClearLocalData()
	{
		localParameters.Clear();
	}

	void Awake()
	{
		reference = this;
	}

	void Start()
	{
		foreach (string name in questFileName) {
			Load (name+".xml");
		}
	}

	public static Quest GetQuest(string name)
	{
		foreach(Quest quest in quests){
			if (quest.name == name)
				return quest;
		}
		Debug.Log("Quest "+name+" not found!");
		return null;
	}

	public static bool IsPassRequirements(Quest quest)
	{
		if (QuestsController.reference.CheckRequirements(quest))
			return true;
		return false;
			
	}

	public static bool IsPassRequirements(TextPart part)
	{
		if (QuestsController.reference.CheckRequirements(part))
			return true;
		return false;

	}

	public static bool IsPassRequirements(Answer answer)
	{
		if (QuestsController.reference.CheckRequirements(answer))
			return true;
		return false;

	}

	public static void ApplyResults(Answer answer)
	{
		for (int resultCounter = 0; resultCounter < answer.results.Count; resultCounter++) {
			QuestsController.reference.ApplyResult (answer.results[resultCounter]);
		}
	}

	public static QuestNode FindNode(Quest quest, string pointer)
	{
		/*if (!quests.Contains(quest)){
			Debug.Log ("Quest "+quest.name+" not found!");
			return null;
		}*/
		foreach (QuestNode node in quest.nodes) {
			if (node.name == pointer)
				return node;
		}
		Debug.Log ("Node '"+pointer+"' not found!");
		return null;
	}

	void Save(string fileName)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Quest));
		FileStream stream = File.Create("Assets/Resources/Quests/" + fileName);
		Quest quest = new Quest ();
		serializer.Serialize(stream, quest);
		stream.Close();
		//quests.Add(quest);
	}

	void Load(string fileName)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Quest));
		FileStream stream = File.Open("Assets/Resources/Quests/"+fileName, FileMode.Open);
		Quest quest = serializer.Deserialize(stream) as Quest;
		stream.Close();
		quests.Add(quest);
	}

	[System.Serializable]
	[XmlRoot("Quest")]
	public class Quest
	{
		[XmlAttribute("name")]
		public string name = "questName";

		[XmlElement("Settings")]
		public QuestSettings settings = new QuestSettings ();

		[XmlArray("Requirements")]
		[XmlArrayItem("Requirement")]
		public List<Requirement> requirements = new List<Requirement>();

		[XmlArray("Nodes")]
		[XmlArrayItem("Node")]
		public List<QuestNode> nodes = new List<QuestNode> (); //new QuestNode[]{new QuestNode()}
	}

	[System.Serializable]
	public class QuestSettings
	{
		[XmlElement("ShowUnavailableAnswers")]
		public ShowUnavailableAnswers showUnavailableAnswers = new ShowUnavailableAnswers();
	}

	[System.Serializable]
	public class ShowUnavailableAnswers
	{
		[XmlAttribute("value")]
		public bool value = false;
	}

	[System.Serializable]
	public class QuestNode
	{
		[XmlAttribute("name")]
		public string name = "node1";

		[XmlAttribute("imageName")]
		public string imageName = "imageName";

		[XmlArray("Text")]
		[XmlArrayItem("Part")]
		public List<TextPart> textParts = new List<TextPart>(); //new TextPart[]{new TextPart()}

		[XmlArray("Answers")]
		[XmlArrayItem("Answer")]
		public List<Answer> answers = new List<Answer>(); //new Answer[]{new Answer()}
	}

	[System.Serializable]
	public class TextPart
	{
		[XmlAttribute("text")]
		public string text = "text";

		[XmlArray("Requirements")]
		[XmlArrayItem("Requirement")]
		public List<Requirement> requirements = new List<Requirement>(); //new Requirement[]{new Requirement(), new Requirement()}
	}

	[System.Serializable]
	public class Requirement
	{
		[XmlAttribute("type")]
		public RequirementType type = RequirementType.and;

		[XmlAttribute("function")]
		public Functions function = Functions.Less;

		[XmlAttribute("parameter1")]
		public string p1 = "p1";

		[XmlAttribute("parameter2")]
		public string p2 = "p2";
	}

	[System.Serializable]
	public class Answer
	{
		[XmlAttribute("text")]
		public string text = "text";

		[XmlAttribute("imageName")]
		public string imageName = "imageName";

		[XmlElement("Pointer")]
		public string pointer = "pointer";

		[XmlArray("Requirements")]
		[XmlArrayItem("Requirement")]
		public List<Requirement> requirements = new List<Requirement>(); //new Requirement[]{new Requirement()}

		[XmlArray("Results")]
		[XmlArrayItem("Result")]
		public List<Result> results = new List<Result>(); //new Result[]{new Result()}
	}

	[System.Serializable]
	public class Result
	{
		[XmlAttribute("function")]
		public Functions function = Functions.AddLocalParameter;

		[XmlAttribute("parameter1")]
		public string p1 = "p1";

		[XmlAttribute("parameter2")]
		public string p2 = "p2";
	}

	public enum RequirementType
	{
		and,
		or
	}

	public enum Functions
	{
		Less,
		LessEqual,
		Equal,
		MoreEqual,
		More,
		HasScales,
		AddLocalParameter,
		RemoveLocalParameter,
		IncreaseLocalParameter,
		DecreaseLocalParameter,
		AddGlobalParameter,
		RemoveGlobalParameter,
		IncreaseGlobalParameter,
		DecreaseGlobalParameter,
		AddItem,
		RemoveItem,
		AddTime,
		RemoveTime
	}

	int GetParameterValue(string parameter)
	{
		if (localParameters.ContainsKey (parameter))
			return localParameters [parameter];
		if (globalParameters.ContainsKey (parameter))
			return globalParameters [parameter];
		int value = 0;
		if (int.TryParse (parameter, out value))
			return value;
		Debug.Log ("Wrong parameter in xml file! "+parameter);
		return 0;
	}

	bool CheckRequirement(Requirement requirement)
	{
		switch (requirement.function) {
		case Functions.Less:		{if (GetParameterValue(requirement.p1) >= GetParameterValue(requirement.p2)) 	{return false;} return true;}
		case Functions.LessEqual:	{if (GetParameterValue(requirement.p1) >  GetParameterValue(requirement.p2)) 	{return false;} return true;}
		case Functions.Equal:		{if (GetParameterValue(requirement.p1) != GetParameterValue(requirement.p2)) 	{return false;} return true;}
		case Functions.MoreEqual:	{if (GetParameterValue(requirement.p1) <  GetParameterValue(requirement.p2)) 	{return false;} return true;}
		case Functions.More:		{if (GetParameterValue(requirement.p1) <= GetParameterValue(requirement.p2)) 	{return false;} return true;}
		case Functions.HasScales:	{if (!PlayerSaveData.reference.trainData.conditions.HasScales)					{return false;} return true;}
		default: {Debug.Log("unexpected function name in requirements in xml!"); return false;}
		}

	}

	public bool CheckRequirements(TextPart target)
	{
		bool returnValue = true;
		for (int requirementCounter = 0; requirementCounter < target.requirements.Count; requirementCounter++) {
			Requirement requirement = target.requirements [requirementCounter];
			if (requirement.type == RequirementType.and) {
				if (returnValue == false) {
					continue;
				}
				returnValue = CheckRequirement (requirement);
			}
			else {
				if (returnValue == true) {
					return true;
				}
				returnValue = CheckRequirement (requirement);
			}
		}
		return returnValue;
	}

	public bool CheckRequirements(Answer target)
	{
		bool returnValue = true;
		for (int requirementCounter = 0; requirementCounter < target.requirements.Count; requirementCounter++) {
			Requirement requirement = target.requirements [requirementCounter];
			if (requirement.type == RequirementType.and) {
				if (returnValue == false) {
					continue;
				}
				returnValue = CheckRequirement (requirement);
			}
			else {
				if (returnValue == true) {
					return true;
				}
				returnValue = CheckRequirement (requirement);
			}
		}
		return returnValue;
	}

	public bool CheckRequirements(Quest target)
	{
		bool returnValue = true;
		for (int requirementCounter = 0; requirementCounter < target.requirements.Count; requirementCounter++) {
			Requirement requirement = target.requirements [requirementCounter];
			if (requirement.type == RequirementType.and) {
				if (returnValue == false) {
					continue;
				}
				returnValue = CheckRequirement (requirement);
			}
			else {
				if (returnValue == true) {
					return true;
				}
				returnValue = CheckRequirement (requirement);
			}
		}
		return returnValue;
	}

	public void ApplyResult(Result target)
	{
		Result result = target;
		switch (result.function) {
		case Functions.AddLocalParameter:		{AddLocalParameter (result.p1, result.p2);						break;}
		case Functions.RemoveLocalParameter:	{RemoveLocalParameter (result.p1);								break;}
		case Functions.IncreaseLocalParameter:	{IncreaseLocalParameter (result.p1);							break;}
		case Functions.DecreaseLocalParameter:	{DecreaseLocalParameter (result.p1);							break;}
		case Functions.AddGlobalParameter:		{AddGlobalParameter (result.p1, result.p2);						break;}
		case Functions.RemoveGlobalParameter:	{RemoveGlobalParameter (result.p1);								break;}
		case Functions.IncreaseGlobalParameter:	{IncreaseGlobalParameter (result.p1);							break;}
		case Functions.DecreaseGlobalParameter:	{DecreaseGlobalParameter (result.p1);							break;}
		case Functions.AddItem:					{AddItem (result.p1);											break;}
		case Functions.RemoveItem:				{RemoveItem (result.p1);										break;}
		case Functions.AddTime:					{AddTime (GetParameterValue(result.p1));						break;}
		case Functions.RemoveTime:				{RemoveTime (GetParameterValue(result.p1));						break;}
		default: {Debug.Log("unexpected function name in results in xml!"); break;}
		}

	}

	void AddLocalParameter(string key, string value)
	{
		if (localParameters.ContainsKey (key)) {
			Debug.Log ("unexpected local parameter adding in xml!");
			return;
		}
		localParameters.Add (key, GetParameterValue(value));
	}

	void RemoveLocalParameter(string key)
	{
		if (!localParameters.ContainsKey (key)) {
			Debug.Log ("unexpected local parameter removing in xml!");
			return;
		}
		localParameters.Remove (key);
	}

	void IncreaseLocalParameter(string key)
	{
		if (!localParameters.ContainsKey (key)) {
			Debug.Log ("unexpected local parameter changing in xml!");
			return;
		}
		localParameters[key] += 1;
	}

	void DecreaseLocalParameter(string key)
	{
		if (!localParameters.ContainsKey (key)) {
			Debug.Log ("unexpected local parameter changing in xml!");
			return;
		}
		localParameters[key] -= 1;
	}

	void AddGlobalParameter(string key, string value)
	{
		if (globalParameters.ContainsKey (key)) {
			Debug.Log ("unexpected global parameter adding in xml!");
			return;
		}
		globalParameters.Add (key, GetParameterValue(value));
	}

	void RemoveGlobalParameter(string key)
	{
		if (!globalParameters.ContainsKey (key)) {
			Debug.Log ("unexpected global parameter removing in xml!");
			return;
		}
		globalParameters.Remove (key);
	}

	void IncreaseGlobalParameter(string key)
	{
		if (!globalParameters.ContainsKey (key)) {
			Debug.Log ("unexpected global parameter changing in xml!");
			return;
		}
		globalParameters[key] += 1;
	}

	void DecreaseGlobalParameter(string key)
	{
		if (!globalParameters.ContainsKey (key)) {
			Debug.Log ("unexpected global parameter changing in xml!");
			return;
		}
		globalParameters[key] -= 1;
	}

	void AddItem(string name)
	{
		InventorySystem.reference.InitItem(ItemDatabase.reference.FindByName (name), InventorySystem.SlotType.Wagon);
	}

	void RemoveItem(string name)
	{
		InventoryItemObject item = InventorySystem.reference.FindItem (name);
		if (item != null)
			Destroy(item.gameObject);
	}

	void AddTime(int value)
	{
		PlayerSaveData.reference.time.minutes += value;
	}

	void RemoveTime(int value)
	{
		PlayerSaveData.reference.time.minutes -= value;
	}
}
