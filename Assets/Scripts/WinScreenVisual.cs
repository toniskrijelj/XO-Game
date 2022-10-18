using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreenVisual : MonoBehaviour
{
	[SerializeField] RectTransform canvasOffset = null;

	[SerializeField] Color tieColor = Color.white;

	TextMeshProUGUI text = null;
	ColorScheme scheme;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
		scheme = GetComponent<ColorScheme>();
		Grid.On3Match += Grid_On3Match;
		Grid.OnGridReset += Grid_OnGridReset;
		Grid.OnGridFull += Grid_OnGridFull;
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		Grid.On3Match -= Grid_On3Match;
		Grid.OnGridReset -= Grid_OnGridReset;
		Grid.OnGridFull -= Grid_OnGridFull;
	}

	private void Grid_OnGridFull()
	{
		PlayScreen("tie");
		scheme.SetSymbolAndUpdateColor(Symbol.None);
		scheme.UpdateColor(tieColor);
	}

	private void Grid_OnGridReset()
	{
		gameObject.SetActive(false);
	}

	private void Grid_On3Match(Symbol symbol, Vector2Int[] arg2)
	{
		PlayScreen(symbol.ToString() + " won");
		scheme.SetSymbolAndUpdateColor(symbol);
	}

	private void PlayScreen(string screenText)
	{
		gameObject.SetActive(true);
		text.text = screenText;
		float winScreenStartScale = 10;
		float winScreenEndScale = 1;
		float winScreenScaleSpeed = 5;
		float shakeIntensity = 25;
		float shakeDuration = 0.05f;
		ObjectScale.Create(text.rectTransform, winScreenStartScale, winScreenEndScale, winScreenScaleSpeed, () => ObjectShake.Create(canvasOffset, shakeIntensity, shakeDuration));

	}
}
