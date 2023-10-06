using StackTheBlockArslan;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ContinueGameScreen : MonoBehaviour
{
	[Tooltip("Will time out after these many seconds")]
	public float timeInSeconds;

	public Image loadingImg;

	public UnityEvent successCallback;

	public UnityEvent failureCallback;

	private void OnEnable()
	{
		StartCoroutine("TimerCoroutine");
	}

	private void OnDisable()
	{
		StopCoroutine("TimerCoroutine");
	}

	private IEnumerator TimerCoroutine()
	{
		float timeleft = timeInSeconds;
		while (timeleft > 0f)
		{
			yield return new WaitForEndOfFrame();
			timeleft -= Time.deltaTime;
			loadingImg.fillAmount = timeleft / timeInSeconds;
		}
		CloseScreen();
		OnAdfailed();
	}

	public void ShowAd()
	{
		CloseScreen();
	//	LazySingleton<AdsManager>.Instance.ShowRewardedAd(OnAdSuccess, OnAdfailed);
	}

	public void CloseScreen()
	{
		base.gameObject.SetActive(value: false);
	}

	private void OnAdSuccess()
	{
		successCallback.Invoke();
	}

	private void OnAdfailed()
	{
		failureCallback.Invoke();
	}
}
