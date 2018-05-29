using System.Collections;
using UnityEngine;

public class StealCorn : CharacterAction
{
    [SerializeField]
    protected Enemy enemyCharacter;

    protected override void Awake()
    {
        base.Awake();

        if(enemyCharacter == null)
            enemyCharacter = GetComponent<Enemy>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        target = LevelManager.Instance.GetRandomHousePoint();
    }

    protected override IEnumerator Action()
    {
        EventManager.TriggerEvent(new EnemyEvent(EnemyEventType.ExecuteAction, enemyCharacter));
        yield return 0;

        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        //LevelManager.Instance.UiManager.LoseCrop(owner.stoleValue);
        SetLasActionExecuteToActualTimeInLevel();

        // Stop the action executiong because the animation has already end.
        EventManager.TriggerEvent(new EnemyEvent(EnemyEventType.FinishExecute, enemyCharacter));
        yield return 0;

        if (enemyCharacter != null)
            enemyCharacter.ChangeCurrentAction(GetComponent<EscapeWithCorn>());

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
