using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class FunctionLerp<Type, DataType, SingleDataType, UnityEventType> : Function<Type> 
																						where SingleDataType : SingleData<DataType> 
																						where UnityEventType : UnityEvent<DataType>
																						where Type : FunctionLerp<Type, DataType, SingleDataType, UnityEventType>
{
	public static Type Create(Data<DataType> data, Action<DataType> setValue, Action onComplete = null, LoopSettings loopSetting = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		FunctionLerp<Type, DataType, SingleDataType, UnityEventType> lerp = Create(name, useUnscaledDeltaTime);
		lerp.data = data;
		lerp.setValue = setValue;
		lerp.loopSetting = loopSetting;
		lerp.onComplete = onComplete;
		return (Type)lerp;
	}

	public LoopSettings loopSetting;
	public Action onComplete;
	protected Data<DataType> data = null;
	protected Action<DataType> setValue;

	protected virtual List<SingleDataType> serialized() => null;
	protected virtual UnityEventType unitySetValue() => null;
	private FloatLerp currentLerp;

	protected int startIndex { get; private set; } = 0;
	protected int endIndex { get; private set; } = 1;

	private int increaseSign = 1;

	private void FloatLerp_OnValueChanged(float value)
	{
		CallSetValue(GetData(value));
	}

	protected void CallSetValue(DataType data)
	{
		setValue?.Invoke(data);
		unitySetValue()?.Invoke(data);
	}

	protected abstract DataType Lerp(DataType a, DataType b, float value);

	private DataType GetData(float value)
	{
		return Lerp(data[startIndex].value, data[endIndex].value, value);
	}

	private void FloatLerp_OnComplete()
	{
		startIndex = endIndex;
		if (endIndex + increaseSign < 0 || endIndex + increaseSign >= data.Length)
		{
			if (loopSetting == LoopSettings.PingPong)
			{
				increaseSign *= -1;
			}
			else if (loopSetting == LoopSettings.None)
			{
				onComplete?.Invoke();
				Destroy();
				return;
			}
		}
		endIndex += increaseSign;
		endIndex %= data.Length;
		currentLerp = FloatLerp.Create(0, 1, data[startIndex].Speed, FloatLerp_OnValueChanged, true, FloatLerp_OnComplete, FunctionName + "_ObjectColor", useUnscaledDeltaTime);
	}

	protected sealed override void UpdateAction()
	{
		if (currentLerp != null)
		{
			currentLerp.useUnscaledDeltaTime = useUnscaledDeltaTime;
		}
	}

	protected virtual void Start()
	{
		if(data == null)
		{
			data = new Data<DataType>(serialized().ToArray());
		}
		startIndex = 0;
		if (data.Length >= 2)
		{
			endIndex = 1;
			currentLerp = FloatLerp.Create(0, 1, data[0].Speed, FloatLerp_OnValueChanged, true, FloatLerp_OnComplete, FunctionName + "_ObjectColor", useUnscaledDeltaTime);
			FloatLerp_OnValueChanged(0);
		}
		else
		{
			if (data.Length == 1)
			{
				endIndex = 0;
				CallSetValue(GetData(1));
			}
			Destroy();
		}
	}

	public override void Destroy()
	{
		if (currentLerp != null) currentLerp.Destroy();
		base.Destroy();
	}
}