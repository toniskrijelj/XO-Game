using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
	public static readonly Color[] colors =
	{
		ColorUtilities.red,
		ColorUtilities.orange,
		ColorUtilities.yellow,
		ColorUtilities.green,
		ColorUtilities.lime,//maybe add more
		ColorUtilities.cyan,
		ColorUtilities.blue,
		ColorUtilities.magenta,
		ColorUtilities.purple,
		ColorUtilities.white,
	};

	int colorIndex;
	[SerializeField] Symbol symbol = Symbol.X;

	public void IncreaseIndex()
	{
		UpdateColor(colorIndex + 1);
	}

	public void DecreaseIndex()
	{
		UpdateColor(colorIndex - 1);
	}

	private void UpdateColor(int index)
	{
		colorIndex = Utilities.LoopBetween(index, 0, colors.Length - 1);
		ColorScheme.SetSymbolColor(symbol, colors[colorIndex]);
	}
}
