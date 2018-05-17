using System.Collections;
using UnityEngine;

public class StealCorn : CharacterAction
{
    protected Enemy enemyCharacter;

    protected override void Awake()
    {
        base.Awake();

        enemyCharacter = GetComponent<Enemy>();
        target = LevelManager.Instance.GetRandomHousePoint();
    }

    public override void ExecuteAction()
    {
        if (lastExecute + cooldown < Time.timeSinceLevelLoad)
        {
            StartCoroutine(Steal());
            return;
        }
        else return;
    }

    private IEnumerator Steal()
    {
        if (enemyCharacter != null)
        {
            enemyCharacter.ExecuteAction(true);
            yield return 0;
        }

        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + 0.15f
                    );

        //LevelManager.Instance.UiManager.LoseCrop(owner.stoleValue);
        SetLasActionExecuteToActualTimeInLevel();

        // Stop the action executiong because the animation has already end.
        if (enemyCharacter != null)
        {
            enemyCharacter.ExecuteAction(false);
            enemyCharacter.ChangeCurrentAction(GetComponent<EscapeWithCorn>());
            enemyCharacter.GetComponent<EnemyMovement>().StopEnemy(false);
            yield return 0;
        }

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
