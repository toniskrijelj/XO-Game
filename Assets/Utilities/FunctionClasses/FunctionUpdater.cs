using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[ExecuteAlways]
public class FunctionUpdater : Function<FunctionUpdater>
{
	public static FunctionUpdater Create(Action action, string name = "", bool useUnscaledDeltaTime = false)
	{
		FunctionUpdater function = Create(name, useUnscaledDeltaTime);
		function.updateFunc = () => { action(); return false; };
		return function;
	}

	public static FunctionUpdater Create(Func<bool> updateFunc, string name = "", bool useUnscaledDeltaTime = false)
	{
		FunctionUpdater function = Create(name, useUnscaledDeltaTime);
		function.updateFunc = updateFunc;
		return function;
	}

	[SerializeField] protected UnityEvent unityAction;
	private Func<bool> updateFunc;

	protected override void UpdateAction()
	{
		unityAction?.Invoke();
		if(updateFunc())
		{
			Destroy();
		}
	}
}