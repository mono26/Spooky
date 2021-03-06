﻿using System.Collections;
using UnityEngine;

public class MeleeAttack : CharacterAction
{
    [SerializeField]
    protected BoxCollider meleeCollider;

    protected override void Start()
    {
        base.Start();

        if(character.CharacterID == "Attacker")
            target = LevelManager.Instance.Spooky;

        return;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        meleeCollider.gameObject.SetActive(false);
    }

    public override void EveryFrame()
    {
        // TODO double code block refacto to only face when the attack is executed.
        if (target != null)
        {
            if (target.position.x > character.transform.position.x)
            {
                if (!character.IsFacingRightDirection)
                    character.Flip();
            }
            // If it's negative, then we're facing left
            else if (target.position.x < character.transform.position.x)
            {
                if (character.IsFacingRightDirection)
                    character.Flip();
            }
        }

        base.EveryFrame();
    }

    protected override IEnumerator Action()
    {
        // We want to enable the collider before the animation ends
        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length / 2
                    );

        Vector3 directionToTarget;
        if (target != null)
            directionToTarget = GetTargetDirection();
        else
            directionToTarget = GetTargetDirection(lastTargetPosition);

        meleeCollider.transform.position = character.transform.position + directionToTarget * range;
        meleeCollider.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(
            character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length / 2 + delayAfterAnimationIsFinished
            );

        meleeCollider.gameObject.SetActive(false);

        yield break;
    }

    /*protected override void UpdateState()
    {
        if (character.characterStateMachine != null)
        {
            if (character != null && character.IsExecutingAction == true)
                character.characterStateMachine.ChangeState(Character.CharacterState.ExecutingAction);
        }
        return;
    }*/
}
