using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {

	public GameObject warningWindow;
	public bool showWarning = false;
	public void ShowRoadWarning(string text)
	{
		GameObject.Find ("Map").GetComponent<RoadFeatureController> ().ToggleFeatures (false);
		showWarning = true;
		warningWindow.GetComponent<TweenPosition> ().from = warningWindow.transform.localPosition;
		warningWindow.GetComponent<TweenPosition> ().to = warningWindow.transform.localPosition - new Vector3(warningWindow.GetComponent<UISprite>().localSize.x,0,0);
		warningWindow.GetComponent<TweenPosition> ().PlayForward ();
		warningWindow.transform.GetChild (0).GetComponent<UILabel> ().text = text;
	}
	public void HideRoadWarning()
	{
		GameObject.Find ("Map").GetComponent<RoadFeatureController> ().ToggleFeatures (true);
		showWarning = false;
		warningWindow.GetComponent<TweenPosition> ().PlayReverse ();
	}
	public bool IsWarningShown()
	{
		return showWarning;
	}
}
