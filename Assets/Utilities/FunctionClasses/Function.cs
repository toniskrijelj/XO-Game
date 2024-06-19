using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class FunctionBase : MonoBehaviour
{
	protected static void InitIfNeeded()
	{
		if (initGameObject == null)
		{
			initGameObject = new GameObject("Function_Global");
			funcList = new List<FunctionBase>();
		}
	}

	public static void DestroyAllFunctions(string name)
	{
		InitIfNeeded();
		for (int i = 0; i < funcList.Count; i++)
		{
			if (funcList[i].FunctionName == name)
			{
				funcList[i].Destroy();
				i--;
			}
		}
	}
	public static bool DoesFuncitonExists(string name)
	{
		InitIfNeeded();
		for (int i = 0; i < funcList.Count; i++)
		{
			if (funcList[i].FunctionName == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool DestroyFunction(string name)
	{
		InitIfNeeded();
		for (int i = 0; i < funcList.Count; i++)
		{
			if (funcList[i].FunctionName == name)
			{
				funcList[i].Destroy();
				return true;
			}
		}
		return false;
	}

	public static FunctionBase Find(string name)
	{
		InitIfNeeded();
		for(int i = 0; i < funcList.Count; i++)
		{
			if(funcList[i].FunctionName == name)
			{
				return funcList[i];
			}
		}
		return null;
	}

	public static List<FunctionBase> GetAllFunctions()
	{
		InitIfNeeded();
		return new List<FunctionBase>(funcList);
	}

	protected static List<FunctionBase> funcList;
	protected static GameObject initGameObject;


	private Action OnUpdate;
	protected GameObject attachedGameObject;

	private void Update()
	{
		OnUpdate?.Invoke();
	}

	public bool useUnscaledDeltaTime;
	[SerializeField] private string functionName = "Function";
	public string FunctionName
	{
		get => functionName;
		protected set => functionName = value.Replace(' ', '_');
	}


	private void Awake()
	{
		InitIfNeeded();
		funcList.Add(this);
		OnUpdate = UpdateAction;
		functionName = functionName.Replace(' ', '_');
	}

	private void OnDestroy()
	{
		funcList.Remove(this);
	}

	public void Pause()
	{
		enabled = false;
	}

	public void Resume()
	{
		enabled = true;
	}

	public bool IsPaused()
	{
		return !enabled;
	}

	public virtual void Destroy()
	{
		if (attachedGameObject != null)
		{
			Destroy(attachedGameObject);
		}
		else
		{
			Destroy(this);
		}
	}

	protected abstract void UpdateAction();
}

public abstract class Function<InstanceType> : FunctionBase where InstanceType : Function<InstanceType>
{
	protected static InstanceType Create(string name, bool useUnscaledDeltaTime)
	{
		InitIfNeeded();
		GameObject gameObject = new GameObject(name + " - " + typeof(InstanceType).ToString(), typeof(InstanceType));
		gameObject.transform.parent = initGameObject.transform;
		InstanceType function = (InstanceType)gameObject.GetComponent(typeof(InstanceType));
		(function as Function<InstanceType>).attachedGameObject = function.gameObject; // MIOZDA FIZXCHASU JSXHHYFDSHSDFahasdfhoasdfhujohj
		function.FunctionName = name;
		function.useUnscaledDeltaTime = useUnscaledDeltaTime;
		return function;
	}
}
