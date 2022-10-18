using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UnityColorEvent : UnityEvent<Color> { }

public class ColorScheme : MonoBehaviour
{
	private static Color xColor = ColorUtilities.white;
	private static Color oColor = ColorUtilities.red;

    public static Color GetSymbolColor(Symbol symbol) => symbol == Symbol.O ? oColor : xColor;

	public static void SetSymbolColor(Symbol symbol, Color color)
	{
		if (symbol == Symbol.O) oColor = color;
		else xColor = color;
		List<ColorScheme> schemes = colorSchemes[symbol];
		foreach (var scheme in schemes)
		{
			scheme.UpdateColor(color);
		}
	}

	private static Dictionary<Symbol, List<ColorScheme>> colorSchemes = new Dictionary<Symbol, List<ColorScheme>>()
	{
		{ Symbol.O, new List<ColorScheme>() },
		{ Symbol.X, new List<ColorScheme>() },
	};

	[SerializeField] bool automaticFind = false;
	[SerializeField] Symbol symbol = Symbol.O;
	[SerializeField] UnityColorEvent setColor = null;
	public Action<Color> setColorAction { set; private get; }

	TextMeshProUGUI[] textComps;
	Image[] imageComps;

	private void Sub(Symbol symbol)
	{
		colorSchemes.TryGetValue(symbol, out var schemesList);
		schemesList?.Add(this);
	}

	private void UnSub(Symbol symbol)
	{
		colorSchemes.TryGetValue(symbol, out var schemesList);
		schemesList?.Remove(this);
	}

	private void Awake()
	{
		Sub(symbol);
		if(symbol != Symbol.None) UpdateColor(GetSymbolColor(symbol));
	}

	private void OnDestroy()
	{
		UnSub(symbol);
	}

	public void SetSymbolAndUpdateColor(Symbol newSymbol)
	{
		UnSub(symbol);
		Sub(newSymbol);
		symbol = newSymbol;
		if (symbol != Symbol.None) UpdateColor(GetSymbolColor(newSymbol));
	}

	public void UpdateColor(Color color)
	{
		setColor?.Invoke(color);
		setColorAction?.Invoke(color);
		if (automaticFind)
			AutomaticUpdateColor(color);
	}

	private void AutomaticUpdateColor(Color color)
	{
		if (textComps == null) textComps = transform.GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < textComps.Length; i++)
		{
			textComps[i].color = color;
		}

		if (imageComps == null) imageComps = transform.GetComponentsInChildren<Image>();
		for (int i = 0; i < imageComps.Length; i++)
		{
			imageComps[i].color = color;
		}
	}
}
