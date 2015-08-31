using UnityEngine;
using System.Collections;

public static class GameController {

	public static void Pause()
	{
		Time.timeScale = 0;
	}

	public static bool IsPaused()
	{
		return Time.timeScale == 0;
	}

	public static void Resume()
	{
		Time.timeScale = 1;
	}

	public static void Quit()
	{
		Application.Quit();
	}
}
