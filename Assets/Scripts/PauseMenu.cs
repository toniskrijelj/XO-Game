using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static event Action<bool> OnPause;

	[SerializeField] GameObject visuals = null;
	[SerializeField] Transform text = null;

	bool paused = false;

	public void Toggle()
	{
		paused = !paused;
		visuals.SetActive(paused);
		if(paused)
		{
			transform.SetAsLastSibling();
			ObjectScale.Create(text, .5f, 1, 10, null, LoopSettings.None, "", true);
		}
		Time.timeScale = paused ? 0 : 1;
		OnPause?.Invoke(paused);
	}
}
