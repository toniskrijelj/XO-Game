using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyCodeUtilities
{
	private static Dictionary<KeyCode, string> stringValues;
	private static Dictionary<KeyCode, KeyCode> baseKeys;

	private static bool init = false;

	private static void Init()
	{
		if (!init)
		{
			stringValues = new Dictionary<KeyCode, string>();
			baseKeys = new Dictionary<KeyCode, KeyCode>();
			for (int i = 0; i < 10; i++)
			{
				stringValues.Add(KeyCode.Alpha0 + i, i.ToString());
				baseKeys.Add(KeyCode.Alpha0 + i, KeyCode.None);
			}
			stringValues.Add(KeyCode.Semicolon, ";");
			baseKeys.Add(KeyCode.Semicolon, KeyCode.None);
			stringValues.Add(KeyCode.Colon, ":");
			baseKeys.Add(KeyCode.Colon, KeyCode.Semicolon);
			stringValues.Add(KeyCode.Exclaim, "!");
			baseKeys.Add(KeyCode.Exclaim, KeyCode.Alpha1);
			stringValues.Add(KeyCode.At, "@");
			baseKeys.Add(KeyCode.At, KeyCode.Alpha2);
			stringValues.Add(KeyCode.Hash, "#");
			baseKeys.Add(KeyCode.Hash, KeyCode.Alpha3);
			stringValues.Add(KeyCode.Dollar, "$");
			baseKeys.Add(KeyCode.Dollar, KeyCode.Alpha4);
			stringValues.Add(KeyCode.Percent, "%");
			baseKeys.Add(KeyCode.Percent, KeyCode.Alpha5);
			stringValues.Add(KeyCode.Caret, "^");
			baseKeys.Add(KeyCode.Caret, KeyCode.Alpha6);
			stringValues.Add(KeyCode.Ampersand, "&");
			baseKeys.Add(KeyCode.Ampersand, KeyCode.Alpha7);
			stringValues.Add(KeyCode.Asterisk, "*");
			baseKeys.Add(KeyCode.Asterisk, KeyCode.Alpha8);
			stringValues.Add(KeyCode.LeftParen, "(");
			baseKeys.Add(KeyCode.LeftParen, KeyCode.Alpha9);
			stringValues.Add(KeyCode.RightParen, ")");
			baseKeys.Add(KeyCode.RightParen, KeyCode.Alpha0);
			stringValues.Add(KeyCode.Minus, "-");
			baseKeys.Add(KeyCode.Minus, KeyCode.None);
			stringValues.Add(KeyCode.Underscore, "_");
			baseKeys.Add(KeyCode.Underscore, KeyCode.Minus);
			stringValues.Add(KeyCode.Equals, "=");
			baseKeys.Add(KeyCode.Equals, KeyCode.None);
			stringValues.Add(KeyCode.Plus, "+");
			baseKeys.Add(KeyCode.Plus, KeyCode.Equals);
			stringValues.Add(KeyCode.BackQuote, "`");
			baseKeys.Add(KeyCode.BackQuote, KeyCode.None);
			stringValues.Add(KeyCode.Tilde, "~");
			baseKeys.Add(KeyCode.Tilde, KeyCode.BackQuote);
			stringValues.Add(KeyCode.LeftBracket, "[");
			baseKeys.Add(KeyCode.LeftBracket, KeyCode.None);
			stringValues.Add(KeyCode.RightBracket, "]");
			baseKeys.Add(KeyCode.RightBracket, KeyCode.None);
			stringValues.Add(KeyCode.LeftCurlyBracket, "{");
			baseKeys.Add(KeyCode.LeftCurlyBracket, KeyCode.LeftBracket);
			stringValues.Add(KeyCode.RightCurlyBracket, "}");
			baseKeys.Add(KeyCode.RightCurlyBracket, KeyCode.RightBracket);
			stringValues.Add(KeyCode.LeftAlt, "Alt");
			stringValues.Add(KeyCode.LeftControl, "Ctrl");
			stringValues.Add(KeyCode.LeftShift, "Shift");
			stringValues.Add(KeyCode.Backslash, "\\");
			baseKeys.Add(KeyCode.Backslash, KeyCode.None);
			stringValues.Add(KeyCode.Question, "?");
			baseKeys.Add(KeyCode.Question, KeyCode.Slash);
			stringValues.Add(KeyCode.Comma, ",");
			baseKeys.Add(KeyCode.Comma, KeyCode.None);
			stringValues.Add(KeyCode.Period, ".");
			baseKeys.Add(KeyCode.Period, KeyCode.None);
			stringValues.Add(KeyCode.Less, "<");
			baseKeys.Add(KeyCode.Less, KeyCode.Comma);
			stringValues.Add(KeyCode.Greater, ">");
			baseKeys.Add(KeyCode.Greater, KeyCode.Period);
			stringValues.Add(KeyCode.Slash, "/");
			baseKeys.Add(KeyCode.Slash, KeyCode.None);
			stringValues.Add(KeyCode.Pipe, "|");
			baseKeys.Add(KeyCode.Pipe, KeyCode.Backslash);
			stringValues.Add(KeyCode.Quote, "'");
			baseKeys.Add(KeyCode.Quote, KeyCode.None);
			stringValues.Add(KeyCode.DoubleQuote, @"""");
			baseKeys.Add(KeyCode.DoubleQuote, KeyCode.Quote);
			stringValues.Add(KeyCode.Return, "Enter");

			init = true;
		}
	}

	public static string ToString(KeyCode keyCode)
	{
		Init();
		if(stringValues.TryGetValue(keyCode, out string value))
		{
			return value;
		}
		return keyCode.ToString();
	}

	public static bool GetKeyDown(KeyCode keyCode)
	{
		Init();
		if(keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
		{
			return (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
				|| Input.GetKeyDown(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift);
		}
		if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
		{
			return (Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
				|| Input.GetKeyDown(KeyCode.RightControl) && !Input.GetKey(KeyCode.LeftControl);
		}
		if (keyCode == KeyCode.LeftAlt || keyCode == KeyCode.RightAlt)
		{
			return (Input.GetKeyDown(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
				|| Input.GetKeyDown(KeyCode.RightAlt) && !Input.GetKey(KeyCode.LeftAlt);
		}

		if (baseKeys.TryGetValue(keyCode, out KeyCode baseValue))
		{
			if(baseValue == KeyCode.None)
			{
				if (Input.GetKeyDown(keyCode) && !GetKey(KeyCode.LeftShift)) return true;
				if (GetKeyUp(KeyCode.LeftShift) && Input.GetKey(keyCode)) return true;
				return false;
			}
			else
			{
				if (Input.GetKeyDown(baseValue) && GetKey(KeyCode.LeftShift)) return true;
				if (GetKeyDown(KeyCode.LeftShift) && Input.GetKey(baseValue)) return true;
				return false;
			}
		}
		return Input.GetKeyDown(keyCode);
	}

	public static bool GetKey(KeyCode keyCode)
	{
		Init();
		if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
		{
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}
		if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
		{
			return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		}
		if (keyCode == KeyCode.LeftAlt || keyCode == KeyCode.RightAlt)
		{
			return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		}

		if (baseKeys.TryGetValue(keyCode, out KeyCode baseValue))
		{
			if (baseValue == KeyCode.None)
			{
				return Input.GetKey(keyCode) && !GetKey(KeyCode.LeftShift);
			}
			else
			{
				return Input.GetKey(baseValue) && GetKey(KeyCode.LeftShift);
			}
		}
		return Input.GetKey(keyCode);
	}

	public static bool GetKeyUp(KeyCode keyCode)
	{
		Init();
		if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
		{
			return (Input.GetKeyUp(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
				|| Input.GetKeyUp(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift);
		}
		if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
		{
			return (Input.GetKeyUp(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
				|| Input.GetKeyUp(KeyCode.RightControl) && !Input.GetKey(KeyCode.LeftControl);
		}
		if (keyCode == KeyCode.LeftAlt || keyCode == KeyCode.RightAlt)
		{
			return (Input.GetKeyUp(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
				|| Input.GetKeyUp(KeyCode.RightAlt) && !Input.GetKey(KeyCode.LeftAlt);
		}

		if (baseKeys.TryGetValue(keyCode, out KeyCode baseValue))
		{
			if (baseValue == KeyCode.None)
			{
				if (Input.GetKeyUp(keyCode) && !GetKey(KeyCode.LeftShift)) return true;
				if (GetKeyDown(KeyCode.LeftShift) && Input.GetKey(keyCode)) return true;
				return false;
			}
			else
			{
				if (Input.GetKeyUp(baseValue) && GetKey(KeyCode.LeftShift)) return true;
				if (GetKeyUp(KeyCode.LeftShift) && Input.GetKey(baseValue)) return true;
				return false;
			}
		}
		return Input.GetKeyUp(keyCode);
	}
}
