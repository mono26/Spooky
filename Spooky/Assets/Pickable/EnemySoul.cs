using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoul : Pickable
{
    [Header("CornBag Settings")]
    [SerializeField]
    protected int moneyGain;

    public override void InitializePickableValue(int _value)
    {
        moneyGain = _value;

        return;
    }

    protected override void Pick(Character _whoPicks)
    {
        EventManager.TriggerEvent<PickEvent>(new PickEvent(PickEventType.EnemySoul, _whoPicks, moneyGain));

        base.Pick(_whoPicks);

        return;
    }
}
