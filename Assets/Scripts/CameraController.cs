using StackTheBlockArslan;
using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Tooltip("Speed at which camera moves to the next position and zooms out at game over")]
	[SerializeField]
	private float speed;

	[Tooltip("The block from which game starts")]
	[SerializeField]
	private Transform startBaseBlock;

	private float heightDelta;

	private Vector3 startPos;

	private Vector3 targetPos;

	private Camera cam;

	[Tooltip("Camera zooms out by this factor multuplied with number of stacked blocks")]
	[SerializeField]
	private float zoomOutFactor;

	private float defaultZoomValue;

	[Tooltip("The minimum zoom value so that there is some zoom out even at zero score")]
	[SerializeField]
	private float minZoomValue;

	[Tooltip("The maximum zoom value so that we don't end up with a hardly visible tower of blocks")]
	[SerializeField]
	private float maxZoomValue;

	private void Awake()
	{
		cam = GetComponent<Camera>();
		startPos = base.transform.position;
		targetPos = base.transform.position;
		heightDelta = startBaseBlock.lossyScale.y;
		defaultZoomValue = cam.orthographicSize;
		GameManager instance = LazySingleton<GameManager>.Instance;
		instance.GameInitialized = (Action)Delegate.Combine(instance.GameInitialized, new Action(ResetCam));
		GameManager instance2 = LazySingleton<GameManager>.Instance;
		instance2.GameOver = (Action)Delegate.Combine(instance2.GameOver, new Action(GameOverHandler));
	}

	private void Update()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, targetPos, Time.deltaTime * speed);
	}

	public void UpdatePos()
	{
		targetPos += Vector3.up * heightDelta;
	}

	private IEnumerator ZoomOutCouroutine()
	{
		float targetOrthographicSize2 = (float)LazySingleton<ScoreAndCashManager>.Instance.currentScore * zoomOutFactor;
		targetOrthographicSize2 = Mathf.Clamp(targetOrthographicSize2, minZoomValue, maxZoomValue);
		while (cam.orthographicSize < targetOrthographicSize2 - 0.3f)
		{
			yield return new WaitForEndOfFrame();
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthographicSize2, Time.deltaTime * speed);
		}
	}

	private void GameOverHandler()
	{
		StartCoroutine("ZoomOutCouroutine");
	}

	private void ResetCam()
	{
		StopCoroutine("ZoomOutCouroutine");
		base.transform.position = startPos;
		targetPos = startPos;
		cam.orthographicSize = defaultZoomValue;
	}
}
