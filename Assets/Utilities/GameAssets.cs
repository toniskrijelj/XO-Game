using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameAssets : MonoBehaviour
{
	private static GameAssets _i;
	public static GameAssets i
	{
		get
		{
			if (_i == null)
			{
				_i = Resources.Load<GameAssets>("GameAssets");
			}
			return _i;
		}
	}

	public TMP_FontAsset robotoMedium;
	public Material sphereGradient;
	public Sprite pixel4x4;
	public Sprite circle;
	public Sprite triangle;
	public TMP_FontAsset arcadeFont;
}
