using System.Collections;
using UnityEngine;

namespace StackTheBlockArslan
{
	public class CustomGridLayout : MonoBehaviour
	{
		private RectTransform container;

		public int coloumns;

		public float padding;

		public float spacing;

		private IEnumerator Start()
		{
			container = GetComponent<RectTransform>();
			if (coloumns <= 0)
			{
				UnityEngine.Debug.Log("Number of coloums in zero or invalid");
				yield break;
			}
			if (spacing < 0f || padding < 0f)
			{
				UnityEngine.Debug.Log("Invalid spacing/padding");
				yield break;
			}
			yield return new WaitForSeconds(0.1f);
			float num = padding * 2f + (float)(coloumns - 1) * spacing;
			float cellWidth = (container.rect.width - num) / (float)coloumns;
			int num2 = 0;
			int num3 = 0;
			foreach (Transform item in container.transform)
			{
				item.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, padding + (float)num3 * (cellWidth + spacing), cellWidth);
				item.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, padding + (float)num2 * (cellWidth + spacing), cellWidth);
				num3++;
				if (num3 % coloumns == 0)
				{
					num2++;
					num3 = 0;
				}
			}
			yield return new WaitForEndOfFrame();
			int num4 = (int)Mathf.Ceil((float)container.transform.childCount / (float)coloumns);
			container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellWidth * (float)num4 + spacing * (float)(num4 - 1) + padding * 2f);
		}
	}
}
