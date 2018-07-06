using UnityEngine;

public enum PickEventType { EnemySoul, CornBag}

public class PickEvent : SpookyCrowEvent
{
    public PickEventType type;
    public Character whoPicks;
    public int pickValue;

    public PickEvent(PickEventType _type, Character _whoPicks, int _pickValue)
    {
        type = _type;
        whoPicks = _whoPicks;
        pickValue = _pickValue;
    }
}

public class Pickable : MonoBehaviour
{
    [Header("Pickable Settings")]
    [SerializeField]
    protected GameObject pickVfx;

    protected virtual void Pick(Character _whoPicks)
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
            Pick(_collider.GetComponent<Character>());
        }

        return;
    }
}
