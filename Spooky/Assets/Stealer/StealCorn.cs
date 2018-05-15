using System.Collections;
using UnityEngine;

public class StealCorn : CharacterAction
{
    protected Enemy enemyCharacter;

    protected override void Awake()
    {
        base.Awake();

        enemyCharacter = GetComponent<Enemy>();
    }

    public override void ExecuteAction()
    {
        Debug.Log("trying to execute steal crop");
        if (lastActionExecute + actionCooldown < Time.timeSinceLevelLoad)
        {
            Debug.Log("Executing steal crop");
            StartCoroutine(StealCrop());
            return;
        }
        else return;
    }

    private IEnumerator StealCrop()
    {
        if (enemyCharacter != null)
        {
            enemyCharacter.ExecuteAction(true);
            yield return 0;
        }

        /*yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length
                    );*/

        SetLasActionExecuteToActualTimeInLevel();

        // Stop the action executiong because the animation has already end.
        if (enemyCharacter != null)
        {
            enemyCharacter.ExecuteAction(false);
            enemyCharacter.ChangeEnemyTarget(LevelManager.Instance.GetRandomRunawayPoint());
            enemyCharacter.GetComponent<EnemyMovement>().StopEnemy(false);
            yield return 0;
        }

        //LevelManager.Instance.UiManager.LoseCrop(owner.stoleValue);

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
