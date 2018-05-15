using UnityEngine;

public abstract class CharacterAction : CharacterComponent 
{
    [SerializeField]
    protected float actionCooldown;
    protected float lastActionExecute;

    protected AudioClip actionSfx;

    protected override void Awake()
    {
        base.Awake();

        // Because if its set to 0 the enemy wont be able to execute the ability
        // Until the timesinceLevelLoad is greatter that the cooldown
        lastActionExecute = Time.timeSinceLevelLoad - actionCooldown;
    }

    public virtual void ExecuteAction()
    {

    }

    protected virtual void PlaySfx()
    {

    }

    protected void SetLasActionExecuteToActualTimeInLevel()
    {
        lastActionExecute = Time.timeSinceLevelLoad;
        return;
    }
}
