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
        VisualEffects.CreateVisualEffect(actionVfx, character.transform);

        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        LevelManager.Instance.LoseCrop((int)Mathf.Abs(Random.Range(minStealValue, stealValue)));

        yield break;
    }
}
