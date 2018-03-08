using System.Collections;
using UnityEngine;

public class Steal : ICloseRangeAttack

{
    protected Enemy owner;
    [SerializeField]
    protected int stoleValue;

    public Steal(Enemy _enemy, int _stoleValue)
    {
        owner = _enemy;
        stoleValue = _stoleValue;
    }

    public void CloseAttack()
    {
        owner.CastAbility(owner.StartCoroutine(StealCrop()));
        return;
    }

    private IEnumerator StealCrop()
    {
        //TODO execute animation.
        var time = Time.timeSinceLevelLoad;
        Debug.Log("{0} stealt crop" + time);
        LevelManager.Instance.LoseCrop(stoleValue);
        yield return 0;
    }
}
