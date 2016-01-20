using UnityEngine;
using System.Collections;

public class ConsoleController : MonoBehaviour {

	public string speedCheat; // speed +/value/ representation
	public string invulnerabilityCheat;
	public UILabel consoleLabel;
	public UIInput inputField;

	void OnEnable()
	{
		inputField.isSelected = true;
	}

	public void CheckCheats(string input)
	{
		
		if (isSpeedCheat (input) && getSpeedCheatValue (input) != 0) {
			consoleLabel.text += "\nSpeed improved by " + getSpeedCheatValue (input);
			PlayerSaveData.reference.trainData.maxSpeed += getSpeedCheatValue (input);
		} else if (isInvulnerabilityCheat (input)) {
			PlayerSaveData.reference.trainData.conditions.Invulnerable = !PlayerSaveData.reference.trainData.conditions.Invulnerable;
			consoleLabel.text += "\nInvulnerability is ";
			if (PlayerSaveData.reference.trainData.conditions.Invulnerable) {
				consoleLabel.text += "on";
			} else {
				consoleLabel.text += "off";
			}

		}
		inputField.value = "";
	}

	bool isSpeedCheat(string input)
	{
		if (input.Length < speedCheat.Length+2) // for 'space' & '+' symbol
			return false;
		foreach (char chr in speedCheat) {
			if (input [speedCheat.IndexOf (chr)] != chr)
				return false;
		}
		if (input [speedCheat.Length] != ' ')
			return false;
		return true;
	}

	bool isInvulnerabilityCheat(string input)
	{
		return invulnerabilityCheat == input;
	}

	int getSpeedCheatValue(string input)
	{
		int mod = 0;
		if (input [speedCheat.Length + 1] == '+')
			mod = 1;
		else if (input [speedCheat.Length + 1] == '-')
			mod = -1;
		else
			return 0;
		int speedValue = 0;
		string stringValue = "";
		for (int remainingSymbolIndex = speedCheat.Length + 2; remainingSymbolIndex < input.Length; remainingSymbolIndex++) {
			stringValue += input [remainingSymbolIndex];
		}
		if (int.TryParse (stringValue, out speedValue)) {
			return speedValue*mod;
		}
		return 0;
	}
}
