using System.Collections.Generic;
using UnityEngine;

namespace StackTheBlockArslan
{
	public class PnCUIManger : MonoBehaviour
	{
		public List<UIScreen> UIScreens;

		private GameObject lastScreen;

		private GameObject openScreen;

		private GameObject modalScreen;

		private Dictionary<string, GameObject> screensDictionary = new Dictionary<string, GameObject>();

		private void Awake()
		{
			foreach (UIScreen uIScreen in UIScreens)
			{
				screensDictionary.Add(uIScreen.screenName, uIScreen.screenGameObj);
			}
			AwakeInit();
		}

		protected virtual void AwakeInit()
		{
		}

		public void OpenScreen(UIScreensList screen)
		{
			OpenScreen(screen.ToString());
		}

		public void OpenScreen(string screenName)
		{
			GameObject value = null;
			if (screensDictionary.TryGetValue(screenName, out value))
			{
				if (openScreen != null)
				{
					openScreen.SetActive(value: false);
					lastScreen = openScreen;
				}
				value.SetActive(value: true);
				openScreen = value;
			}
			else
			{
				UnityEngine.Debug.LogError("Screen does not exist");
			}
		}

		public void Back()
		{
			if (lastScreen != null)
			{
				lastScreen.SetActive(value: true);
			}
			openScreen.SetActive(value: false);
			GameObject gameObject = lastScreen;
			lastScreen = openScreen;
			openScreen = gameObject;
		}

		public void OpenModal(UIScreensList screen)
		{
			OpenModal(screen.ToString());
		}

		public void OpenModal(string screenName)
		{
			GameObject value = null;
			if (screensDictionary.TryGetValue(screenName, out value))
			{
				value.SetActive(value: true);
				modalScreen = value;
			}
			else
			{
				UnityEngine.Debug.LogError("Screen does not exist");
			}
		}

		public void CloseModal()
		{
			if (modalScreen != null)
			{
				modalScreen.SetActive(value: false);
			}
		}
	}
}
