using UnityEngine;

namespace StackTheBlockArslan
{
	public class LazySingleton<T> : MonoBehaviour where T : Component
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if ((Object)instance == (Object)null)
				{
					instance = UnityEngine.Object.FindObjectOfType<T>();
				}
				return instance;
			}
		}
	}
}
