using UnityEngine;

public class BlockScript : MonoBehaviour
{
	private Vector3 pointA;

	private Vector3 pointB;

	private Vector3 target;

	[HideInInspector]
	public float speed;

	public void setPositions(Vector3 point1, Vector3 point2)
	{
		pointA = point1;
		pointB = point2;
		target = pointA;
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.localPosition, target) < 0.1f)
		{
			target = ((target == pointB) ? pointA : pointB);
		}
		base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, target, speed * Time.deltaTime);
	}
}
