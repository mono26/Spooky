using UnityEngine;

public class CornBag : Pickable
{
    [Header("CornBag Settings")]
    [SerializeField]
    protected int _cornGain;

    protected override void Pick(GameObject _whoPicks)
    {
        LevelManager.Instance.GiveCrop(_cornGain);
        LevelUIManager.Instance.UpdateCropUIBar();

        base.Pick(_whoPicks);

        return;
    }

    public override void InitializePickableValue(int _value)
    {
        _cornGain = _value;

        return;
    }
}
