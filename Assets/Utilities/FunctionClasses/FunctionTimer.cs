using UnityEngine;
using System;
using UnityEngine.Events;

public class FunctionTimer : Function<FunctionTimer>
{
	public static FunctionTimer Create(Action action, float timer, string name = "", bool useUnscaledDeltaTime = false)
	{
		FunctionTimer function = Create(name, useUnscaledDeltaTime);
		function.timer = timer;
		function.action = action;
		return function;
	}

	public Action action { set; private get; }
	[SerializeField] protected UnityEvent unityAction;
	public float timer;

	protected override void UpdateAction()
	{
		timer -= useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
		if (timer <= 0)
		{
			action?.Invoke();
			unityAction?.Invoke();
			Destroy();
		}
	}
}