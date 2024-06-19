using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities
{
	private static Camera mainCamera;
	public static Camera MainCamera
	{
		get
		{
			if(mainCamera == null)
			{
				mainCamera = Camera.main;
			}
			return mainCamera;
		}
	}

	private static Canvas mainCanvas;
	public static Canvas MainCanvas
	{
		get
		{
			if (mainCanvas == null)
			{
				mainCanvas = UnityEngine.Object.FindObjectOfType<Canvas>();
			}
			return mainCanvas;
		}
	}

	public static Canvas CreateCanvas(string name)
	{
		Canvas newCanvas = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
		((RectTransform)newCanvas.transform).sizeDelta = new Vector2(1920, 1080);
		newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		CanvasScaler canvasScaler = newCanvas.GetComponent<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(1920, 1080);
		return newCanvas;
	}

	public static Vector3 GetMouseWorldPosition()
	{
		return GetWorldPosition(Input.mousePosition);
	}

	public static Vector3 GetWorldPosition(Vector3 screenPosition, float z = 0)
	{
		Vector3 position = MainCamera.ScreenToWorldPoint(screenPosition);
		position.z = z;
		return position;
	}

	public static float GetAngleFromVector(Vector3 dir)
	{
		float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360;

		return n;
	}

	public static Vector3 GetVectorFromAngle(float angle)
	{
		float angleRad = angle * (Mathf.PI / 180f);
		return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
	}

	public static float SignClamp(float value, float sign, float min, float max)
	{
		return SignClamp(value, sign, min, max, out bool clamped);
	}
	public static float SignClamp(float value, float sign, float min, float max, out bool clamped)
	{
		if (sign > 0)
		{
			if (max <= value)
			{
				clamped = true;
				return max;
			}
			clamped = false;
			return value;
		}
		if (min >= value)
		{
			clamped = true;
			return min;
		}
		clamped = false;
		return value;
	}

	public static Vector3 VectorClamp(Vector3 value, float sign, Vector3 min, Vector3 max)
	{
		value.x = SignClamp(value.x, sign, min.x, max.x);
		value.y = SignClamp(value.y, sign, min.y, max.y);
		value.z = SignClamp(value.z, sign, min.z, max.z);

		return value;
	}
	public static Vector3 VectorClamp(Vector3 value, float sign, Vector3 min, Vector3 max, out bool clampedAll)
	{
		clampedAll = true;
		value.x = SignClamp(value.x, sign, min.x, max.x, out bool clamped);
		if (!clamped) clampedAll = false;
		value.y = SignClamp(value.y, sign, min.y, max.y, out clamped);
		if (!clamped) clampedAll = false;
		value.z = SignClamp(value.z, sign, min.z, max.z, out clamped);
		if (!clamped) clampedAll = false;

		return value;
	}

	public static Vector3 VectorClamp(Vector3 value, Vector3 signs, Vector3 min, Vector3 max)
	{
		value.x = SignClamp(value.x, signs.x, min.x, max.x);
		value.y = SignClamp(value.y, signs.y, min.y, max.y);
		value.z = SignClamp(value.z, signs.z, min.z, max.z);

		return value;
	}
	public static Vector3 VectorClamp(Vector3 value, Vector3 signs, Vector3 min, Vector3 max, out bool clampedAll)
	{
		clampedAll = true;
		value.x = SignClamp(value.x, signs.x, min.x, max.x, out bool clamped);
		if (!clamped) clampedAll = false;
		value.y = SignClamp(value.y, signs.y, min.y, max.y, out clamped);
		if (!clamped) clampedAll = false;
		value.z = SignClamp(value.z, signs.z, min.z, max.z, out clamped);
		if (!clamped) clampedAll = false;

		return value;
	}

	public static float FloatMove(float start, float end, float speed)
	{
		float sign = Mathf.Sign(end - start);
		return SignClamp(start + sign * speed, sign, end, end);
	}
	public static float FloatMove(float start, float endMin, float endMax, float speed)
	{
		return SignClamp(start + speed, speed, endMin, endMax);
	}
	public static float FloatMove(float start, float endMin, float endMax, float speed, out bool clamped)
	{
		return SignClamp(start + speed, speed, endMin, endMax, out clamped);
	}
	public static float FloatMove(float start, float end, float speed, out bool clamped)
	{
		float sign = Mathf.Sign(end - start);
		return SignClamp(start + sign * speed, sign, end, end, out clamped);
	}
	
	public static Vector3 VectorMove(Vector3 start, Vector3 end, float speed)
	{
		return VectorMove(start, end, speed, out bool clamped);
	}
	public static Vector3 VectorMove(Vector3 start, Vector3 end, float speed, out bool clamped)
	{
		clamped = true;
		start.x = FloatMove(start.x, end.x, speed, out bool currentClamped);
		if (!currentClamped) clamped = false;
		start.y = FloatMove(start.y, end.y, speed, out currentClamped);
		if (!currentClamped) clamped = false;
		start.z = FloatMove(start.z, end.z, speed, out currentClamped);
		if (!currentClamped) clamped = false;
		return start;
	}

	public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable<T>
	{
		List<T> objects = new List<T>();
		foreach (Type type in
			Assembly.GetAssembly(typeof(T)).GetTypes()
			.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
		{
			objects.Add((T)Activator.CreateInstance(type, constructorArgs));
		}
		objects.Sort();
		return objects;
	}

	public static void SetAndStretchToParentSize(RectTransform rect, RectTransform parent)
	{
		rect.anchoredPosition = parent.position;
		rect.anchorMin = new Vector2(0, 0);
		rect.anchorMax = new Vector2(1, 1);
		rect.pivot = new Vector2(0.5f, 0.5f);
		rect.sizeDelta = parent.rect.size;
		rect.transform.SetParent(parent);
	}

	public static float HexToDec01(string hex)
	{
		return Convert.ToInt32(hex, 16) / 255f;
	}

	public static int LoopBetween(int value, int min, int max)
	{
		if(min > max)
		{
			Swap(ref min, ref max);
		}
		int diff = max - min + 1;
		while (value < min) value += diff;
		while (value > max) value -= diff;
		return value;
	}

	public static void Swap<T>(ref T first, ref T second)
	{
		T temp = first;
		first = second;
		second = temp;
	}

	public static bool IsBetween(float value, float min, float max)
	{
		return value >= min && value <= max;
	}
}
