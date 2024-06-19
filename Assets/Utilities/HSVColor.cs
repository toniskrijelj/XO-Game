using UnityEngine;

[System.Serializable]
public struct HSVColor
{
	private float h;
	private float s;
	private float v;
	private float a;

	public float H
	{
		get => h;
		set => h = Mathf.Clamp01(value);
	}
	public float S
	{
		get => s;
		set => s = Mathf.Clamp01(value);
	}
	public float V
	{
		get => v;
		set => v = Mathf.Clamp(value, 0.01f, 1f);
	}
	public float A
	{
		get => a;
		set => a = Mathf.Clamp01(value);
	}

	public HSVColor(float h, float s, float v, float a)
	{
		h = Mathf.Clamp01(h);
		s = Mathf.Clamp01(s);
		v = Mathf.Clamp01(v);
		a = Mathf.Clamp01(a);
		this.h = h;
		this.s = s;
		this.v = v;
		this.a = a;
	}

	public HSVColor(float h, float s, float v)
	{
		h = Mathf.Clamp01(h);
		s = Mathf.Clamp01(s);
		v = Mathf.Clamp01(v);
		this.h = h;
		this.s = s;
		this.v = v;
		a = 1f;
	}

	public HSVColor(Color col)
	{
		HSVColor temp = FromColor(col);
		h = temp.h;
		s = temp.S;
		v = temp.V;
		a = temp.A;
	}

	public static HSVColor FromColor(Color color)
	{
		HSVColor ret = new HSVColor(0f, 0f, 0f, color.a);

		float r = color.r;
		float g = color.g;
		float b = color.b;

		float max = Mathf.Max(r, Mathf.Max(g, b));

		if (max <= 0)
		{
			return ret;
		}

		float min = Mathf.Min(r, Mathf.Min(g, b));
		float dif = max - min;

		if (max > min)
		{
			if (g == max)
			{
				ret.h = (b - r) / dif * 60f + 120f;
			}
			else if (b == max)
			{
				ret.h = (r - g) / dif * 60f + 240f;
			}
			else if (b > g)
			{
				ret.h = (g - b) / dif * 60f + 360f;
			}
			else
			{
				ret.h = (g - b) / dif * 60f;
			}
			if (ret.h < 0)
			{
				ret.h = ret.h + 360f;
			}
		}
		else
		{
			ret.h = 0;
		}

		ret.h *= 1f / 360f;
		ret.s = (dif / max) * 1f;
		ret.v = max;

		return ret;
	}

	public static Color ToColor(HSVColor hsvColor)
	{
		float r = hsvColor.V;
		float g = hsvColor.V;
		float b = hsvColor.V;
		if (hsvColor.S != 0)
		{
			float max = hsvColor.V;
			float dif = hsvColor.V * hsvColor.S;
			float min = hsvColor.V - dif;

			float h = hsvColor.H * 360f;

			if (h < 60f)
			{
				r = max;
				g = h * dif / 60f + min;
				b = min;
			}
			else if (h < 120f)
			{
				r = -(h - 120f) * dif / 60f + min;
				g = max;
				b = min;
			}
			else if (h < 180f)
			{
				r = min;
				g = max;
				b = (h - 120f) * dif / 60f + min;
			}
			else if (h < 240f)
			{
				r = min;
				g = -(h - 240f) * dif / 60f + min;
				b = max;
			}
			else if (h < 300f)
			{
				r = (h - 240f) * dif / 60f + min;
				g = min;
				b = max;
			}
			else if (h <= 360f)
			{
				r = max;
				g = min;
				b = -(h - 360f) * dif / 60 + min;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}

		return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsvColor.A);
	}

	public Color ToColor()
	{
		return ToColor(this);
	}

	public override string ToString()
	{
		return "H:" + H + " S:" + S + " B:" + V;
	}

	public static HSVColor Lerp(HSVColor a, HSVColor b, float t)
	{
		float h, s;

		//check special case black (color.b==0): interpolate neither hue nor saturation!
		//check special case grey (color.s==0): don't interpolate hue!
		if (a.v == 0)
		{
			h = b.h;
			s = b.s;
		}
		else if (b.V == 0)
		{
			h = a.h;
			s = a.s;
		}
		else
		{
			if (a.s == 0)
			{
				h = b.h;
			}
			else if (b.S == 0)
			{
				h = a.h;
			}
			else
			{
				// works around bug with LerpAngle
				float angle = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t);
				while (angle < 0f)
					angle += 360f;
				while (angle > 360f)
					angle -= 360f;
				h = angle / 360f;
			}
			s = Mathf.Lerp(a.s, b.s, t);
		}
		return new HSVColor(h, s, Mathf.Lerp(a.v, b.v, t), Mathf.Lerp(a.a, b.a, t));
	}

	public static implicit operator Color(HSVColor color) => color.ToColor();
	public static implicit operator HSVColor(Color color) => color.ToHSVColor();
}