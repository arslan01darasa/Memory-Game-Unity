using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGridFitter : MonoBehaviour
{
	private RectTransform container;

	[Tooltip("Desired width divided by desired height")]
	public float aspectRatio;

	public int noOfColumns;

	private GridLayoutGroup gridLayoutGroup;

	private void Awake()
	{
		container = GetComponent<RectTransform>();
		gridLayoutGroup = GetComponent<GridLayoutGroup>();
	}

	private void OnEnable()
	{
		StartCoroutine(SetGridLayout());
	}

	private IEnumerator SetGridLayout()
	{
		yield return new WaitForEndOfFrame();
		float num = (float)(gridLayoutGroup.padding.left + gridLayoutGroup.padding.right) + (float)(noOfColumns - 1) * gridLayoutGroup.spacing.x;
		float num2 = (container.rect.width - num) / (float)noOfColumns;
		gridLayoutGroup.cellSize = new Vector2(num2, num2 * aspectRatio);
	}
}
