using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "Stack the Blocks/Color Palette", order = 0)]
public class ColorPalette : ScriptableObject
{
	public List<ColorBP> colors;

	public void FixColorAlpha()
	{
		for (int i = 0; i < colors.Count; i++)
		{
			colors[i].blockColor.a = 255f;
			colors[i].TopColor.a = 255f;
			colors[i].BottomColor.a = 255f;
		}
	}
}
