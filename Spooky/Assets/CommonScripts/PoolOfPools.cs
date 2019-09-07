using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PoolData
{
    public PoolableObject prefab;
    public int initialSize;
}

public class PoolOfPools : MonoBehaviour
{
    [SerializeField] PoolData[] poolsData = new PoolData[0];
    Dictionary<string, List<PoolableObject>> pools = new Dictionary<string, List<PoolableObject>>();

#region Unity Functions
    void Awake() 
    {
		// Subscribe to the pool manager and create all the object collections.
        PoolsManager.SubscribePool(this);
        FillPool();    
    }

	// Usually occurs when the scene changes.
    void OnDestroy() 
    {
		// Must do this because the manager is static and we don't want to keep old pools data.
        PoolsManager.UnSubscribePool(this);
    }
#endregion

    void FillPool()
    {
        for (int i = 0; i < poolsData.Length; i++)
        {
            if (pools.ContainsKey(poolsData[i].prefab.GetName))
            {
                continue;
            }

            var pool = new List<PoolableObject>(poolsData[i].initialSize);
            for (int j = 0; j < poolsData[i].initialSize; j++)
            {
                var newObject = InstantiatePrefab(poolsData[i].prefab);

                if (!newObject)
                {
                    continue;
                }

				newObject.SetObjectPosition = new Vector3(9999, 9999, 9999);
                newObject.SetPoolableState(PoolableStates.InsidePool);
                pool.Add(newObject);
            }

            pools.Add(poolsData[i].prefab.GetName, pool);
        }
    }

    PoolableObject InstantiatePrefab(PoolableObject prefab)
    {
        if (prefab == null)
        {
            return null;
        }

        var newGameObject = Instantiate(prefab);
        newGameObject.transform.SetParent(transform, false);
        return newGameObject;
    }

    /// <summary>
    /// Get one object from the pool.
    /// </summary>
    /// <param name="poolKey"></param>
    /// <returns></returns>
    public PoolableObject GetGameObjectFromPool(string poolKey)
    {
        PoolableObject objectToGet = null;
        List<PoolableObject> pool = null;
        if (pools.TryGetValue(poolKey, out pool))
        {
            if (pool.Count == 0)
            {
                objectToGet = InstantiatePrefab(GetPrefabFromKey(poolKey));
            }
            else
            {
                objectToGet = pool[pool.Count - 1];
                pool.Remove(objectToGet);
            }

            objectToGet.SetPoolableState(PoolableStates.OutOfPool);
            objectToGet.transform.SetParent(null);
        }

        return objectToGet;
    }

    public bool ContainsPool(string key)
    {
        bool contains = false;
        if (pools.ContainsKey(key))
        {
            contains = true;
        }

        return contains;
    }

    PoolableObject GetPrefabFromKey(string poolKey)
    {
        PoolableObject prefab = null;
        for (int i = 0; i < poolsData.Length; i++)
        {
            if (poolsData[i].prefab.GetName.Equals(poolKey))
            {
                prefab = poolsData[i].prefab;
            }
        }

        return prefab;
    }

    public void ReturnGameObjectToPool(PoolableObject gameObjectToReturn)
    {
        var poolKey = gameObjectToReturn.GetName.Replace("(Clone)", "");
        List<PoolableObject> pool = null;
        if (pools.TryGetValue(poolKey, out pool))
        {
            if (!pool.Contains(gameObjectToReturn))
            {
                gameObjectToReturn.transform.SetParent(transform);
				gameObjectToReturn.SetObjectPosition = new Vector3(9999, 9999, 9999);
				gameObjectToReturn.SetPoolableState(PoolableStates.InsidePool);
				pool.Add(gameObjectToReturn);
            }
        }
    }
}

public static class PoolsManager
{
    public static List<PoolOfPools> pools = new List<PoolOfPools>();

    public static void SubscribePool(PoolOfPools poolToSubscribe)
    {
        pools.Add(poolToSubscribe);
    }

    public static void UnSubscribePool(PoolOfPools poolToUnSubscribe)
    {
        pools.Remove(poolToUnSubscribe);

        if (pools.Count.Equals(0))
        {
            pools.Clear();
        }
    }

    /// <summary>
    /// Gets a GameObject from a Pool.
    /// </summary>
    /// <param name="goToPool"></param>
    /// <param name="instantiateFallback">if there is no Pool tha contains the GameObject instantiate.</param>
    /// <returns></returns>
    public static PoolableObject GetObjectFromPools(PoolableObject goToPool, bool instantiateFallback = true)
    {
        if (!goToPool)
        {
            return null;
        }

        PoolableObject goFromPool = null;
        string poolKey = goToPool.GetName;
        if (poolKey.Contains("(Clone)"))
        {
            poolKey = poolKey.Replace("(Clone)", "");
        }
        for (int i = 0; i < pools.Count; i++)
        {
            if (!pools[i].ContainsPool(poolKey))
            {
                continue;
            }

            goFromPool = pools[i].GetGameObjectFromPool(poolKey);
        }

        if (!goFromPool && instantiateFallback)
        {
            Debug.Log("There is no pool with that key: "+ poolKey + ", instantiating");
            goFromPool = GameObject.Instantiate(goToPool);
        }

        return goFromPool;
    }

    public static void ReturnObjectToPools(PoolableObject objectToReturn, bool destroyFallback = true)
    {
        if (!objectToReturn)
        {
            return;
        }

        var poolKey = objectToReturn.GetName.Replace("(Clone)", "");
        bool returnedToPool = false;
        for (int i = 0; i < pools.Count; i++)
        {
            if (!pools[i].ContainsPool(poolKey))
            {
                continue;
            }

            pools[i].ReturnGameObjectToPool(objectToReturn);
            returnedToPool = true;
        }

        if (!returnedToPool && destroyFallback)
        {
            /// In case there is no pool to return the object, destroy it
            GameObject.Destroy(objectToReturn.gameObject);
            // Debug.Log("There is no posible pool to return, destroying: " + goToReturn.name);
        }
        else if (!returnedToPool && !destroyFallback)
        {
            objectToReturn.gameObject.SetActive(false);
            // Debug.Log("There is no posible pool to return, deactivating: " + goToReturn.name);
        }
    }
}
