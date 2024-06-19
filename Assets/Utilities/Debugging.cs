using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debugging : MonoBehaviour
{
	private static Canvas canvas;

	[RuntimeInitializeOnLoadMethod]
	private static void CreateCanvasIfNeeded()
	{
		if (canvas == null)
		{
			canvas = Utilities.CreateCanvas("Canvas_Debug");
		}
	}

	public static void Log(string message)
	{
		Debug.Log(message);
		Console.SendMessageToChat(message, Color.white);
	}

	public static void LogWarning(string message)
	{
		Debug.LogWarning(message);
		Console.SendMessageToChat(message, ColorUtilities.yellow);
	}

	public static void LogError(string message)
	{
		Debug.LogError(message);
		Console.SendMessageToChat(message, ColorUtilities.red);
	}

	public static Debugging MousePopup(string message, Color color)
	{
		CreateCanvasIfNeeded();
		Debugging debug = new GameObject("Message", typeof(RectTransform), typeof(Debugging), typeof(TextMeshProUGUI)).GetComponent<Debugging>();
		debug.transform.SetParent(canvas.transform, false);
		debug.rectTransform = (RectTransform)debug.transform;
		debug.textMeshPro = debug.GetComponent<TextMeshProUGUI>();
		debug.color = color;
		debug.textMeshPro.text = message;
		debug.textMeshPro.alignment = TextAlignmentOptions.Midline;
		debug.rectTransform.sizeDelta = new Vector2(200, 50);
		debug.textMeshPro.enableAutoSizing = true;
		debug.transform.position = Input.mousePosition;
		Destroy(debug.gameObject, 3);
		ObjectMove.Create(debug.rectTransform.anchoredPosition, (debug.rectTransform.anchoredPosition + Vector2.up * 1000), 0.3f, (Vector3 pos) => debug.rectTransform.anchoredPosition = pos);
		FunctionTimer.Create(() =>
		{
			Color col = color;
			col.a = 0;
			ObjectColor.Create(debug.color, col, 1, (Color newColor) => debug.color = newColor);
		}, 2);
		return debug;
	}

	public Color color
	{
		get => textMeshPro.color;
		set
		{
			textMeshPro.color = value;
		}
	}

	public string text
	{
		get => textMeshPro.text;
		set
		{
			textMeshPro.text = value;
		}
	}

	RectTransform rectTransform;
	TextMeshProUGUI textMeshPro;

	public void LogMessage(string message)
	{
		Log(message);
	}
}
