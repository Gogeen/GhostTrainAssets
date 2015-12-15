using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class JournalQuest {

    public string name;
    public List<Stage> stages = new List<Stage>();
	public int currentStage;
    [System.Serializable]
    public class Stage
    {
        public bool isFinishing;
        public string description;
    }
}
