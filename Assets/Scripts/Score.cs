using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	public static event Action<Symbol, int> OnScoreChanged;

	private Dictionary<Symbol, int> scores;

	private void Awake()
	{
		scores = new Dictionary<Symbol, int>
		{
			{ Symbol.O, 0 },
			{ Symbol.X, 0 }
		};
	}

	private void OnEnable()
	{
		Grid.On3Match += Grid_On3Match;
	}

	private void OnDisable()
	{
		Grid.On3Match -= Grid_On3Match;
	}

	private void Grid_On3Match(Symbol symbol, Vector2Int[] combination)
	{
		OnScoreChanged?.Invoke(symbol, ++scores[symbol]);
	}
}
