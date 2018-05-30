using System.Collections;
using UnityEngine;

public class MeleeAttack : CharacterAction
{
    [SerializeField]
    protected Enemy enemyCharacter;
    [SerializeField]
    protected BoxCollider meleeCollider;

    protected override void Awake()
    {
        base.Awake();

        if (enemyCharacter == null)
            enemyCharacter = GetComponent<Enemy>();

        target = LevelManager.Instance.Spooky;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        meleeCollider.gameObject.SetActive(false);
    }

    protected override IEnumerator Action()
    {
        EventManager.TriggerEvent(new EnemyEvent(EnemyEventType.ExecuteAction, enemyCharacter));
        yield return 0;

        // We want to enable the collider before the animation ends
        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length / 2
                    );

        Vector3 directionToTarget = target.position - character.CharacterTransform.position;
        directionToTarget = directionToTarget.normalized;
        meleeCollider.transform.position = character.CharacterTransform.position + directionToTarget * range;
        meleeCollider.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(
            character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length / 2 + delayAfterAnimationIsFinished
            );

        //LevelManager.Instance.UiManager.LoseCrop(owner.stoleValue);
        SetLasActionExecuteToActualTimeInLevel();

        // Stop the action executiong because the animation has already end.
        EventManager.TriggerEvent(new EnemyEvent(EnemyEventType.FinishExecute, enemyCharacter));
        meleeCollider.gameObject.SetActive(false);
        yield break;
    }

    protected override void UpdateState()
    {
        if (enemyCharacter.IsExecutingAction == true)
        {
            character.characterStateMachine.ChangeState(Character.CharacterState.ExecutingAction);
            return;
        }
        else return;
    }
}
