﻿using System.Collections;
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
        owner.animationComponent.PlayAnimation("Steal");

        yield return new WaitForSecondsRealtime(
                    owner.animationComponent.Animator.GetCurrentAnimatorStateInfo(0).length +
                    owner.animationComponent.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                    );

        /*var time = Time.timeSinceLevelLoad;
        Debug.Log(time + " health bar " + settings.HealthBar.gameObject);*/

        LevelManager.Instance.LoseCrop(owner.stoleValue);
        owner.HasLoot(true);

        // TODO add loot

        yield return 0;
    }
}
