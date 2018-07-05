using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoul : Pickable
{
    [Header("CornBag Settings")]
    [SerializeField]
    protected int _moneyGain;

    protected override void Pick(GameObject _whoPicks)
    {
        LevelManager.Instance.GiveMoney(_moneyGain);
        LevelUIManager.Instance.UpdateMoneyDisplay();

        base.Pick(_whoPicks);

        return;
    }

    public override void InitializePickableValue(int _value)
    {
        _moneyGain = _value;

        return;
    }
}
