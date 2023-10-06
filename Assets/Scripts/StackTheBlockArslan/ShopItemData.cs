using System;
using UnityEngine;

namespace StackTheBlockArslan
{
	[Serializable]
	public class ShopItemData
	{
		public int ID;

		public bool isLocked;

		public float cost;

		public Sprite thumbnail;
	}
}
