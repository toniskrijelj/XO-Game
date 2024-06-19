using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
	[SerializeField] Player player = null;

	Image indicator = null;
	ColorScheme scheme = null;

	private void Awake()
	{
		indicator = GetComponent<Image>();
		scheme = GetComponent<ColorScheme>();
		scheme.setColorAction = SetIndicatorColor;
	}

	private void Start()
	{
		Player_OnActive(player.IsActive());
	}

	private void OnEnable()
	{
		player.OnActive += Player_OnActive;
		player.OnMove += Player_OnMove;
	}

	private void OnDisable()
	{
		player.OnActive -= Player_OnActive;
		player.OnMove -= Player_OnMove;
	}

	private void Player_OnMove(int x, int y)
	{
		indicator.rectTransform.anchoredPosition = GridVisual.GetUIPosition(x, y);
	}

	private void Player_OnActive(bool value)
	{
		SetIndicatorColor(indicator.color);
		if(value)
			indicator.rectTransform.SetAsLastSibling();
	}

	private void SetIndicatorColor(Color color)
	{
		color.a = player.IsActive() ? 1 : 0.25f;
		indicator.color = color;
	}
}
