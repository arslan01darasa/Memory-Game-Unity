using StackTheBlockArslan;
using System.Collections.Generic;
using UnityEngine;

public class SkinsShopManager : ShopManager<SkinItemData>
{
	[Tooltip("Default List of skins. This is a Scriptable object asset")]
	[SerializeField]
	private SkinList skinList;

	[Tooltip("The amount of cash added after user watches ad ad")]
	[SerializeField]
	private float adRewardCash;

	public void OpenSkinsScreen()
	{
		SoundManager.Instance.playSound(AudioClips.UI);
		RefreshScreen();
		UIManager.Instance.cashText.text = Getcash().ToString();
		UIManager.Instance.OpenScreen(UIScreensList.SkinsShop);
	}

	protected override float Getcash()
	{
		return PlayerDataHandler<PlayerData>.Instance.cash;
	}

	protected override List<int> GetUnlockedItemIDs()
	{
		return PlayerDataHandler<PlayerData>.Instance.unlockedSkinItems;
	}

	protected override int GetSelectedItemID()
	{
		return PlayerDataHandler<PlayerData>.Instance.selectedSkinID;
	}

	private void Start()
	{
		InitShopList();
		LazySingleton<GameplayController>.Instance.UpdateSkin(skinList.skins.Find((SkinItemData x) => x.ID == GetSelectedItemID()).texture);
	}

	protected override List<SkinItemData> GetDefaultList()
	{
		return Object.Instantiate(skinList).skins;
	}

	protected override void ItemSelected(bool isAlreadyPurchased, bool hasSufficientCash, SkinItemData item)
	{
		SoundManager.Instance.playSound(AudioClips.UI);
		if (isAlreadyPurchased)
		{
			PlayerDataHandler<PlayerData>.Instance.selectedSkinID = item.ID;
			PlayerDataHandler<PlayerData>.Instance.SaveData();
			LazySingleton<GameplayController>.Instance.UpdateSkin(item.texture);
		}
		else if (hasSufficientCash)
		{
			UIManager.Instance.ToggleOpenCostPopUp(status: true);
		}
		else
		{
			UIManager.Instance.ToggleOpenCostPopUp(status: true, "Insufficient cash!");
		}
	}

	protected override void ItemPurcahsed(bool success = true, float remaingingCash = 0f, int newUnlockedId = 0)
	{
		SoundManager.Instance.playSound(AudioClips.UI);
		if (success)
		{
			PlayerDataHandler<PlayerData>.Instance.unlockedSkinItems.Add(newUnlockedId);
			PlayerDataHandler<PlayerData>.Instance.selectedSkinID = newUnlockedId;
			PlayerDataHandler<PlayerData>.Instance.cash = remaingingCash;
			PlayerDataHandler<PlayerData>.Instance.SaveData();
			UIManager.Instance.cashText.text = remaingingCash.ToString();
			UIManager.Instance.ToggleOpenCostPopUp(status: false);
		}
		else
		{
			UIManager.Instance.ToggleOpenCostPopUp(status: false);
		}
	}

	public void WatchAdForCash()
	{
		//		LazySingleton<AdsManager>.Instance.ShowRewardedAd(OnAdSuccess, OnAdFailed);
		if (AdsManager.mInstance)
		{
			AdsManager.mInstance.ShowRewardedAd();
			OnAdSuccess();
        }
		else
		{
			OnAdFailed();
        }

		

	}

	private void OnAdSuccess()
	{
		PlayerDataHandler<PlayerData>.Instance.cash += adRewardCash;
		PlayerDataHandler<PlayerData>.Instance.SaveData();
		UIManager.Instance.cashText.text = PlayerDataHandler<PlayerData>.Instance.cash.ToString();
	}

	private void OnAdFailed()
	{
		UnityEngine.Debug.Log("watch Ad for Cash failed");
	}
}
