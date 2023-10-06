using UnityEngine;
using UnityEngine.UI;

namespace StackTheBlockArslan
{
	public class ShopItem : MonoBehaviour
	{
		[HideInInspector]
		public ShopItemData itemData;

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image thumbNailImage;

		[SerializeField]
		private Text costText;

		[SerializeField]
		private GameObject selectedOverlayImg;

		[SerializeField]
		private GameObject lockImg;

		public void setItem(ShopItemData _skin)
		{
			itemData = _skin;
			thumbNailImage.sprite = _skin.thumbnail;
			costText.text = _skin.cost.ToString();
			toggleLock(_skin.isLocked);
		}

		public void toggleselection(bool status)
		{
			if (!itemData.isLocked)
			{
				selectedOverlayImg.SetActive(status);
			}
		}

		public void toggleLock(bool status)
		{
			itemData.isLocked = status;
			lockImg.SetActive(status);
		}
	}
}
