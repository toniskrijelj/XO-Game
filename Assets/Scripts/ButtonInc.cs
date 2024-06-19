using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonInc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] float scalehoverMultiplier = 1.05f;
	[SerializeField] float scaleClickMultiplier = 1.2f;
	[SerializeField] float scaleUpSpeed = 3;
	[SerializeField] float scaleDownSpeed = 4;

	[SerializeField] bool triggerOnKeyDown = false;
	[SerializeField] bool useUnscaledDeltaTime = false;

	[SerializeField] KeyCode key = KeyCode.None;

	[SerializeField] AudioClip hoverSound = null;
	[SerializeField] AudioClip clickSound = null;

	[SerializeField] UnityEvent OnClickUnityEvent = null;
	private Action OnClick;

	FloatLerp lerp;

	bool holdingMouse;
	bool holdingKey;
	bool mouseOnButton;
	Vector2 baseScale;

	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseOnButton = true;
		if(hoverSound != null)
		{
			AudioEffects.PlayClip(hoverSound, true);
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		mouseOnButton = false;
	}

	private void UpdateScale(float scale)
	{
		transform.localScale = baseScale * scale;
	}

	private void Awake()
	{
		baseScale = transform.localScale;
		lerp = FloatLerp.CreateEmpty(UpdateScale, null, "", useUnscaledDeltaTime);
		if(key != KeyCode.None) FunctionKey.Create(key, () => 
		{
			if ((Time.timeScale == 1 || useUnscaledDeltaTime) && gameObject.activeInHierarchy)
			{
				if (triggerOnKeyDown)
				{
					Click();
				}
				else
					holdingKey = true;
			}
		}, null, () => 
		{
			if(!triggerOnKeyDown)
				if ((Time.timeScale == 1 || useUnscaledDeltaTime) && holdingKey && gameObject.activeInHierarchy)
					Click();
			holdingKey = false;
		});
	}

	private void OnEnable()
	{
		UpdateScale(1);
		lerp.value = 1;
		lerp.target = 1;
	}

	private void OnDisable()
	{
		mouseOnButton = false;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			holdingMouse = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (mouseOnButton)
			{
				Click();
			}
			holdingMouse = false;
		}
		if(mouseOnButton || holdingKey)
		{
			lerp.target = holdingMouse || holdingKey ? scaleClickMultiplier : scalehoverMultiplier;
			lerp.speed = scaleUpSpeed;
		}
		else
		{
			lerp.target = 1;
			lerp.speed = scaleDownSpeed;
		}
	}

	private void Click()
	{
		if(clickSound != null)
		{
			AudioEffects.PlayClip(clickSound, true);
		}
		OnClick?.Invoke();
		OnClickUnityEvent?.Invoke();
	}

	public void SetOnClick(Action OnClick)
	{
		this.OnClick = OnClick;
	}
}
