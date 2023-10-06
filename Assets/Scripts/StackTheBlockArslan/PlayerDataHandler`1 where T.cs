using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace StackTheBlockArslan
{
	[Serializable]
	public class PlayerDataHandler<T> where T : new()
	{
		protected static T instance;

		private static string FilePath = Application.persistentDataPath + "/gameData.sv";

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					Create();
				}
				return instance;
			}
		}

		public void SaveData()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Create(FilePath);
			binaryFormatter.Serialize(fileStream, instance);
			fileStream.Close();
		}

		public static void Create()
		{
			if (File.Exists(FilePath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(FilePath, FileMode.Open);
				instance = (T)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();
			}
			else
			{
				instance = new T();
			}
		}

		public static void Clear()
		{
			try
			{
				File.Delete(FilePath);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			instance = new T();
		}
	}
}
