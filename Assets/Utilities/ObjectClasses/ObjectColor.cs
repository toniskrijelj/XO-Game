using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ColorData : SingleData<Color>
{
	public ColorData(Color value, float speed) : base(value, speed) { }
}

[Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ObjectColor : FunctionLerp<ObjectColor, Color, ColorData, ColorEvent>
{
	protected override Color Lerp(Color a, Color b, float value)
	{
		if (useHSVLerp)
			return HSVColor.Lerp(a, b, value);
		else
			return Color.Lerp(a, b, value);
	}

	protected override List<ColorData> serialized() => inspectorData;
	protected override ColorEvent unitySetValue() => UnitySetValue;

	[SerializeField] private List<ColorData> inspectorData = null;
	[SerializeField] private ColorEvent UnitySetValue = null;

	public static ObjectColor Create(Color startColor, Color endColor, float speed, Action<Color> setColor, LoopSettings loopSetting = LoopSettings.None, string name = "", bool useUnscaledDeltaTime = false)
	{
		ObjectColor lerp = Create(name, useUnscaledDeltaTime);
		SingleData<Color> param1 = new SingleData<Color>(startColor, speed);
		SingleData<Color> param2 = new SingleData<Color>(endColor, speed);
		lerp.data = new Data<Color>(param1, param2);
		lerp.setValue = setColor;
		lerp.loopSetting = loopSetting;
		return lerp;
	}

	public static ObjectColor CreateRainbow(float alpha, float speed, Action<Color> setColor, LoopSettings loopSetting = LoopSettings.Circle, string name = "", bool useUnscaledDeltaTime = false)
	{
		alpha = Mathf.Clamp01(alpha);
		ObjectColor color = Create(name, useUnscaledDeltaTime);
		List<SingleData<Color>> data = new List<SingleData<Color>>();
		for(int i = 0; i < 12; i++)
		{
			data.Add(new SingleData<Color>(new HSVColor(30 * i / 360f, 1, 1, alpha), speed));
		}
		color.data = new Data<Color>(data);
		color.setValue = setColor;
		color.loopSetting = loopSetting;
		return color;
	}

	public bool useHSVLerp = false;

	public void ChangeColorV(float v)
	{
		for (int i = 0; i < data.Count; i++)
		{
			HSVColor color = data[i].value.ToHSVColor();
			color.V = v;
			data[i].value = color;
		}
	}

	public void ChangeColorS(float s)
	{
		for (int i = 0; i < data.Count; i++)
		{
			HSVColor color = data[i].value.ToHSVColor();
			color.S = s;
			data[i].value = color;
		}
	}

	public void ChangeColorAlpha(float a)
	{
		for (int i = 0; i < data.Count; i++)
		{
			HSVColor color = data[i].value;
			color.A = a;
			data[i].value = color;
		}
	}

}