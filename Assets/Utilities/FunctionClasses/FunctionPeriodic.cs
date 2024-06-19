using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FunctionPeriodic : Function<FunctionPeriodic>
{
	public static FunctionPeriodic Create(Action action, float timer, string name = "", bool triggerImmediate = false, bool useUnscaledDeltaTime = false, int? destroyAfterXLoops = null)
	{
		FunctionPeriodic function = Create(name, useUnscaledDeltaTime);
		function.destroyAfterXLoops = destroyAfterXLoops;
		function.timerMax = timer;
		function.timesLooped = 0;
		function.timer = 0;
		function.action = action;
		if (triggerImmediate) action();
		return function;
	}

	public Action action { set; private get; }
	[SerializeField] protected UnityEvent unityAction;
	public int? destroyAfterXLoops;

	private float _timerMax;
	public float timerMax
	{
		get => _timerMax;
		set
		{
			if (value < 0.001f) value = 0.001f;
			_timerMax = value;
		}
	}
	public float timer;
	private int timesLooped;

	protected override void UpdateAction()
	{
		timer += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
		while(timer >= timerMax)
		{
			timesLooped++;
			timer -= timerMax;
			action?.Invoke();
			unityAction?.Invoke();
			if(destroyAfterXLoops != null)
			{
				if(timesLooped >= destroyAfterXLoops)
				{
					Destroy();
					break;
				}
			}
		}
	}
}