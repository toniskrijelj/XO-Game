using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct SingleLoopData
{
	public Action action { private get; set; }
	[SerializeField] private UnityEvent unityAction;
	public float timeBeforeNext;

	public SingleLoopData(Action action, float timeBeforeNext)
	{
		this.action = action;
		this.timeBeforeNext = timeBeforeNext;
		unityAction = null;
	}

	public void Action()
	{
		action?.Invoke();
		unityAction?.Invoke();
	}
}

[Serializable]
public struct LoopData
{
	[SerializeField] private SingleLoopData[] data;

	public LoopData(params SingleLoopData[] data)
	{
		this.data = data;
	}

	public int Length => data.Length;

	public SingleLoopData this[int i]
	{
		get => data[i];
	}
}
public class FunctionLooper : Function<FunctionLooper>
{
	public static FunctionLooper Create(LoopData data, string name = "", bool useUnscaledDeltaTime = false)
	{
		FunctionLooper function = Create(name, useUnscaledDeltaTime);
		function.loopData = data;
		function.currentIndex = 0;
		function.nextCallTime = Time.time;
		function.currentTime = Time.time;
		return function;
	}

	[SerializeField] LoopData loopData;
	int currentIndex;
	float currentTime;
	float nextCallTime;

	protected override void UpdateAction()
	{
		currentTime += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
		while (currentTime >= nextCallTime)
		{
			nextCallTime += loopData[currentIndex].timeBeforeNext;
			loopData[currentIndex].Action();
			currentIndex = (currentIndex + 1) % loopData.Length;
		}
	}
}