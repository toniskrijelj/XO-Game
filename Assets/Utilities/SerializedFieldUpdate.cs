using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedFieldUpdate<T>
{
	public static SerializedFieldUpdate<T> Create(Func<T> GetFunc, Action<T> SetFunc, string name = "")
	{
		SerializedFieldUpdate<T> serialized = new SerializedFieldUpdate<T>();
		serialized.previousValue = GetFunc();
		serialized.GetFunc = GetFunc;
		serialized.SetFunc = SetFunc;
		serialized.funcUpdater = FunctionUpdater.Create(() =>
		{
			T newValue = serialized.GetFunc();
			if (!serialized.previousValue.Equals(newValue))
			{
				serialized.SetFunc(newValue);
			}
			serialized.previousValue = newValue;
		});
		return serialized;
	}

	FunctionUpdater funcUpdater;

	T previousValue;
	Func<T> GetFunc;
	Action<T> SetFunc;

	public void Destroy()
	{
		funcUpdater.Destroy();
	}
}
