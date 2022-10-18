using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FunctionKey : Function<FunctionKey>
{
	public static FunctionKey Create(KeyCode key, Action onDown, Action onHold, Action onUp, string name = "")
	{
		FunctionKey functionKey = Create(name, false);
		functionKey.key = key;
		functionKey.onDown = onDown;
		functionKey.onHold = onHold;
		functionKey.onUp = onUp;
		return functionKey;
	}

	public static FunctionKey Create(KeyCode key, Action onPress, bool shift = false, bool alt = false, bool ctrl = false, string name = "")
	{
		FunctionKey functionKey = Create(name, false);
		functionKey.onDown = onPress;
		functionKey.key = key;
		functionKey.shift = shift;
		functionKey.alt = alt;
		functionKey.ctrl = ctrl;
		return functionKey;
	}

	/*
	public static FunctionKey Create(ButtonUI button, KeyCode key, bool shift = false, bool alt = false, bool ctrl = false, string name = "")
	{
		FunctionKey functionKey = Create(name, false);
		functionKey.onDown = () =>
		{
			if (button == null)
			{
				functionKey.Destroy();
			}
			else
			{
				//button.OnPointerEnter(new PointerEventData(EventSystem.current));
				button.OnPointerDown(new PointerEventData(EventSystem.current));
			}
		};
		functionKey.onUp = () =>
		{
			if (button == null)
			{
				functionKey.Destroy();
			}
			else
			{
				button.OnPointerUp(new PointerEventData(EventSystem.current));
				//button.OnPointerExit(new PointerEventData(EventSystem.current));
			}
		};
		functionKey.key = key;
		functionKey.shift = shift;
		functionKey.alt = alt;
		functionKey.ctrl = ctrl;
		return functionKey;
	}
	*/

	public bool locked = false;

	public KeyCode key;
	public bool shift;
	public bool alt;
	public bool ctrl;
	public Action onDown { private get; set; }
	[SerializeField] private UnityEvent unityOnDown = null;
	public Action onHold { private get; set; }
	[SerializeField] private UnityEvent unityOnHold = null;
	public Action onUp { private get; set; }
	[SerializeField] private UnityEvent unityOnUp = null;

	protected override void UpdateAction()
	{
		if (locked) return;
		if(Input.GetKeyDown(key) && (Input.GetKey(KeyCode.LeftControl) || !ctrl) && (Input.GetKey(KeyCode.LeftShift) || !shift) && (Input.GetKey(KeyCode.LeftAlt) || !alt))
		{
			onDown?.Invoke();
			unityOnDown?.Invoke();
		}
		if(Input.GetKey(key) && (Input.GetKey(KeyCode.LeftControl) || !ctrl) && (Input.GetKey(KeyCode.LeftShift) || !shift) && (Input.GetKey(KeyCode.LeftAlt) || !alt))
		{
			onHold?.Invoke();
			unityOnHold?.Invoke();
		}
		if(Input.GetKeyUp(key) && (Input.GetKey(KeyCode.LeftControl) || !ctrl) && (Input.GetKey(KeyCode.LeftShift) || !shift) && (Input.GetKey(KeyCode.LeftAlt) || !alt))
		{
			onUp?.Invoke();
			unityOnUp?.Invoke();
		}
	}

	public void Lock(bool value)
	{
		locked = value;
	}
}
