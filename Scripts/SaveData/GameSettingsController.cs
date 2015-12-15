﻿using UnityEngine;
using System.Collections;

public class GameSettingsController : MonoBehaviour {

	public static GameSettingsController reference;

	[Range(0, 100)]public float MasterVolume;
	public UISlider MasterVolumeSlider;

	[Range(0, 100)]public float MusicVolume;
	public UISlider MusicVolumeSlider;

	bool lastCheckboxValue;
	public UIToggle fullScreenCheckbox;

	public void Load()
	{
		MasterVolume = PlayerPrefs.GetFloat ("MasterVolume");
		MusicVolume = PlayerPrefs.GetFloat ("MusicVolume");

		MasterVolumeSlider.value = MasterVolume;
		MusicVolumeSlider.value = MusicVolume;

		fullScreenCheckbox.value = Screen.fullScreen;
		lastCheckboxValue = fullScreenCheckbox.value;
	}

	public void Save()
	{
		PlayerPrefs.SetFloat ("MasterVolume", MasterVolume);
		PlayerPrefs.SetFloat ("MusicVolume", MusicVolume);
	}

	public void Update()
	{
		MasterVolume = MasterVolumeSlider.value;
		AudioListener.volume = MasterVolume / 100;
		MusicVolume = MusicVolumeSlider.value;

		if (lastCheckboxValue != fullScreenCheckbox.value) {
			Screen.fullScreen = fullScreenCheckbox.value;
			lastCheckboxValue = fullScreenCheckbox.value;
		} else if (Screen.fullScreen != fullScreenCheckbox.value) {
			fullScreenCheckbox.value = Screen.fullScreen;
		}
			
	}

	public void GoBack()
	{
		GlobalUI.reference.GoBack ();
	}
}