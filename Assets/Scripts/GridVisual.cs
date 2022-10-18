using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridVisual : MonoBehaviour
{
	private const float x0 = -160;
	private const float y0 = -285;

	private const float cellSize = 160;

	private const float particleScale = 200f;

	Image winLine = null;
	ParticleSystem lineParticles = null;
	RectTransform container = null;

	private TextMeshProUGUI[,] grid = new TextMeshProUGUI[Grid.gridSize, Grid.gridSize];

	private void Awake()
	{
		winLine = transform.Find("winLine").GetComponent<Image>();
		lineParticles = transform.Find("lineParticles").GetComponent<ParticleSystem>();
		container = (RectTransform)transform.Find("container");
	}

	private void OnEnable()
	{
		Grid.OnGridChange += Grid_OnGridChange;
		Grid.On3Match += Grid_On3Match;
		Grid.OnGridReset += Grid_OnGridReset;
	}

	private void OnDisable()
	{
		Grid.OnGridChange -= Grid_OnGridChange;
		Grid.On3Match -= Grid_On3Match;
		Grid.OnGridReset -= Grid_OnGridReset;
	}

	private void Grid_OnGridReset()
	{
		for (int i = 0; i < Grid.gridSize; i++)
		{
			for (int j = 0; j < Grid.gridSize; j++)
			{
				if(grid[i, j])
				{
					Destroy(grid[i, j]);
				}
			}
		}
		winLine.enabled = false;
	}

	private void Grid_OnGridChange(int x, int y, Symbol symbol)
	{
		if (!grid[x, y])
			grid[x, y] = CreateGridVisualSymbol(x, y);
		else print("bruh");
		grid[x, y].text = symbol.ToString();
		grid[x, y].color = ColorScheme.GetSymbolColor(symbol);
	}

	private void Grid_On3Match(Symbol symbol, Vector2Int[] combination)
	{
		SpawnWinLineAndParticles(ColorScheme.GetSymbolColor(symbol), combination);
	}

	private void SpawnWinLineAndParticles(Color color, Vector2Int[] combination)
	{
		winLine.enabled = true;
		Vector2 start = GetUIPosition(combination[0].x, combination[0].y);
		Vector2 end = GetUIPosition(combination[Grid.gridSize - 1].x, combination[Grid.gridSize - 1].y);
		Vector2 position = (end + start) / 2;
		winLine.rectTransform.anchoredPosition = position;
		float size = Vector2.Distance(start, end);
		winLine.rectTransform.sizeDelta = new Vector2(size, winLine.rectTransform.sizeDelta.y);
		float angle = Utilities.GetAngleFromVector((end - start).normalized);

		winLine.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
		winLine.rectTransform.SetAsLastSibling();
		winLine.color = color;
		winLine.fillAmount = 0;
		lineParticles.transform.localEulerAngles = new Vector3(0, 0, angle);
		ParticleUtilities.SetParticleCircleShape(lineParticles, size / particleScale, 1);
		ParticleUtilities.SetParticleDuration(lineParticles, 0.2f * size / particleScale);
		lineParticles.transform.localPosition = new Vector3(position.x, position.y, -5);
		ParticleUtilities.SetParticlesColor(lineParticles, color);
		lineParticles.Play();
		lineParticles.transform.SetAsLastSibling();
		FloatLerp.Create(0, 1, 3.33f, (float value) => winLine.fillAmount = value);
	}

	private TextMeshProUGUI CreateGridVisualSymbol(int x, int y)
	{
		TextMeshProUGUI textComponent = new GameObject("field", typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>();
		textComponent.rectTransform.SetParent(container, false);
		textComponent.rectTransform.anchoredPosition = GetUIPosition(x, y);
		textComponent.alignment = TextAlignmentOptions.Midline;
		textComponent.fontSize = 70;
		textComponent.font = GameAssets.i.arcadeFont;
		return textComponent;
	}

	public static Vector2 GetUIPosition(int x, int y)
	{
		return new Vector2(x0 + x * cellSize, y0 + y * cellSize);
	}
}
