using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatData : SingleData<float>
{
	public FloatData(float value, float speed) : base(value, speed) { }
}

[Serializable]
public class FloatEvent : UnityEvent<float> { }


public class ObjectRotate : FunctionLerp<ObjectRotate, float, FloatData, FloatEvent>
{
	public static ObjectRotate Create(Transform target, Data<float> relativeRotation, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectRotate objectRotate = Create(relativeRotation, null, null, loopSettings, name, useUnscaledDeltaTime);
		objectRotate.target = target;
		return objectRotate;
	}

	public static ObjectRotate Create(Transform target, float startRotation, float endRotation, float speed, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectRotate objectRotate = Create(name, useUnscaledDeltaTime);
		objectRotate.target = target;
		objectRotate.data = new Data<float>(new SingleData<float>(startRotation, speed), new SingleData<float>(endRotation, speed));
		objectRotate.loopSetting = loopSettings;
		return objectRotate;
	}

	[SerializeField] private Transform target;

	protected override List<FloatData> serialized() => inspectorData;

	[SerializeField] private List<FloatData> inspectorData = null;

	protected override void Start()
	{
		setValue = (float rotation) => target.localEulerAngles = new Vector3(0, 0, rotation);
		base.Start();
	}

	protected override float Lerp(float a, float b, float value) => Mathf.Lerp(a, b, value);
}