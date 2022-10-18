using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffects : MonoBehaviour
{
	private static List<AudioSource> sources = new List<AudioSource>();

	[SerializeField] AudioClip moveSound = null;
	[SerializeField] AudioClip placeSound = null;
	[SerializeField] AudioClip fallSound = null;

	[Range(0f, 1f)][SerializeField] float moveSoundVolume = 1;
	[Range(0f, 1f)][SerializeField] float placeSoundVolume = 1;
	[Range(0f, 1f)][SerializeField] float fallSoundVolume = 1;

	private void OnEnable()
	{
		Player.OnPlayerMove += Player_OnPlayerMove;
		Grid.OnGridChange += Grid_OnGridChange;
		Grid.On3Match += Grid_On3Match;
		Grid.OnGridFull += Grid_OnGridFull;
		PauseMenu.OnPause += PauseMenu_OnPause;
	}

	private void PauseMenu_OnPause(bool paused)
	{
		//PlayClipAtPoint(moveSound, Utilities.MainCamera.transform.position, moveSoundVolume, true);
		for (int i = 0; i < sources.Count; i++)
		{
			if (paused)
				sources[i].Pause();
			else
				sources[i].UnPause();
		}
	}

	private void Grid_OnGridFull()
	{
		PlayClipAtPoint(fallSound, Utilities.MainCamera.transform.position, fallSoundVolume);
	}

	private void Grid_On3Match(Symbol arg1, Vector2Int[] arg2)
	{
		PlayClipAtPoint(fallSound, Utilities.MainCamera.transform.position, fallSoundVolume);
	}

	private void OnDisable()
	{
		Player.OnPlayerMove -= Player_OnPlayerMove;
		Grid.OnGridChange -= Grid_OnGridChange;
		Grid.On3Match -= Grid_On3Match;
		Grid.OnGridFull -= Grid_OnGridFull;
		PauseMenu.OnPause -= PauseMenu_OnPause;
	}

	private void Player_OnPlayerMove(Player arg1, int arg2, int arg3)
	{
		PlayClipAtPoint(moveSound, Utilities.MainCamera.transform.position, moveSoundVolume);
	}

	private void Grid_OnGridChange(int arg1, int arg2, Symbol symbol)
	{
		if(symbol != Symbol.None)
		{
			PlayClipAtPoint(placeSound, Utilities.MainCamera.transform.position, placeSoundVolume);
		}
	}

	public static AudioSource PlayClip(AudioClip clip, bool ignorePause = false)
	{
		return PlayClipAtPoint(clip, Utilities.MainCamera.transform.position, 1, ignorePause);
	}

	public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, bool ignorePause = false)
	{
		AudioSource source = new GameObject("playClipAtPoint", typeof(AudioSource)).GetComponent<AudioSource>();
		DontDestroyOnLoad(source.gameObject);
		source.clip = clip;
		source.transform.position = position;
		source.volume = volume;
		source.Play();
		if (!ignorePause)
		{
			sources.Add(source);
			FunctionTimer.Create(() => { sources.Remove(source); Destroy(source.gameObject); }, clip.length + 1);
		}
		else
		{
			Destroy(source.gameObject, clip.length + 1);
		}
		return source;
	}

	public void PlayMoveSound(bool ignorePause)
	{
		PlayClipAtPoint(moveSound, Utilities.MainCamera.transform.position, moveSoundVolume, ignorePause);
	}
}
