using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleObjectPool : ObjectPool
{
    public GameObject gameObjectToPool;

    public int startingPoolSize = 10;

    public bool poolCanIncreaseSize = true;

    protected List<GameObject> pool;

    public SingleObjectPool(GameObject _objectPrefab)
    {
        gameObjectToPool = _objectPrefab;
    }

    protected override void FillPoolWithObjects()
    {
        CreatePoolContainer();

        pool = new List<GameObject>();

        for (int i = 0; i < startingPoolSize; i++)
        {
            AddOneGameObjectToThePool();
        }
    }

    protected override string GetPoolName()
    {
        return ("[SingleObjectPool] " + this.name);
    }

    protected virtual GameObject AddOneGameObjectToThePool()
    {
        if (gameObjectToPool == null)
        {
            Debug.LogWarning("The " + gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
            return null;
        }
        GameObject newGameObject = (GameObject)Instantiate(gameObjectToPool);
        newGameObject.GetComponent<PoolableObject>().SetParentContainer(poolContainer.transform);
        newGameObject.gameObject.SetActive(false);
        newGameObject.transform.SetParent(poolContainer.transform);
        newGameObject.name = gameObjectToPool.name + "-" + pool.Count;
        pool.Add(newGameObject);
        return newGameObject;
    }

    public override GameObject GetObjectFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }		
        if (poolCanIncreaseSize)
        {
            return AddOneGameObjectToThePool();
        }
        return null;
    }
}
