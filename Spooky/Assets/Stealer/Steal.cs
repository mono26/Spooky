using System.Collections;
using UnityEngine;

public class Steal : ICloseRangeAttack

{
    protected Stealer owner;

    public Steal(Stealer _enemy)
    {
        owner = _enemy;
    }

    public void CloseAttack()
    {
        owner.CastAbility(owner.StartCoroutine(StealCrop()));
        return;
    }

    private IEnumerator StealCrop()
    {
        //TODO execute animation.
        LevelManager.Instance.LoseCrop(owner.stoleValue);
        owner.HasLoot(true);
        yield return 0;
    }
}
