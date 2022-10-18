using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public static class ColorUtilities
{
	public static string SetTextColor(string text, Color color)
	{
		return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
	}

	public static Color GetTextColor(string text)
	{
		return Color(text.Remove(0, 8));
	}

	public static Color Color(string hex)
	{
		float red = Utilities.HexToDec01(hex.Substring(0, 2));
		float green = Utilities.HexToDec01(hex.Substring(2, 2));
		float blue = Utilities.HexToDec01(hex.Substring(4, 2));
		float alpha = 1f;
		if (hex.Length >= 8)
		{
			alpha = Utilities.HexToDec01(hex.Substring(6, 2));
		}
		return new Color(red, green, blue, alpha);
	}

	private static Color[] allColors = null;

	public static Color RandomColor()
	{
		if (allColors == null)
		{
			Type type = typeof(ColorUtilities);
			var fieldInfos = type.GetFields();
			List<Color> colors = new List<Color>();
			for(int i = 0; i < fieldInfos.Length; i++)
			{
				if(fieldInfos[i].FieldType == typeof(Color))
				{
					colors.Add((Color)fieldInfos[i].GetValue(null));
				}
			}
			allColors = colors.ToArray();
		}
		return allColors[UnityEngine.Random.Range(0, allColors.Length)];
	}

	public static Color RandomColor2()
	{
		return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
	}

	public static readonly Color red = Color("EC1919");
	public static readonly Color darkRed = Color("9C1010");
	public static readonly Color orange = Color("EB7619");
	public static readonly Color yellow = Color("EBD319");
	public static readonly Color darkYellow = Color("C7BE26");
	public static readonly Color lime = Color("AEEB19");
	public static readonly Color green = Color("35EB1E");
	public static readonly Color darkGreen = Color("20941C");
	public static readonly Color cyan = Color("1EEBBC");
	public static readonly Color darkCyan = Color("249C97");
	public static readonly Color blue = Color("1EE3EB");
	public static readonly Color darkBlue = Color("24419C");
	public static readonly Color purple = Color("881EEB");
	public static readonly Color darkPurple = Color("5F249C");
	public static readonly Color magenta = Color("CF3FE2");
	public static readonly Color darkMagenta = Color("87249C");
	public static readonly Color pink = Color("E23FB5");
	public static readonly Color darkPink = Color("9C248A");
	public static readonly Color brown = Color("E23FB5");
	public static readonly Color white = Color("FFFFFF");
	public static readonly Color black = Color("191919");
	public static readonly Color lightGray = Color("ADADAD");
	public static readonly Color gray = Color("7D7D7D");
	public static readonly Color darkGray = Color("434343");
}