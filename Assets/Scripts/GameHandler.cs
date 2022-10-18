using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
	[SerializeField] Player playerX = null;
	[SerializeField] Player playerO = null;

	[SerializeField] float restartGameTimer = 1;

	private void Start()
	{
		PickRandom();
	}

	private void RestartGame()
	{
		Grid.ResetGrid();
		PickRandom();
	}

	private void PickRandom()
	{
		bool q = UnityEngine.Random.value <= 0.5f;
		playerX.SetActive(q);
		playerO.SetActive(!q);
	}

	private void OnEnable()
	{
		Grid.OnGridChange += Grid_OnGridChange;
		Grid.On3Match += Grid_On3Match;
		Grid.OnGridFull += Grid_OnGridFull;
	}

	private void OnDisable()
	{
		Grid.OnGridChange -= Grid_OnGridChange;
		Grid.On3Match -= Grid_On3Match;
		Grid.OnGridFull -= Grid_OnGridFull;
	}

	private void Grid_OnGridChange(int x, int y, Symbol symbol)
	{
		playerX.SetActive(symbol != Symbol.X);
		playerO.SetActive(symbol != Symbol.O);
	}

	private void Grid_On3Match(Symbol symbol, Vector2Int[] combination)
	{
		Grid_OnGridFull();
	}

	private void Grid_OnGridFull()
	{
		playerX.SetActive(false);
		playerO.SetActive(false);
		FunctionTimer.Create(RestartGame, restartGameTimer);
	}
}
