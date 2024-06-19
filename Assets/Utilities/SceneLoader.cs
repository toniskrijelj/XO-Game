using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	private static SceneLoader instance;
	public static SceneLoader Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("SceneLoader", typeof(SceneLoader)).GetComponent<SceneLoader>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void LoadScene(int index)
	{
		var sceneLoading = SceneManager.LoadSceneAsync(index);
	}

	public void LoadScene(string scene)
	{
		var sceneLoading = SceneManager.LoadSceneAsync(scene);
	}

	public static void LoadSceneStatic(int index, float delay = 0)
	{
		Instance.LoadScene(index);
	}

	public static void LoadSceneStatic(string name)
	{
		Instance.LoadScene(name);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void SetTimeScale(float timeScale)
	{
		Time.timeScale = timeScale;
	}
}
