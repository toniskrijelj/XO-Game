using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
	public static void SetColor(this SpriteRenderer sr, Color color)
	{
		sr.color = color;
	}

	public static void SetColor(this Image image, Color color)
	{
		image.color = color;
	}

	public static HSVColor ToHSVColor(this Color color)
	{
		return new HSVColor(color);
	}

	public static void AttachChildrenToParent(this Transform transform)
	{

		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).SetParent(transform.parent, false);
		}
	}

	public static string ConvertToString(this KeyCode keycode)
	{
		return KeyCodeUtilities.ToString(keycode);
	}
}
