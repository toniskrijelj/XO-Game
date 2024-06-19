using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Grid
{
	public const int gridSize = 3;

	public static event Action<int, int, Symbol> OnGridChange;
	public static event Action<Symbol, Vector2Int[]> On3Match;
	public static event Action OnGridFull;
	public static event Action OnGridReset;

	private static Symbol[,] grid = new Symbol[gridSize, gridSize];

	public static void ResetGrid()
	{
		if(grid != null)
		{
			for (int i = 0; i < gridSize; i++)
			{
				for (int j = 0; j < gridSize; j++)
				{
					grid[i, j] = Symbol.None;
				}
			}
		}
		OnGridReset?.Invoke();
	}

	public static void TrySetSymbol(int x, int y, Symbol symbol)
	{
		if(Utilities.IsBetween(x, 0, gridSize - 1) && Utilities.IsBetween(y, 0, gridSize - 1) && grid[x, y] == Symbol.None)
		{
			grid[x, y] = symbol;
			OnGridChange?.Invoke(x, y, symbol);
			Vector2Int[] combination = Check3Match(symbol);
			if (combination != null)
			{
				On3Match?.Invoke(symbol, combination);
			}
			else
			{
				bool full = true;
				for (int i = 0; i < gridSize; i++)
				{
					for (int j = 0; j < gridSize; j++)
					{
						if(grid[i, j] == Symbol.None)
						{
							full = false;
							break;
						}
					}
					if (!full) break;
				}
				if(full)
				{
					OnGridFull?.Invoke();
				}
			}
		}
	}

	private static Vector2Int[] Check3Match(Symbol symbol)
	{
		bool match;
		Vector2Int[] combination = new Vector2Int[gridSize];
		for (int x = 0; x < gridSize; x++)
		{
			match = true;
			for(int y = 0; y < gridSize; y++)
			{
				combination[y].x = x;
				combination[y].y = y;
				if (!CheckMatch(x, y, symbol))
				{
					match = false;
					break;
				}
			}
			if (match) return combination;
		}
		for (int y = 0; y < gridSize; y++)
		{
			match = true;
			for (int x = 0; x < gridSize; x++)
			{
				combination[x].x = x;
				combination[x].y = y;
				if (!CheckMatch(x, y, symbol))
				{
					match = false;
					break;
				}
			}
			if (match) return combination;
		}
		match = true;
		for(int i = 0; i < gridSize; i++)
		{
			combination[i].x = i;
			combination[i].y = i;
			if (!CheckMatch(i, i, symbol))
			{
				match = false;
				break;
			}
		}
		if (match) return combination;
		match = true;
		for (int i = 0; i < gridSize; i++)
		{
			combination[i].x = i;
			combination[i].y = gridSize - i - 1;
			if (!CheckMatch(i, gridSize - i - 1, symbol))
			{
				match = false;
				break;
			}
		}
		return match ? combination : null;
	}

	private static bool CheckMatch(int x, int y, Symbol symbol)
	{
		return grid[x, y] == symbol;
	}

}
