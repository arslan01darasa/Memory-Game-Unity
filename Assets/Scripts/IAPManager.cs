using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
	[HideInInspector]
	public bool isIAPEnabled;

	public string gameIdentifier = "com.PolyandCode.StackTheBlocks";

	public string NonConsumableProductID = "com.PolyandCode.StackTheBlocks.nonconsumable";

	public Button restoreIAPBtn;

	public UnityEvent OnPurchaseSuccessEvent;

	public UnityEvent OnPurchaseFailedEvent;

	private void Start()
	{
		if (restoreIAPBtn != null)
		{
			restoreIAPBtn.gameObject.SetActive(value: false);
		}
	}

	public void BuyNonConsumable()
	{
	}

	public void RestorePurchases()
	{
	}
}
