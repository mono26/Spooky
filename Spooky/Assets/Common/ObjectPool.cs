using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public bool combineObjectPools = false;

    protected GameObject poolContainer;

    protected virtual void Awake()
    {
        instance = this;
        FillPoolWithObjects();
    }

    protected virtual void CreatePoolContainer()
    {
        if(combineObjectPools == false)
        {
            poolContainer = new GameObject(GetPoolName());
            return;
        }
        else
        {
            GameObject _objectPool = GameObject.Find(GetPoolName());
            if(poolContainer != null)
            {
                poolContainer = _objectPool;
            }
            else
            {
                poolContainer = new GameObject(GetPoolName());
            }
        }
    }

    protected virtual string GetPoolName()
    {
        return ("[ObjectPool] " + this.name);
    }

    protected virtual void FillPoolWithObjects()
    {
        return;
    }

    public virtual GameObject GetObjectFromPool()
    {
        return null;
    }
}
