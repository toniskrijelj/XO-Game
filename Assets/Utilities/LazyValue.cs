using System;

public class LazyValue<T>
{
	private T _value;
	private bool initialized = false;
	private readonly Func<T> initializer;

	private LazyValue(T value)
	{
		this.value = value;
	}

	public LazyValue(Func<T> initializer)
	{
		this.initializer = initializer;
	}

	public T value
	{
		get
		{
			Initialize();
			return _value;
		}
		set
		{
			initialized = true;
			_value = value;
		}
	}

	public void Initialize()
	{
		if (!initialized)
		{
			_value = initializer();
			initialized = true;
		}
	}

	public static implicit operator LazyValue<T>(T value)
	{
		return new LazyValue<T>(value);
	}

	public static implicit operator T(LazyValue<T> instance)
	{
		return instance.value;
	}

}