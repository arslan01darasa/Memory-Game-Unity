using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
	[Tooltip("Game's Block and Background colors (Scriptable UnityEngine.Object asset)")]
	[SerializeField]
	private ColorPalette colorPalette;

	private List<ColorBP> colors = new List<ColorBP>();

	[Tooltip("The delta with which color transitions for the block")]
	[SerializeField]
	private float delta;

	private float time;

	private int blockIndexFrom;

	private int blockIndexTo;

	[Tooltip("The UI material for Gradient background")]
	public Material backgroundMat;

	[Tooltip("The transition speed for Gradient background")]
	public float bgTransitionSpeed;

	private Color bgTopColor;

	private Color bgBottomColor;

	private Color topBGTo;

	private Color bottomBGTo;

	private int bgIndexFrom;

	private int bgIndexTo;

	public Color BlockColor
	{
		get
		{
			time += delta;
			if (time > 1f)
			{
				time = 0f;
				blockIndexFrom++;
				blockIndexFrom = ((blockIndexFrom < colors.Count) ? blockIndexFrom : 0);
				blockIndexTo = ((blockIndexFrom != colors.Count - 1) ? (blockIndexFrom + 1) : 0);
				bgIndexFrom++;
				bgIndexFrom = ((bgIndexFrom < colors.Count) ? bgIndexFrom : 0);
				bgIndexTo = ((bgIndexFrom != colors.Count - 1) ? (bgIndexFrom + 1) : 0);
			}
			topBGTo = Color.Lerp(colors[bgIndexFrom].TopColor, colors[bgIndexTo].TopColor, time);
			bottomBGTo = Color.Lerp(colors[bgIndexFrom].BottomColor, colors[bgIndexTo].BottomColor, time);
			return Color.Lerp(colors[blockIndexFrom].blockColor, colors[blockIndexTo].blockColor, time);
		}
	}

	private void Awake()
	{
		colors = UnityEngine.Object.Instantiate(colorPalette).colors;
	}

	public void Reset()
	{
		Shuffle();
		time = 0f - delta;
		bgIndexFrom = (blockIndexFrom = 0);
		bgIndexTo = (blockIndexTo = 1);
		bgTopColor = colors[0].TopColor;
		bgBottomColor = colors[0].BottomColor;
	}

	private void Shuffle()
	{
		int num = colors.Count;
		while (num > 1)
		{
			num--;
			int index = UnityEngine.Random.Range(0, num);
			ColorBP value = colors[index];
			colors[index] = colors[num];
			colors[num] = value;
		}
	}

	private void Update()
	{
		bgTopColor = Color.Lerp(bgTopColor, topBGTo, Time.deltaTime * bgTransitionSpeed);
		bgBottomColor = Color.Lerp(bgBottomColor, bottomBGTo, Time.deltaTime * bgTransitionSpeed);
		backgroundMat.SetColor("_TopColor", bgTopColor);
		backgroundMat.SetColor("_BottomColor", bgBottomColor);
	}
}
