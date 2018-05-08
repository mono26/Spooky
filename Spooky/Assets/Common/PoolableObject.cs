using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    // Value for proyectile mostly, enemies can have a value of 0
    private Transform parentContainer;
    public float lifeTime = 10;

    public delegate void OnSpawnCompleteDelegate();
    public event OnSpawnCompleteDelegate OnSpawnComplete;

    protected void OnDisable()
    {
        CancelInvoke("Release");
    }

    public void Release()
    {
        transform.SetParent(parentContainer);
        gameObject.SetActive(false);
    }

    protected void OnSpawnCompleted()
    {
        if (OnSpawnComplete != null)
            OnSpawnComplete();
    }

    public void SetParentContainer(Transform _parent)
    {
        parentContainer = _parent;
        return;
    }

    public void InvokeRelease()
    {
        if (lifeTime > 0)
            Invoke("Release", lifeTime);
    }
}
