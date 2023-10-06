using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StackTheBlockArslan
{
	public abstract class ShopManager<T> : MonoBehaviour where T : ShopItemData
	{
		[Tooltip("UI Gameobject parent for shop items")]
		[SerializeField]
		private GameObject container;

		[Tooltip("The UI shop item prefab")]
		[SerializeField]
		private GameObject shopItemPrefab;

		private ShopItem selectedShopItem;

		private ShopItem selectedShopItemforBuying;

		private float availableCash;

		private List<int> unlockedItemIDs;

		private int selectedItemID;

		protected abstract List<T> GetDefaultList();

		protected abstract float Getcash();

		protected abstract List<int> GetUnlockedItemIDs();

		protected abstract int GetSelectedItemID();

		public void InitShopList()
		{
			availableCash = Getcash();
			unlockedItemIDs = GetUnlockedItemIDs();
			selectedItemID = GetSelectedItemID();
			List<ShopItemData> list = GetDefaultList().ConvertAll((Converter<T, ShopItemData>)((T x) => x));
			for (int i = 0; i < list.Count; i++)
			{
				GameObject shopItem = UnityEngine.Object.Instantiate(shopItemPrefab, container.transform, worldPositionStays: false);
				ShopItemData itemData = list[i];
				shopItem.GetComponent<ShopItem>().setItem(itemData);
				shopItem.GetComponentInChildren<Button>().onClick.AddListener(delegate
				{
					selectItem(shopItem.GetComponent<ShopItem>());
				});
				if (unlockedItemIDs.Exists((int x) => x == itemData.ID))
				{
					shopItem.GetComponent<ShopItem>().toggleLock(status: false);
				}
				if (list[i].ID == selectedItemID)
				{
					shopItem.GetComponent<ShopItem>().toggleselection(status: true);
					selectedShopItem = shopItem.GetComponent<ShopItem>();
				}
			}
			shopItemsInitialised();
		}

		protected virtual void shopItemsInitialised()
		{
		}

		public void RefreshScreen()
		{
			availableCash = Getcash();
		}

		public void selectItem(ShopItem shopItem)
		{
			if (!shopItem.itemData.isLocked)
			{
				if (selectedShopItem != null)
				{
					selectedShopItem.toggleselection(status: false);
				}
				selectedShopItem = shopItem;
				shopItem.toggleselection(status: true);
				ItemSelected(isAlreadyPurchased: true, hasSufficientCash: true, (T)selectedShopItem.itemData);
			}
			else
			{
				selectedShopItemforBuying = shopItem;
				if (selectedShopItemforBuying.itemData.cost <= availableCash)
				{
					ItemSelected(isAlreadyPurchased: false, hasSufficientCash: true, (T)selectedShopItem.itemData);
				}
				else
				{
					ItemSelected(isAlreadyPurchased: false, hasSufficientCash: false, (T)selectedShopItem.itemData);
				}
			}
		}

		protected abstract void ItemSelected(bool isAlreadyPurchased, bool hasSufficientCash, T item);

		public void BuyItem()
		{
			if (selectedShopItemforBuying.itemData.cost <= availableCash)
			{
				selectedShopItemforBuying.toggleLock(status: false);
				selectItem(selectedShopItemforBuying);
				availableCash -= selectedShopItemforBuying.itemData.cost;
				ItemPurcahsed(success: true, availableCash, selectedShopItemforBuying.itemData.ID);
				selectedShopItemforBuying = null;
			}
			else
			{
				ItemPurcahsed(success: false);
			}
		}

		protected abstract void ItemPurcahsed(bool success = true, float remaingingCash = 0f, int newUnlockedId = 0);
	}
}
