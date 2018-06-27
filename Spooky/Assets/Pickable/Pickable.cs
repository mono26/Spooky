using UnityEngine;

public class Pickable : MonoBehaviour
{
    [Header("Pickable Settings")]
    [SerializeField]
    protected GameObject pickVfx;

    protected virtual void Pick(GameObject _whoPicks)
    {
        VisualEffects.CreateVisualEffect(pickVfx, transform);

        Destroy(gameObject);

        return;
    }

    public virtual void InitializePickableValue(int _value)
    {
        return;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Spooky") == true)
        {
            Pick(_collider.gameObject);
        }
        return;
    }
}
