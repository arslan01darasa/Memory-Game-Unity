using StackTheBlockArslan;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : LazySingleton<AdsManager>
{
	[HideInInspector]
	public bool isAdEnabled;

	[HideInInspector]
	public bool isAdRemoved;

	[SerializeField]
	private string iosGameId;

	[SerializeField]
	private string AndroidGameId;

	private string gameId;

	public bool testMode = true;

	public Ad displayAd;

	public Ad bannerAd;

	public Ad rewardedAd;
	/*
		public void ShowDisplayAd()
		{
			if (isAdRemoved)
			{
				return;
			}
			if (displayAd.useFrequency)
			{
				displayAd.callCount++;
				if (displayAd.adFrequency != 0 && displayAd.callCount % displayAd.adFrequency == 0)
				{
					StartCoroutine(ShowDisplayAdWhenReady());
				}
			}
			else
			{
				StartCoroutine(ShowDisplayAdWhenReady());
			}
		}

		public void ShowBannerAd()
		{
			if (isAdRemoved)
			{
				return;
			}
			if (bannerAd.useFrequency)
			{
				bannerAd.callCount++;
				if (bannerAd.adFrequency != 0 && bannerAd.callCount % bannerAd.adFrequency == 0)
				{
					StartCoroutine(ShowBannerAdWhenReady());
				}
			}
			else
			{
				StartCoroutine(ShowBannerAdWhenReady());
			}
		}

		public void HideBannerAd()
		{
		//	Advertisement.Banner.Hide();
		}

		public void ShowRewardedAd(Action success, Action failure)
		{
			rewardedAd.callCount++;
			if (rewardedAd.useFrequency && rewardedAd.adFrequency != 0 && rewardedAd.callCount % rewardedAd.adFrequency == 0)
			{
				StartCoroutine(ShowRewardedAdWhenReady(success, failure));
			}
			else
			{
				StartCoroutine(ShowRewardedAdWhenReady(success, failure));
			}
		}

		private void Awake()
		{
			gameId = AndroidGameId;
			Advertisement.Initialize(gameId, testMode);
		}

		private IEnumerator ShowDisplayAdWhenReady()
		{
			while (!Advertisement.IsReady(displayAd.placementID))
			{
				yield return new WaitForSeconds(0.25f);
			}
			yield return new WaitForSeconds(displayAd.delay);
			ShowOptions showOptions = new ShowOptions
			{
				resultCallback = HandleAdResult
			};
			Advertisement.Show(displayAd.placementID, showOptions);
		}

		private IEnumerator ShowBannerAdWhenReady()
		{
			while (!Advertisement.IsReady(bannerAd.placementID))
			{
				yield return new WaitForSeconds(0.25f);
			}
			yield return new WaitForSeconds(bannerAd.delay);
			Advertisement.Banner.Show(bannerAd.placementID);
		}

		private void HandleAdResult(ShowResult result)
		{
			switch (result)
			{
			case ShowResult.Finished:
				UnityEngine.Debug.Log("The ad was successfully shown.");
				break;
			case ShowResult.Skipped:
				UnityEngine.Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				UnityEngine.Debug.LogError("The ad failed to be shown.");
				break;
			}
		}

		private IEnumerator ShowRewardedAdWhenReady(Action _success, Action _failure)
		{
			rewardedAd.success = _success;
			rewardedAd.failure = _failure;
			while (!Advertisement.IsReady(rewardedAd.placementID))
			{
				yield return new WaitForSeconds(0.25f);
			}
			yield return new WaitForSeconds(rewardedAd.delay);
			ShowOptions showOptions = new ShowOptions
			{
				resultCallback = HandleRewardedAdResultwc
			};
			Advertisement.Show(rewardedAd.placementID, showOptions);
		}

		private void HandleRewardedAdResultwc(ShowResult result)
		{
			switch (result)
			{
			case ShowResult.Finished:
				UnityEngine.Debug.Log("Rewarded ad was successfully shown.");
				rewardedAd.success();
				break;
			case ShowResult.Skipped:
				UnityEngine.Debug.Log("Rewarded ad was skipped before reaching the end.");
				rewardedAd.failure();
				break;
			case ShowResult.Failed:
				rewardedAd.failure();
				UnityEngine.Debug.LogError("Rewarded ad failed to be shown.");
				break;
			}
		}
	}
	*/
}
