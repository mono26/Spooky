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
}
