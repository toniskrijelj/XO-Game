using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public struct ShakeParams
{
	public float magnitude;
	public float duration;

	public ShakeParams(float magnitude, float duration)
	{
		this.magnitude = magnitude;
		this.duration = duration;
	}
}

public class ObjectShake : Function<ObjectShake>
{

	public static ObjectShake Create(Transform target, float magnitude, float duration, string name = "", bool useUnscaledDeltaTime = false, Action onComplete = null)
	{
		ObjectShake objectShake = Create(name, useUnscaledDeltaTime);
		objectShake.shakeParams.Add(new ShakeParams(magnitude, duration));
		objectShake.onComplete = onComplete;
		objectShake.loopSetting = LoopSettings.None;
		objectShake.elapsed = 0;
		objectShake.timeBetweenShakes = Mathf.Min(0.05f, duration / 7f);
		objectShake.nextShakeTime = objectShake.timeBetweenShakes;
		objectShake.target = target;
		objectShake.createdThroughCode = true;
		return objectShake;
	}

	public static ObjectShake Create(Transform target, List<ShakeParams> data, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectShake objectShake = Create(name, useUnscaledDeltaTime);
		objectShake.shakeParams = data;
		objectShake.onComplete = null;
		objectShake.loopSetting = loopSettings;
		objectShake.nextShakeTime = objectShake.timeBetweenShakes;
		objectShake.elapsed = 0;
		objectShake.target = target;
		objectShake.createdThroughCode = true;
		return objectShake;
	}

	private bool createdThroughCode;

	[SerializeField] private Transform target;

	public Action onComplete { set; protected get; }

	[SerializeField] private List<ShakeParams> shakeParams = new List<ShakeParams>();
	public LoopSettings loopSetting;

	private float timeBetweenShakes = 0.05f;

	int currentIndex = 0;
	int increaseSign = 1;

	float elapsed;
	float nextShakeTime;

	private void Start()
	{
		if (shakeParams.Count == 0)
		{
			Destroy();
		}
		else
		{
			if (!createdThroughCode)
			{
				Create(target, shakeParams, loopSetting, name, useUnscaledDeltaTime);
				target = null;
				Destroy();
			}
			else
			{
				transform.SetParent(target.parent, false);
				target.SetParent(transform);
			}
		}
	}

	protected override void UpdateAction()
	{
		if (target != null)
		{
			elapsed += useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
			if (elapsed >= shakeParams[currentIndex].duration)
			{
				elapsed -= shakeParams[currentIndex].duration;
				nextShakeTime = 0;
				if(currentIndex + increaseSign < 0 || currentIndex + increaseSign >= shakeParams.Count)
				{
					if(loopSetting == LoopSettings.Circle)
					{
						increaseSign = 1;
						currentIndex = -1;
					}
					else if(loopSetting == LoopSettings.PingPong)
					{
						increaseSign *= -1;
					}
					else
					{
						Destroy();
					}
				}
				currentIndex += increaseSign;
			}
			else if (elapsed >= nextShakeTime)
			{
				nextShakeTime += timeBetweenShakes;
				transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * shakeParams[currentIndex].magnitude;
			}
		}
		else
		{
			Destroy();
		}
	}

	public override void Destroy()
	{
		if (target != null)
		{
			transform.localPosition = Vector3.zero;
			transform.AttachChildrenToParent();
		}
		base.Destroy();
	}
}