using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StackTheBlockArslan
{
	public class ObjectPooler : MonoBehaviour
	{
		public List<PoolItem> itemsToPool;

		public List<List<GameObject>> pooledItemsList = new List<List<GameObject>>();
		public string SceneName;

		public ShapeSwitcher.ShapeType ShapeType;

		public static ObjectPooler Instance;
		
		public void SetShape()
		{
            ShapeType = (ShapeSwitcher.ShapeType)Random.Range
				(0, System.Enum.GetValues(typeof(ShapeSwitcher.ShapeType)).Length);
        }

        public void gameover()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene(SceneName);
        }

        private void Awake()
		{
			if(Instance!)
			{
                Instance = this;
            }
            CreateObjectPool();
			SetShape();

        }

		public void CreateObjectPool()
		{
			foreach (PoolItem item in itemsToPool)
			{
				List<GameObject> list = new List<GameObject>();
				for (int i = 0; i < item.amountToPool; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(item.objectToPool);
					gameObject.SetActive(value: false);
					list.Add(gameObject);
				}
				pooledItemsList.Add(list);
			}
		}

		public GameObject GetPooledObject(string itemName, bool activeState = false)
		{
			int num = itemsToPool.FindIndex((PoolItem x) => x.itemName == itemName);
			if (num == -1)
			{
				UnityEngine.Debug.LogError("Item with this name not found. Compare the assigned name and the name being passed");
				return null;
			}
			for (int i = 0; i < pooledItemsList[num].Count; i++)
			{
				if (!pooledItemsList[num][i].activeInHierarchy)
				{
					pooledItemsList[num][i].SetActive(activeState);
					return pooledItemsList[num][i];
				}
			}
			if (itemsToPool[num].shouldExpand)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(itemsToPool[num].objectToPool);
				gameObject.SetActive(activeState);
				pooledItemsList[num].Add(gameObject);
				return gameObject;
			}
			return null;
		}

		public GameObject GetPooledObject(ObjectPoolItems item, bool activeState = false)
		{
			return GetPooledObject(item.ToString(), activeState);
		}

		public GameObject GetPooledObject(string itemName, Vector3 pos, bool activeState = false)
		{
			GameObject pooledObject = GetPooledObject(itemName, activeState);
			pooledObject.transform.position = pos;
			return pooledObject;
		}

		public GameObject GetPooledObject(ObjectPoolItems itemName, Vector3 pos, bool activeState = false)
		{
			return GetPooledObject(itemName.ToString(), pos, activeState);
		}

		public GameObject GetPooledObject(string itemName, Transform posRot, bool activeState = false)
		{
			GameObject pooledObject = GetPooledObject(itemName, activeState);
			pooledObject.transform.position = posRot.transform.position;
			pooledObject.transform.rotation = posRot.transform.rotation;
			return pooledObject;
		}

		public GameObject GetPooledObject(ObjectPoolItems itemName, Transform posRot, bool activeState = false)
		{
			return GetPooledObject(itemName.ToString(), posRot, activeState);
		}

		public void disablePooled(string itemName)
		{
			int index = itemsToPool.FindIndex((PoolItem x) => x.itemName == itemName);
			for (int i = 0; i < pooledItemsList[index].Count; i++)
			{
				if (pooledItemsList[index][i].activeInHierarchy)
				{
					pooledItemsList[index][i].SetActive(value: false);
				}
			}
		}

		public void disableAllPooled()
		{
			for (int i = 0; i < pooledItemsList.Count; i++)
			{
				for (int j = 0; j < pooledItemsList[i].Count; j++)
				{
					if (pooledItemsList[i][j].activeInHierarchy)
					{
						pooledItemsList[i][j].SetActive(value: false);
					}
				}
			}
		}
	}
}
