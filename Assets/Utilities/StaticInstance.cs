using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : StaticInstance<T>
{
	private static bool first = true;
	protected static T instance;
	public static T Instance
	{
		get
		{
			if(instance == null && first)
			{
				instance = FindObjectOfType<T>();
				first = false;
			}
			return instance;
		}
		private set => instance = value;
	}

	protected virtual void Awake()
	{
		instance = (T)this;
	}
}
