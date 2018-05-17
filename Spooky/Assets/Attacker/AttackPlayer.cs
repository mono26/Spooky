using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : CloseRangeAttack
{
    //protected Attacker owner;

    public void CloseAttack()
    {
        //owner.CastAbility(owner.StartCoroutine(MeleeAttackThePlayer()));
        return;
    }

    private IEnumerator MeleeAttackThePlayer()
    {
        /*owner.AnimationComponent.PlayAnimation("Steal");

        yield return new WaitForSecondsRealtime(
                    owner.AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length +
                    owner.AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                    );*/

        //LevelManager.Instance.UiManager.LoseCrop(owner.stoleValue);
        //owner.HasLoot(true);

        // TODO add loot

        yield return 0;
    }
}
