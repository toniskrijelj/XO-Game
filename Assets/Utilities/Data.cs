using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoopSettings
{
	None,
	PingPong,
	Circle
}


[Serializable]
public class SingleData<T>
{
	[SerializeField] private float speed;
	[SerializeField] public T value;

	public float Speed
	{
		get => speed;
		set
		{
			speed = Mathf.Abs(value);
			speed = Mathf.Max(speed, 0.001f);
		}
	}

	public SingleData(T value, float speed)
	{
		this.value = value;
		Speed = speed;
	}

	public static SingleData<T> Create(T value, float speed)
	{
		return new SingleData<T>(value, speed);
	}
}

public class Data<T>
{
	[SerializeField] private SingleData<T>[] data;

	public Data(params SingleData<T>[] data)
	{
		this.data = data;
	}

	public Data(List<SingleData<T>> data)
	{
		this.data = data.ToArray();
	}

	public int Length => data.Length;
	public int Count => data.Length;

	public SingleData<T> this[int i]
	{
		get => data[i];
	}

	public List<SingleData<T>> ToList()
	{
		return new List<SingleData<T>>(data);
	}
}