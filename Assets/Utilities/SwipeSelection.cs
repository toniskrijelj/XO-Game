using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeSelection : MonoBehaviour
{
	public Scrollbar scrollbar;
	private float scroll_pos = 0;
	float[] pos;
	float distance;

	public int selected = 0;

	void Update()
	{
		pos = new float[transform.childCount];
		distance = 1f / (pos.Length - 1f);
		for (int i = 0; i < pos.Length; i++)
		{
			pos[i] = distance * i;
		}

		MoveTowardsSelected();

		for (int i = 0; i < pos.Length; i++)
		{
			if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
			{
				if (Input.GetMouseButton(0))
				{
					selected = i;
				}
				transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
			}
			else
			{
				transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.8f, 0.8f), 0.1f);
			}
		}
	}

	private void MoveTowardsSelected()
	{
		if (!Input.GetMouseButton(0))
		{
			scrollbar.value = Mathf.Lerp(scrollbar.value, pos[selected], 0.1f);
		}
		scroll_pos = scrollbar.value;
	}
}
