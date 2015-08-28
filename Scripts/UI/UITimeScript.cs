using UnityEngine;
using System.Collections;

public class UITimeScript : MonoBehaviour {
	

	// Update is called once per frame
	void Update () {
		string time = "";
		if (TrainTimeScript.hours < 10)
			time += "0";
		time += TrainTimeScript.hours + ":";
		if (TrainTimeScript.minutes < 10)
			time += "0";
		time += TrainTimeScript.minutes + ":";
		if (TrainTimeScript.seconds < 10)
			time += "0";
		time += (int)TrainTimeScript.seconds;
		GetComponent<UILabel>().text = time;
	}
}
