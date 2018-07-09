using UnityEngine;

public class CornBag : Pickable
{
    [Header("CornBag Settings")]
    [SerializeField]
    protected int _cornGain;

    public override void InitializePickableValue(int _value)
    {
        _cornGain = _value;

        return;
    }

    protected override void Pick(Character _whoPicks)
    {
        EventManager.TriggerEvent<PickEvent>(new PickEvent(PickEventType.CornBag, _whoPicks, _cornGain));

        base.Pick(_whoPicks);

        return;
    }
}
