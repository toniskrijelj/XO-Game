using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Vector3Data : SingleData<Vector3>
{
	public Vector3Data(Vector3 value, float speed) : base(value, speed) { }
}

[Serializable]
public class Vector3Event : UnityEvent<Vector3> { }

public class ObjectMove : FunctionLerp<ObjectMove, Vector3, Vector3Data, Vector3Event>
{
	public static ObjectMove Create(Transform target, Data<Vector3> data, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectMove objectMove = Create(name, useUnscaledDeltaTime);
		objectMove.data = data;
		objectMove.target = target;
		objectMove.loopSetting = loopSettings;
		objectMove.createdThroughCode = true;
		return objectMove;
	}

	public static ObjectMove Create(Transform target, Vector3 startPosition, Vector3 endPosition, float speed, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectMove move = Create(name, useUnscaledDeltaTime);
		move.data = new Data<Vector3>(new SingleData<Vector3>(startPosition, speed), new SingleData<Vector3>(endPosition, speed));
		move.target = target;
		move.loopSetting = loopSettings;
		move.createdThroughCode = true;
		return move;
	}

	public static ObjectMove Create(Vector3 startPosition, Vector3 endPosition, float speed, Action<Vector3> setPosition, LoopSettings loopSettings = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectMove move = Create(name, useUnscaledDeltaTime);
		move.data = new Data<Vector3>(new SingleData<Vector3>(startPosition, speed), new SingleData<Vector3>(endPosition, speed));
		move.setValue = setPosition;
		move.loopSetting = loopSettings;
		move.createdThroughCode = true;
		return move;
	}

	private bool createdThroughCode;

	[SerializeField] protected Transform target;

	protected override Vector3 Lerp(Vector3 a, Vector3 b, float value) => Vector3.Lerp(a, b, value);

	protected override List<Vector3Data> serialized() => inspectorData;
	protected override Vector3Event unitySetValue() => UnitySetValue;

	[SerializeField] protected List<Vector3Data> inspectorData = null;
	[SerializeField] protected Vector3Event UnitySetValue = null;


	protected override void Start()
	{
		if (!createdThroughCode)
		{
			ObjectMove objectMove = Create(target, new Data<Vector3>(serialized().ToArray()), loopSetting, name, useUnscaledDeltaTime);
			target = null;
			Destroy();
			return;
		}
		else if (target != null)
		{
			transform.SetParent(target.parent, false);
			target.SetParent(transform, false);
			setValue = (Vector3 position) => transform.localPosition = position;
		}
		base.Start();
	}

	public override void Destroy()
	{
		if(target != null)
		{
			transform.AttachChildrenToParent();
		}
		base.Destroy();
	}
}