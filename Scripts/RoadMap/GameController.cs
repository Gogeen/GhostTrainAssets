using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour {

	public static GameController reference;
	public GameObject MainCamera;

	public int townSceneIndex = -1;

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
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
		yield return asyncOperation;
		GlobalUI.reference.SetState (GlobalUI.States.Road);
		//lastLoadedSceneIndex = index;
		//MainCamera.SetActive (false);
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

	void LoadTownScene()
	{
		if (townSceneIndex >= 0)
			SceneManager.LoadScene (townSceneIndex);
	}

	public void GameOver()
	{
		GlobalUI.reference.SetState (GlobalUI.States.MainMenu);
		GlobalUI.reference.SetState (GlobalUI.States.LoadGame);
		UnloadMap ();

	}

	public void UnloadMap()
	{
		LoadTownScene ();
		/*
		if (lastLoadedSceneIndex >= 0) {
			Debug.Log ("map should be unloaded");
			SceneManager.UnloadScene (lastLoadedSceneIndex);
			lastLoadedSceneIndex = -1;
			MainCamera.SetActive (true);
		}
		*/
	}

}
