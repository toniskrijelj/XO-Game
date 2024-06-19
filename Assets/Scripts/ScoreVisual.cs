using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreVisual : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI xScore = null;
	[SerializeField] TextMeshProUGUI oScore = null;

	Dictionary<Symbol, TextMeshProUGUI> scoreTexts;

	private void Awake()
	{
		scoreTexts = new Dictionary<Symbol, TextMeshProUGUI>
		{
			{ Symbol.X, xScore },
			{ Symbol.O, oScore }
		};
	}

	private void OnEnable()
	{
		Score.OnScoreChanged += Score_OnScoreChanged;
	}

	private void OnDisable()
	{
		Score.OnScoreChanged -= Score_OnScoreChanged;
	}

	private void Score_OnScoreChanged(Symbol symbol, int score)
	{
		scoreTexts[symbol].text = score.ToString();
	}
}
