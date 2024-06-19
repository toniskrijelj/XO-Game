using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static event Action<Player, int, int> OnPlayerMove;
	public event Action<int, int> OnMove;
	public event Action<bool> OnActive;

	[SerializeField] PlayerControls controls = null;
	[SerializeField] Symbol symbol = Symbol.O;

	private int x = 1;
	private int y = 1;
	private bool active;
    
    private void Update()
    {
		if (Time.timeScale < 1) return;
		CheckPlayerMove();
		if (active && Input.GetKeyDown(controls.fire))
		{
			Grid.TrySetSymbol(x, y, symbol);
		}
    }

	private void CheckPlayerMove()
	{
		bool changed = false;
		if (Input.GetKeyDown(controls.left))
		{
			x--;
			changed = true;
		}
		if (Input.GetKeyDown(controls.up))
		{
			y++;
			changed = true;
		}
		if (Input.GetKeyDown(controls.down))
		{
			y--;
			changed = true;
		}
		if (Input.GetKeyDown(controls.right))
		{
			x++;
			changed = true;
		}
		if (changed)
		{
			x = Utilities.LoopBetween(x, 0, Grid.gridSize - 1);
			y = Utilities.LoopBetween(y, 0, Grid.gridSize - 1);
			OnMove?.Invoke(x, y);
			OnPlayerMove?.Invoke(this, x, y);
		}
	}

	public void SetActive(bool value)
	{
		if (active != value)
		{
			active = value;
			OnActive?.Invoke(value);
		}
	}

	public bool IsActive() => active;
}
