using System.Collections;
using UnityEngine;

public class StealCorn : CharacterAction
{
    [Header("Especific action values.")]
    [SerializeField]
    protected float stealValue = 30;
    [SerializeField]
    protected float minStealValue = 13;

    protected override void Start()
    {
        base.Start();

        target = LevelManager.Instance.GetRandomHousePoint();

        return;
    }

    protected override IEnumerator Action()
    {
        EventManager.TriggerEvent(new CharacterEvent(CharacterEventType.ExecuteAction, character));
        yield return 0;

        PlayActionVfxEffect();

        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        LevelManager.Instance.LoseCrop((int)Mathf.Abs(Random.Range(minStealValue, stealValue)));
        SetLasActionExecuteToActualTimeInLevel();

        // Stop the action executiong because the animation has already end.
        EventManager.TriggerEvent(new CharacterEvent(CharacterEventType.FinishExecute, character));
        yield return 0;

        yield break;
    }

    protected override void UpdateState()
    {
        if (character.characterStateMachine != null)
        {
            if (character.IsExecutingAction == true)
            {
                character.characterStateMachine.ChangeState(Character.CharacterState.ExecutingAction);

            }
        }
        return;
    }
}
