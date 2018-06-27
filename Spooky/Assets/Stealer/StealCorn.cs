using System.Collections;
using UnityEngine;

public class StealCorn : CharacterAction, EventHandler<CharacterEvent>
{
    [Header("Especific action values.")]
    [SerializeField]
    protected Pickable drop;
    [SerializeField]
    protected int maxStealValue = 30;
    [SerializeField]
    protected int minStealValue = 13;

    protected int stoleValue;
    protected bool hasDrop;

    protected override void Start()
    {
        base.Start();

        target = LevelManager.Instance.GetRandomHousePoint();

        return;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        hasDrop = false;
        stoleValue = 0;

        return;
    }

    protected override IEnumerator Action()
    {
        VisualEffects.CreateVisualEffect(actionVfx, character.transform);

        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        stoleValue = Mathf.Abs(Random.Range(minStealValue, maxStealValue));
        LevelManager.Instance.LoseCrop(stoleValue);
        hasDrop = true;

        yield break;
    }

    public override void OnEvent(CharacterEvent _characterEvent)
    {
        base.OnEvent(_characterEvent);

        if(character.Equals(_characterEvent.character) == true && _characterEvent.type == CharacterEventType.Death)
        {
            if(hasDrop == true)
            {
                Pickable _drop = Instantiate(drop, transform.position, transform.rotation);
                _drop.GetComponent<Pickable>().InitializePickableValue(stoleValue);
            }
        }
    }
}
