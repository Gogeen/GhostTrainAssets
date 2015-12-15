using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour {

	public static GameController reference;


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

	public void LoadMap(int index)
	{
		// something like waiting window should be here
		StartCoroutine(LoadLevel (index));
	}

	IEnumerator LoadLevel(int index)
	{
		GlobalUI.reference.SetState (GlobalUI.States.LoadingScreen);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
		yield return asyncOperation;
		GlobalUI.reference.SetState (GlobalUI.States.Road);

	}
	void Update()
	{

	}
}
