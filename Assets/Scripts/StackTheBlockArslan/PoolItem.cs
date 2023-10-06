using System;
using UnityEngine;

namespace StackTheBlockArslan
{
	[Serializable]
	public class PoolItem
	{
		public string itemName;

		public GameObject objectToPool;

		public int amountToPool;

		public bool shouldExpand;
	}
}
