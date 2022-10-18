using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Vector2Data : SingleData<Vector2>
{
	public Vector2Data(Vector2 value, float speed) : base(value, speed) { }
}

[Serializable]
public class Vector2Event : UnityEvent<Vector2> { }

public class ObjectScale : FunctionLerp<ObjectScale, Vector2, Vector2Data, Vector2Event>
{
	public static ObjectScale Create(Transform target, Data<Vector2> relativeScale, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectScale objectScale = Create(relativeScale, null, null, loopSettings, name, useUnscaledDeltaTime);
		objectScale.target = target;
		objectScale.baseScale = target.localScale;
		return objectScale;
	}

	public static ObjectScale Create(Transform target, Vector2 startScale, Vector2 endScale, float speed, Action onComplete  = null, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectScale objectScale = Create(name, useUnscaledDeltaTime);
		objectScale.onComplete = onComplete;
		objectScale.target = target;
		objectScale.data = new Data<Vector2>(new SingleData<Vector2>(startScale, speed), new SingleData<Vector2>(endScale, speed));
		objectScale.loopSetting = loopSettings;
		objectScale.baseScale = target.localScale;
		return objectScale;
	}

	public static ObjectScale Create(Transform target, float startScale, float endScale, float speed, Action onComplete = null, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		return Create(target, new Vector2(startScale, startScale), new Vector2(endScale, endScale), speed, onComplete, loopSettings, name, useUnscaledDeltaTime);
	}

	private Vector2 baseScale;
	[SerializeField] private Transform target;

	protected override List<Vector2Data> serialized() => inspectorData;

	[SerializeField] private List<Vector2Data> inspectorData = null;

	protected override void Start()
	{
		setValue = (Vector2 scale) => target.localScale = Vector2.Scale(baseScale, scale);
		base.Start();
	}

	protected override Vector2 Lerp(Vector2 a, Vector2 b, float value) => Vector2.Lerp(a, b, value);
}