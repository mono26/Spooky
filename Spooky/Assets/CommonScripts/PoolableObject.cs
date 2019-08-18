using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    // Value for proyectile mostly, enemies can have a value of 0
    private Transform parentContainer;
    public float lifeTime = 10;

    public delegate void OnSpawnCompleteDelegate();
    public event OnSpawnCompleteDelegate OnSpawnComplete;
    public delegate void OnReleaseDelegate();
    public event OnReleaseDelegate OnRelease;

    protected virtual void OnEnable()
    {
        OnSpawnComplete += InvokeRelease;
        return;
    }

    protected virtual void OnDisable()
    {
        CancelInvoke("Release");
        return;
    }

    public void Release()
    {
        if (OnRelease != null)
            OnRelease();

        transform.SetParent(parentContainer);
        gameObject.SetActive(false);
    }

    public void OnSpawnCompleted()
    {
        if (OnSpawnComplete != null)
            OnSpawnComplete();
    }

    public void SetParentContainer(Transform _parent)
    {
        parentContainer = _parent;
        return;
    }

    protected void InvokeRelease()
    {
        if (lifeTime > 0)
            Invoke("Release", lifeTime);
    }
}
