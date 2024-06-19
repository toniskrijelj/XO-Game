using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
	private static Canvas canvas = null;

	private static int numberOfAutofillButtons = 0;

	private static void CreateCanvasIfNeeded()
	{
		if (canvas == null)
		{
			canvas = Utilities.CreateCanvas("Canvas_ButtonUI");
		}
	}

	public static ButtonUI Create(Action onPress, string buttonText, Color? fillColor, Color? textColor, Vector2? anchoredPosition, Vector2? sizeDelta, params KeyCode[] buttons)
	{
		CreateCanvasIfNeeded();
		if (fillColor == null) fillColor = ColorUtilities.white;
		if (textColor == null) textColor = ColorUtilities.black;
		if (anchoredPosition == null)
		{
			anchoredPosition = new Vector2(-780 + 312 * (numberOfAutofillButtons % 6), 450 - 900 * (numberOfAutofillButtons / 6));
			numberOfAutofillButtons++;
		}
		if(sizeDelta == null)
		{
			sizeDelta = new Vector2(300, 100);
		}
		GameObject buttonGameObject = new GameObject("Button", typeof(RectTransform));
		buttonGameObject.transform.SetParent(canvas.transform);
		ButtonUI buttonUI = buttonGameObject.AddComponent<ButtonUI>();
		buttonGameObject.AddComponent<Text>();
		buttonUI.rectTransform.anchoredPosition = (Vector2) anchoredPosition;
		buttonUI.rectTransform.sizeDelta = (Vector2)sizeDelta;
		GameObject background = new GameObject("Background", typeof(RectTransform));
		Utilities.SetAndStretchToParentSize((RectTransform)background.transform, (RectTransform)buttonGameObject.transform);
		buttonUI.background = background.AddComponent<Image>();
		HSVColor hsvColor = (Color)fillColor;
		buttonUI.background.rectTransform.offsetMin = new Vector2(0, -((Vector2)sizeDelta).y / 6);
		GameObject fill = new GameObject("Fill", typeof(RectTransform));
		Utilities.SetAndStretchToParentSize((RectTransform)fill.transform, (RectTransform)buttonGameObject.transform);
		buttonUI.fill = fill.AddComponent<Image>();
		buttonUI.fill.color = (Color)fillColor;
		GameObject textGameObject = new GameObject("Text", typeof(RectTransform));
		Utilities.SetAndStretchToParentSize((RectTransform)textGameObject.transform, (RectTransform)buttonGameObject.transform);
		buttonUI.text = textGameObject.AddComponent<TextMeshProUGUI>();
		buttonUI.OriginalColor = (Color)fillColor;
		buttonUI.text.color = (Color)fillColor;
		hsvColor.V -= 0.2f;
		buttonUI.background.color = hsvColor;
		buttonUI.HoverColor = hsvColor;
		hsvColor.V -= 0.2f;
		buttonUI.PressColor = hsvColor;
		buttonUI.text.enableAutoSizing = true;
		buttonUI.text.fontSizeMin = 0;
		buttonUI.text.fontSizeMax = 500;
		buttonUI.text.characterWidthAdjustment = .5f;
		buttonUI.text.rectTransform.offsetMin = Vector2.zero;
		buttonUI.text.rectTransform.offsetMax = Vector2.zero;
		buttonUI.text.text = buttonText;
		buttonUI.text.color = (Color)textColor;
		buttonUI.TextColor = (Color)textColor;
		buttonUI.TextHoverColor = (Color)textColor;
		HSVColor textHsvColor = (Color)textColor;
		textHsvColor.S += 1;
		textHsvColor.V += .2f;
		buttonUI.TextPressColor = textHsvColor;
		buttonUI.text.richText = true;
		buttonUI.text.font = GameAssets.i.robotoMedium;
		buttonUI.text.alignment = TextAlignmentOptions.Center;
		buttonUI.text.margin = new Vector4(((Vector2)sizeDelta).y * 0.25f, ((Vector2)sizeDelta).y * 0.25f, ((Vector2)sizeDelta).y * 0.25f, ((Vector2)sizeDelta).y * 0.25f);
		buttonUI.originalAnchoredPosition = (Vector2)anchoredPosition;
		buttonUI.madeFromCode = true;
		buttonUI.buttonsText = Instantiate(buttonUI.text);
		Utilities.SetAndStretchToParentSize(buttonUI.buttonsText.rectTransform, buttonUI.rectTransform);
		string text = "";
		if (buttons.Length != 0)
		{
			for (int i = 0; i < buttons.Length - 1; i++)
			{
				text += ColorUtilities.SetTextColor(buttons[i].ConvertToString(), ColorUtilities.black) + " + ";
			}
			text += ColorUtilities.SetTextColor(buttons[buttons.Length - 1].ConvertToString(), ColorUtilities.black);
		}
		buttonUI.buttonsText.text = text;
		buttonUI.buttonsText.color = ColorUtilities.black;
		buttonUI.onUp = onPress;
		buttonUI.UpdateKeys(buttons);
		if (buttons.Length != 0)
		{
			buttonUI.text.margin = new Vector4(buttonUI.rectTransform.sizeDelta.y * .25f, buttonUI.rectTransform.sizeDelta.y * .125f, buttonUI.rectTransform.sizeDelta.y * .25f, buttonUI.rectTransform.sizeDelta.y * .4f);
		}
		else
		{
			buttonUI.text.margin = new Vector4(buttonUI.rectTransform.sizeDelta.y * .25f, buttonUI.rectTransform.sizeDelta.y * .25f, buttonUI.rectTransform.sizeDelta.y * .25f, buttonUI.rectTransform.sizeDelta.y * .25f);
		}
		buttonUI.buttonsText.margin = new Vector4(buttonUI.rectTransform.sizeDelta.y * .3f, buttonUI.rectTransform.sizeDelta.y * .6f, buttonUI.rectTransform.sizeDelta.y * .3f, buttonUI.rectTransform.sizeDelta.y * .0625f);
		buttonUI.buttonsText.font = GameAssets.i.robotoMedium;
		buttonUI.CorrectTextColor = ColorUtilities.green;
		buttonUI.NonCorrectTextColor = ColorUtilities.black;
		return buttonUI;
	}

	public static ButtonUI CreateLocked(Action onPress, float lockedDuration, string buttonText, Color? fillColor, Color? textColor, Vector2? anchoredPosition, Vector2? sizeDelta, params KeyCode[] buttons)
	{
		ButtonUI button = Create(onPress, buttonText, fillColor, textColor, anchoredPosition, sizeDelta);
		button.lockOnClick = true;
		button.lockOnClickDuration = lockedDuration;
		return button;
	}

	public static ButtonUI CreateRandom(Action onPress, string buttonText, Vector2? anchoredPosition, Vector2? sizeDelta, params KeyCode[] buttons)
	{
		ButtonUI button = Create(onPress, buttonText, ColorUtilities.RandomColor2(), ColorUtilities.RandomColor2(), anchoredPosition, sizeDelta, buttons);
		button.CorrectTextColor = ColorUtilities.RandomColor2();
		button.NonCorrectTextColor = ColorUtilities.RandomColor2();
		return button;
	}

	public static ButtonUI CreateMouseButton(Action onPress, string buttonText, Color? fillColor = null, Color? textColor = null, Vector2? anchoredPosition = null, Vector2? sizeDelta = null)
	{
		ButtonUI button = Create(onPress, buttonText, fillColor, textColor, anchoredPosition, sizeDelta);
		button.allowKeys = false;
		return button;
	}

	public static ButtonUI CreateMouseLocked(Action onPress, float lockedDuration, string buttonText, Color? fillColor = null, Color? textColor = null, Vector2? anchoredPosition = null, Vector2? sizeDelta = null)
	{
		ButtonUI button = Create(onPress, buttonText, fillColor, textColor, anchoredPosition, sizeDelta);
		button.lockOnClickDuration = lockedDuration;
		button.lockOnClick = true;
		button.allowKeys = false;
		return button;
	}

	public static ButtonUI CreateKeysButton(Action onPress, string buttonText, Color? fillColor, Color? textColor, Vector2? anchoredPosition, Vector2? sizeDelta, params KeyCode[] buttons)
	{
		ButtonUI button = Create(onPress, buttonText, fillColor, textColor, anchoredPosition, sizeDelta, buttons);
		button.allowMouse = false;
		return button;
	}

	public static ButtonUI CreateKeysLocked(Action onPress, float lockedDuration, string buttonText, Color? fillColor, Color? textColor, Vector2? anchoredPosition, Vector2? sizeDelta, params KeyCode[] buttons)
	{
		ButtonUI button = Create(onPress, buttonText, fillColor, textColor, anchoredPosition, sizeDelta, buttons);
		button.allowMouse = false;
		button.lockOnClick = true;
		button.lockOnClickDuration = lockedDuration;
		return button;
	}

	[Header("Cached components")]
	[SerializeField] TextMeshProUGUI text = null;
	[SerializeField] Image background = null;
	[SerializeField] Image fill = null;
	[SerializeField] TextMeshProUGUI buttonsText;

	[Header("Events")]
	[SerializeField] UnityEvent onDownUnity = null;
	[SerializeField] UnityEvent onUpUnity = null;
	[SerializeField] UnityEvent onEnterUnity = null;
	[SerializeField] UnityEvent onExitUnity = null;

	[Header("Colors")]
	[SerializeField] Color originalColor = ColorUtilities.white;
	public Color OriginalColor
	{
		get => originalColor;
		set
		{
			if(!holding && !onButton)
			{
				SetColor(value);
			}
			originalColor = value;
		}
	}

	[SerializeField] Color hoverColor = ColorUtilities.lightGray;
	public Color HoverColor
	{
		get => hoverColor;
		set
		{
			if(!holding && onButton)
			{
				SetColor(value);
			}
			hoverColor = value;
		}
	}

	[SerializeField] Color pressColor = ColorUtilities.gray;
	public Color PressColor
	{
		get => pressColor;
		set
		{
			if(holding)
			{
				SetColor(value);
			}
			pressColor = value;
		}
	}

	[SerializeField] Color textColor = ColorUtilities.black;
	public Color TextColor
	{
		get => textColor;
		set
		{
			if(!holding)
			{
				SetTextColor(value);
			}
			textColor = value;
		}
	}

	[SerializeField] Color textHoverColor = ColorUtilities.black;
	public Color TextHoverColor
	{
		get => textHoverColor;
		set
		{
			if (!holding && onButton)
			{
				SetTextColor(value);
			}
			textHoverColor = value;
		}
	}

	[SerializeField] Color textPressColor = ColorUtilities.black;
	public Color TextPressColor
	{
		get => textPressColor;
		set
		{
			if(holding)
			{
				SetTextColor(value);
			}
			textPressColor = value;
		}
	}

	[SerializeField] Color correctTextColor = ColorUtilities.green;
	public Color CorrectTextColor
	{
		get => correctTextColor;
		set
		{
			correctTextColor = value;
			UpdateText();
		}
	}

	[SerializeField] Color nonCorrectTextColor = ColorUtilities.black;
	public Color NonCorrectTextColor
	{
		get => nonCorrectTextColor;
		set
		{
			nonCorrectTextColor = value;
			UpdateText();
		}
	}

	[Header("Keys")]
	[SerializeField] List<KeyCode> keys = null;
	private ObjectColor[] keyFades = null;
	private int keyIndex = 0;
	public bool resetOnKeyUp = true;

	private Vector2 originalAnchoredPosition;

	public bool allowMouse = true;
	public bool allowKeys = true;

	private bool holdingMouse;
	private bool hoverMouse;
	private bool holdingKey;
	private bool hoverKey;

	public Action onDown { private get; set; } = null;
	public Action onUp { private get; set; } = null;
	public Action onEnter { private get; set; } = null;
	public Action onExit { private get; set; } = null;

	public bool lockOnClick = false;
	public float lockOnClickDuration = 0;
	private bool buttonLocked = false;

	private ObjectColor fading;
	private ObjectColor textFading;

	private RectTransform rectTransform;

	private bool onButton => hoverMouse || hoverKey;
	private bool holding => holdingKey || holdingMouse;

	private bool madeFromCode = false;

	private void OnDown()
	{
		if (buttonLocked) return;
		onDown?.Invoke();
		onDownUnity?.Invoke();
		MoveButton(0.8f);
		SetColor(PressColor);
		SetTextColor(TextPressColor);
	}

	private void OnUp()
	{
		if (buttonLocked) return;
		onUp?.Invoke();
		onUpUnity?.Invoke();
		if (lockOnClick)
		{
			Lock(lockOnClickDuration);
		}
		else
		{
			SetTextColor(TextColor);
			MoveButton(0.1f);
			SetColor(HoverColor);
			SetTextColor(TextHoverColor);
		}
	}

	private void OnExit()
	{
		if (buttonLocked) return;
		onExit?.Invoke();
		onExitUnity?.Invoke();
		MoveButton(0);
		SetColor(OriginalColor);
		SetTextColor(TextColor);
	}

	private void OnEnter()
	{
		if (buttonLocked) return;
		onEnter?.Invoke();
		onEnterUnity?.Invoke();
		MoveButton(0.1f);
		SetColor(HoverColor);
		SetTextColor(TextHoverColor);
	}
	bool start = false;
	[SerializeField] private float textColorSpeed = 10; 
	private void SetColor(Color color)
	{
		if (color == fill.color) return;
		if(fading != null)
		{
			fading.Destroy();
		}
		if (Application.isPlaying)
		{
			if (start)
			{
				fading = ObjectColor.Create(fill.color, color, textColorSpeed, (Color col) =>
				{
					fill.color = col;
					HSVColor hsvColor = col;
					hsvColor.V -= .2f;
					background.color = hsvColor;
				});
			}
			else
			{
				fill.color = color;
				HSVColor hsvColor = color;
				hsvColor.V -= .2f;
				background.color = hsvColor;
				start = true;
			}
		}
		else
		{
			fill.color = color;
			HSVColor hsvColor = color;
			hsvColor.V -= .2f;
			background.color = hsvColor;
		}
	}

	public void SetColorToEverything(Color color)
	{
		HSVColor col = color;
		OriginalColor = col;
		col.H -= 0.15f;
		HoverColor = col;
		col.H -= 0.15f;
		PressColor = col;
	}

	private void SetTextColor(Color color)
	{
		if (text == null) return;
		if (color == text.color) return;
		if (textFading != null)
		{
			textFading.Destroy();
		}
		if (Application.isPlaying)
		{
			textFading = ObjectColor.Create(text.color, color, textColorSpeed, (Color col) => text.color = col);
		}
		else
		{
			text.color = color;
		}
	}

	[SerializeField] private float pressSpeed = 15;
	private FloatLerp currentLerp;
	private void MoveButton(float pressPercentage)
	{
		if(currentLerp != null)
		{
			currentLerp.Destroy();
		}
		Vector2 startPos = rectTransform.anchoredPosition, endPos = originalAnchoredPosition + Vector2.down * rectTransform.sizeDelta.y / 6 * pressPercentage;
		float start = background.rectTransform.offsetMin.y, end = -rectTransform.sizeDelta.y / 6 * (1 - pressPercentage);
		currentLerp = FloatLerp.Create(0, 1, pressSpeed, (float value) =>
		{
			rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, value);
			background.rectTransform.offsetMin = new Vector2(0, Mathf.Lerp(start, end, value));
		});
	}

	protected virtual void Awake()
	{
		rectTransform = (RectTransform)transform;
	}

	private void Start()
	{
		if (!madeFromCode)
		{
			originalAnchoredPosition = rectTransform.anchoredPosition;
			TextColor = textColor;
			OriginalColor = originalColor;
			background.rectTransform.offsetMin = new Vector2(0, -rectTransform.sizeDelta.y / 6 * 1);
			buttonsText.margin = new Vector4(rectTransform.sizeDelta.y * .3f, rectTransform.sizeDelta.y * .6f, rectTransform.sizeDelta.y * .3f, rectTransform.sizeDelta.y * .0625f);
			UpdateKeys(keys.ToArray());
			UpdateText();
		}
		keyFades = new ObjectColor[keys.Count];
	}

	[SerializeField] bool or = false;

	private void Update()
	{
		if (Application.isPlaying)
		{
			if (keys.Count != 0)
			{
				if (!or)
				{
					if (resetOnKeyUp)
					{
						for (int i = 0; i < keyIndex; i++)
						{
							if (KeyCodeUtilities.GetKeyUp(keys[i]))
							{
								if (keyIndex == keys.Count)
								{

									if (!holdingMouse)
									{
										OnUp();
									}
									holdingKey = false;
								}
								if (i == 0)
								{
									hoverKey = false;
									if (!hoverMouse)
									{
										OnExit();
									}
								}
								if (buttonLocked) break;
								keyIndex = i;
								UpdateText();
								break;
							}
						}
					}
					if (keyIndex != keys.Count)
					{
						if (KeyCodeUtilities.GetKeyDown(keys[keyIndex]))
						{
							if (keyIndex == 0)
							{
								if (!hoverMouse)
								{
									OnEnter();
								}
								hoverKey = true;
							}
							if (!buttonLocked)
							{
								keyIndex++;
								UpdateText();
							}
							if (keyIndex == keys.Count)
							{
								if (!holdingMouse)
								{
									OnDown();
								}
								holdingKey = true;
							}
						}
					}
					else
					{
						if (KeyCodeUtilities.GetKeyUp(keys[keyIndex - 1]))
						{
							if (!holdingMouse)
							{
								OnUp();
								if (!hoverMouse)
								{
									OnExit();
								}
								hoverKey = false;
								if (!buttonLocked)
								{
									keyIndex = 0;
									UpdateText();
								}
							}
							holdingKey = false;
						}
					}
				}
				else
				{
					if(keyIndex == 0)
					{
						for(int i = 0; i < keys.Count; i++)
						{
							if(KeyCodeUtilities.GetKeyDown(keys[i]))
							{
								if (!hoverMouse)
								{
									OnEnter();
								}
								hoverKey = true;
								if (!buttonLocked)
								{
									keyIndex = keys.Count;
									UpdateText();
									if (!holdingMouse)
									{
										OnDown();
									}
									holdingKey = true;
								}
								break;
							}
						}
					}
					else
					{
						bool ok = false;
						for (int i = 0; i < keys.Count; i++)
						{
							if (KeyCodeUtilities.GetKey(keys[i]))
							{
								ok = true;
								break;
							}
						}
						if(!ok)
						{
							if (!holdingMouse)
							{
								OnUp();
								if (!hoverMouse)
								{
									OnExit();
								}
								hoverKey = false;
								if (!buttonLocked)
								{
									keyIndex = 0;
									UpdateText();
								}
							}
							holdingKey = false;
						}
					}
				}
			}
		}
		else
		{
			TextColor = textColor;
			OriginalColor = originalColor;
			background.rectTransform.offsetMin = new Vector2(0, -rectTransform.sizeDelta.y / 6 * 1);
			text.margin = new Vector4(rectTransform.sizeDelta.y * 0.25f, rectTransform.sizeDelta.y * 0.25f, rectTransform.sizeDelta.y * 0.25f, rectTransform.sizeDelta.y * 0.25f);
			buttonsText.margin = new Vector4(rectTransform.sizeDelta.y * .3f, rectTransform.sizeDelta.y * .6f, rectTransform.sizeDelta.y * .3f, rectTransform.sizeDelta.y * .0625f);
			UpdateKeys(keys.ToArray());
			UpdateText();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!allowMouse) return;
		holdingMouse = true;
		if (!buttonLocked)
		{
			if (!holdingKey)
			{
				keyIndex = keys.Count;
				UpdateText();
				OnDown();
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!allowMouse) return;
		hoverMouse = true;
		if (!hoverKey && !holdingMouse)
		{
			OnEnter();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!allowMouse) return;
		hoverMouse = false;
		if (!hoverKey && !holdingMouse)
		{
			OnExit();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!allowMouse) return;
		holdingMouse = false;
		if (!holdingKey)
		{
			hoverKey = false;
		}
		if (!holdingKey)
		{
			if(hoverMouse)
				OnUp();
			if (!buttonLocked)
			{
				keyIndex = 0;
				UpdateText();
				if (!onButton)
				{
					OnExit();
				}
			}
		}
	}

	private void UpdateText()
	{
		string[] split = buttonsText.text.Split(' ');
		for (int i = 0; i < keys.Count; i++)
		{
			if (keyFades?[i] != null)
			{
				keyFades[i].Destroy();
			}
			Color color = (i < keyIndex) ? CorrectTextColor : NonCorrectTextColor;
			int j = i;
			if (Application.isPlaying)
			{
				keyFades[i] = ObjectColor.Create(ColorUtilities.GetTextColor(split[i * 2]), color, 7, (Color col) =>
				{
					split[j * 2] = ColorUtilities.SetTextColor(keys[j].ConvertToString(), col);
					if (j != 0)
					{
						split[j * 2 - 1] = ColorUtilities.SetTextColor((!or) ? "+" : "-", col);
					}
					buttonsText.text = string.Join(" ", split);
				});
			}
			else
			{
				split[j * 2] = ColorUtilities.SetTextColor(keys[j].ConvertToString(), color);
				if (j != 0)
				{
					split[j * 2 - 1] = ColorUtilities.SetTextColor((!or) ? "+" : "-", color);
				}
				buttonsText.text = string.Join(" ", split);
			}
		}
	}

	[SerializeField] private bool showKeysText = true;
	public void UpdateKeys(params KeyCode[] keyCodes)
	{
		if (!allowKeys) keyCodes = new KeyCode[0];
		if (keyCodes.Length != 0 && showKeysText)
		{
			text.margin = new Vector4(rectTransform.sizeDelta.y * .25f, rectTransform.sizeDelta.y * .125f, rectTransform.sizeDelta.y * .25f, rectTransform.sizeDelta.y * .4f);
			buttonsText.enabled = true;
		}
		else
		{
			text.margin = new Vector4(rectTransform.sizeDelta.y * .25f, rectTransform.sizeDelta.y * .25f, rectTransform.sizeDelta.y * .25f, rectTransform.sizeDelta.y * .25f);
			buttonsText.enabled = false;
		}
		string newText = "";
		if (keyCodes.Length != 0)
		{
			keys = new List<KeyCode>(keyCodes);
			keyFades = new ObjectColor[keys.Count];
			for (int i = 0; i < keys.Count - 1; i++)
			{
				newText += ColorUtilities.SetTextColor(keys[i].ConvertToString(), ColorUtilities.black) + " + ";
			}
			newText += ColorUtilities.SetTextColor(keys[keys.Count - 1].ConvertToString(), ColorUtilities.black);
		}
		else
		{
			keys = new List<KeyCode>();
			keyFades = null;
		}
		buttonsText.text = newText;
	}

	FunctionTimer timer;
	public void Lock(float lockDuration)
	{
		timer?.Destroy();
		buttonLocked = true;
		timer = FunctionTimer.Create(Unlock, lockDuration);
	}

	public void Unlock()
	{
		if (buttonLocked)
		{
			buttonLocked = false;

			if (!holding)
			{
				if (keys.Count == 0 || !KeyCodeUtilities.GetKey(keys[keys.Count - 1]))
				{
					keyIndex = 0;
					SetTextColor(TextColor);
					MoveButton(0.1f);
					SetColor(HoverColor);
					SetTextColor(TextHoverColor);
					if (!onButton)
					{
						OnExit();
					}
					UpdateText();
				}
			}
		}
	}

	public void SetText(string t)
	{
		text.text = t;
	}

	public TextMeshProUGUI GetText() => text;
}
  