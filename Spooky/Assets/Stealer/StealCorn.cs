using System.Collections;
using UnityEngine;

public class StealCorn : CloseRangeAttack
{
    protected Stealer owner;

    public StealCorn(Stealer _enemy)
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
        owner.AnimationComponent.PlayAnimation("Steal");

        yield return new WaitForSecondsRealtime(
                    owner.AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length +
                    owner.AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                    );

        LevelManager.Instance.UiManager.LoseCrop(owner.stoleValue);
        owner.HasLoot(true);

        // TODO add loot

        yield return 0;
    }
}
