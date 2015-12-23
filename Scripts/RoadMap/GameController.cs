using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour {

	public static GameController reference;
	public GameObject MainCamera;

	void Awake()
	{
		if (reference == null)
		{
			reference = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Pause()
	{
		Time.timeScale = 0;
	}

	public bool IsPaused()
	{
		return Time.timeScale == 0;
	}

	public void Resume()
	{
		Time.timeScale = 1;
	}

	int lastLoadedSceneIndex = -1;
	public void LoadMap(int index)
	{
		UnloadMap ();
		StartCoroutine(LoadLevel (index));
		//LoadLevel (index);
	}

	IEnumerator LoadLevel(int index)
	{
		GlobalUI.reference.SetState (GlobalUI.States.LoadingScreen);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		yield return asyncOperation;
		GlobalUI.reference.SetState (GlobalUI.States.Road);
		lastLoadedSceneIndex = index;
		MainCamera.SetActive (false);
	}
	/*
	void LoadLevel(int index)
	{
		GlobalUI.reference.SetState (GlobalUI.States.LoadingScreen);
		SceneManager.LoadScene(index);
		GlobalUI.reference.SetState (GlobalUI.States.Road);
		lastLoadedSceneIndex = index;
	}
	*/

	public void GameOver()
	{
		UnloadMap ();
		GlobalUI.reference.SetState (GlobalUI.States.MainMenu);
		GlobalUI.reference.SetState (GlobalUI.States.LoadGame);
	}

	public void UnloadMap()
	{
		if (lastLoadedSceneIndex >= 0) {
			Debug.Log ("map should be unloaded");
			SceneManager.UnloadScene (lastLoadedSceneIndex);
			lastLoadedSceneIndex = -1;
			MainCamera.SetActive (true);
		}

	}

}
