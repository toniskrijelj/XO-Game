using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	private void Start()
	{
		//FunctionPeriodic.Create(() => Debugging.MousePopup("bruh", ColorUtilities.RandomColor2()), 3);
		//KeyButtonUI.Create(() => Console.SendMessageToChat("bruh2"), new Vector2(0, 0), new Vector2(300, 100), Color.white, "AFK", Color.black, KeyCode.LeftControl, KeyCode.LeftAlt, KeyCode.A);
		/*
		KeyButtonUI.CreateRandom(() => Debugging.MousePopup("q", Color.red), "Button1", KeyCode.A, KeyCode.F, KeyCode.A, KeyCode.Alpha2, KeyCode.Return);
		KeyButtonUI.CreateLocked(() => Debugging.MousePopup("e", Color.red), 2, "Button61", KeyCode.B, KeyCode.Z, KeyCode.Y, KeyCode.Alpha4, KeyCode.Return);
		KeyButtonUI.CreateRandom(() => Debugging.MousePopup("s", Color.red), "Button21", KeyCode.E, KeyCode.T, KeyCode.X, KeyCode.Alpha0, KeyCode.Return);
		KeyButtonUI.CreateLocked(() => Debugging.MousePopup("w", Color.red), 0.5f, "Button61", KeyCode.W, KeyCode.I, KeyCode.V, KeyCode.Alpha5, KeyCode.Return);
		KeyButtonUI.CreateRandom(() => Debugging.MousePopup("q", Color.red), "Button41", KeyCode.O, KeyCode.Q, KeyCode.P, KeyCode.Alpha8, KeyCode.Return);
		KeyButtonUI.CreateRandom(() => Debugging.MousePopup("q", Color.red), "Button143", KeyCode.LeftCurlyBracket, KeyCode.Underscore, KeyCode.K, KeyCode.Exclaim, KeyCode.Return);
		MouseButtonUI.CreateRandom(() => Debugging.MousePopup("a", Color.red), "Button21");
		MouseButtonUI.CreateRandom(() => Debugging.MousePopup("bv", Color.red), "Button01");
		MouseButtonUI.CreateRandom(() => Debugging.MousePopup("tr", Color.red), "Button11");
		MouseButtonUI.CreateLocked(() => Debugging.MousePopup("xc", Color.red), 3, "Button52");
		MouseButtonUI.CreateRandom(() => Debugging.MousePopup("sdf", Color.red), "Button93");
		MouseButtonUI.CreateLocked(() => Debugging.MousePopup("r", Color.red), 1.5f, "Button74");*/
		ButtonUI.CreateMouseButton(() => Console.SendMessageToChat("bruh"), "bruh");
		ButtonUI.CreateMouseLocked(() => Console.SendMessageToChat("bruh2"), 2, "bruh2");
		ButtonUI.CreateKeysButton(() => Console.SendMessageToChat("bruh3"), "bruh3", null, null, null, null, KeyCode.G);
		ButtonUI.CreateKeysLocked(() => Console.SendMessageToChat("bruh4"), 2, "bruh4", null, null, null, null, KeyCode.A);
		ButtonUI.CreateRandom(() => Console.SendMessageToChat("bruh5"),"bruh5", null, null, KeyCode.B);
		ButtonUI.Create(() => Console.SendMessageToChat("bruh6"),"bruh6", null, null, null, null, KeyCode.Q);
	}
	//float counter1 = 0;
	//float counter2 = 0;
	private void Update()
	{/*
		counter1 += Time.deltaTime;
		counter2 += Time.deltaTime;
		if(KeyCodeUtilities.GetKeyDown(KeyCode.Exclaim))
		{
			Debugging.MousePopup("PRESSED Exclaim", ColorUtilities.black);
		}
		if(KeyCodeUtilities.GetKey(KeyCode.Exclaim))
		{
			if (counter1 >= .2f)
			{
				Debugging.MousePopup("HOLDING Exclaim", ColorUtilities.gray);
				counter1 = 0;
			}
		}
		if(KeyCodeUtilities.GetKeyUp(KeyCode.Exclaim))
		{
			Debugging.MousePopup("RELEASED Exclaim", ColorUtilities.darkPink);
		}
		if(KeyCodeUtilities.GetKeyDown(KeyCode.Alpha1))
		{
			Debugging.MousePopup("PRESSED Alpha1", ColorUtilities.cyan);
		}
		if (KeyCodeUtilities.GetKey(KeyCode.Alpha1))
		{
			if (counter2 >= .2f)
			{
				Debugging.MousePopup("HOLDING Alpha1", ColorUtilities.yellow);
				counter2 = 0;
			}
		}
		if (KeyCodeUtilities.GetKeyUp(KeyCode.Alpha1))
		{
			Debugging.MousePopup("RELEASED Alpha1", ColorUtilities.green);
		}
		*/
	}
}
