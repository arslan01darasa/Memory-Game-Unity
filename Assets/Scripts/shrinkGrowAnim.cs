using UnityEngine;

public class shrinkGrowAnim : MonoBehaviour
{
	public float min;

	public float max;

	public float speed;

	private float timer;

	private void OnEnable()
	{
		timer = -1.57f;
		timer = 0f;
		base.gameObject.transform.localPosition = new Vector3(max * Mathf.Sin(timer), base.gameObject.transform.localPosition.y, base.gameObject.transform.localPosition.z);
	}

	private void Update()
	{
		base.gameObject.transform.localPosition = new Vector3(max * Mathf.Sin(timer), base.gameObject.transform.localPosition.y, base.gameObject.transform.localPosition.z);
		timer += Time.deltaTime;
	}
}
