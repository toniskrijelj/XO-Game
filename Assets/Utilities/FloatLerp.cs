using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLerp : Function<FloatLerp>
{
	public static FloatLerp Create(float start, float end, float speed, Action<float> onValueChanged, bool destroyOnComplete = true, Action onComplete = null, string name = "", bool useUnscaledDeltaTime = false)
	{
		FloatLerp function = Create(name, useUnscaledDeltaTime);
		function.value = start;
		function.target = end;
		function.speed = speed;
		function.onComplete = onComplete;
		function.destroyOnComplete = destroyOnComplete;
		function.onValueChanged = onValueChanged;
		return function;
	}
	
	public static FloatLerp CreateEmpty(Action<float> onValueChanged, Action onComplete = null, string name = "", bool useUnscaledDeltaTime = false)
	{
		FloatLerp function = Create(name, useUnscaledDeltaTime);
		function.value = 1;
		function.target = 1;
		function.speed = 0;
		function.onComplete = onComplete;
		function.onValueChanged = onValueChanged;
		function.destroyOnComplete = false;
		return function;
	}

	public float target;
	public float value;

	public float speed;
	public bool destroyOnComplete;

	public Action onComplete { set; protected get; }
	public Action<float> onValueChanged { set; protected get; }

	private bool completed;

	protected override void UpdateAction()
	{
		float newValue = Utilities.FloatMove(value, target, speed * (useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime), out bool clamped);
		if (value != newValue)
		{
			completed = false;
			onValueChanged?.Invoke(newValue);
		}
		value = newValue;
		if (clamped)
		{
			if (!completed)
			{
				completed = true;
				onComplete?.Invoke();
			}
			if (destroyOnComplete)
				Destroy();
		}
	}
}
