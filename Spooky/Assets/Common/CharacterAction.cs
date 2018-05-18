using System;
using UnityEngine;

public abstract class CharacterAction : CharacterComponent 
{
    [SerializeField]
    protected Transform target;
    public Transform Target { get { return target; } }
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected AudioClip actionSfx;
    [SerializeField]
    protected float delayAfterAnimationIsFinished = 0.15f;

    protected float lastExecute;

    protected override void Awake()
    {
        base.Awake();

        // Because if its set to 0 the enemy wont be able to execute the ability
        // Until the timesinceLevelLoad is greatter that the cooldown
        lastExecute = Time.timeSinceLevelLoad - cooldown;
    }

    public virtual void ExecuteAction()
    {

    }

    protected virtual void PlaySfx()
    {

    }

    protected void SetLasActionExecuteToActualTimeInLevel()
    {
        lastExecute = Time.timeSinceLevelLoad;
        return;
    }

    public bool IsTargetInRange()
    {
        float distance = Vector3.Distance(character.CharacterTransform.position, target.position);
        if (distance <= range)
        {
            return true;
        }
        else return false;
    }

    public void SetTarget(Transform _newTarget)
    {
        target = _newTarget;
        return;
    }
}
