using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler Instance;

	public ObjectsToPool[] objectsToPool;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	void Start()
	{
		foreach (ObjectsToPool o in objectsToPool)
		{
			o.pooledObjects = new List<GameObject>();
			for (int i = 0; i < o.amountToPool; i++)
			{
				GameObject obj = (GameObject)Instantiate(o.prefab);
				obj.SetActive(false);
				o.pooledObjects.Add(obj);
				obj.transform.parent = o.objectsParent.transform;
				obj.transform.position = o.objectsParent.transform.position;
			}

		}

	}

	public static GameObject GetPooledObject(int objectID)
    {
        for (int i = 0; i < Instance.objectsToPool[objectID].pooledObjects.Count; i++)
        {
            if (!Instance.objectsToPool[objectID].pooledObjects[i].activeInHierarchy)
            {
                return Instance.objectsToPool[objectID].pooledObjects[i];
            }
        }

		GameObject obj = Instantiate(Instance.objectsToPool[objectID].prefab);
		obj.SetActive(false);
		Instance.objectsToPool[objectID].pooledObjects.Add(obj);
		obj.transform.parent = Instance.objectsToPool[objectID].objectsParent.transform;
		obj.transform.position = Instance.objectsToPool[objectID].objectsParent.transform.position;
		return obj;
	}

	public static void ResetObjects(int objectID)
	{
		foreach (GameObject pooledObject in Instance.objectsToPool[objectID].pooledObjects)
		{
			pooledObject.transform.parent = Instance.objectsToPool[objectID].objectsParent.transform;
			pooledObject.transform.localPosition = Vector3.zero;
			pooledObject.SetActive(false);
		}
	}
}
[System.Serializable]
public class ObjectsToPool
{
	public string name;
	public GameObject prefab;
	public GameObject objectsParent;
	[Min(0)]
	public int amountToPool;
	public List<GameObject> pooledObjects;
}
