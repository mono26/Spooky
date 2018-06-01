﻿using System.Collections;
using UnityEngine;

public abstract class CharacterAction : CharacterComponent, EventHandler<DetectEvent>
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
    protected GameObject actionVfx;
    [SerializeField]
    protected float delayAfterAnimationIsFinished = 0.15f;
    [SerializeField]
    protected bool actionStopsMovement = true;
    [SerializeField]
    protected bool needsTargetToExecute = true;
    [SerializeField]
    protected CharacterAction nextAction;

    protected float lastExecute;

    protected override void Awake()
    {
        base.Awake();

        // Because if its set to 0 the enemy wont be able to execute the ability
        // Until the timesinceLevelLoad is greatter that the cooldown
        lastExecute = Time.timeSinceLevelLoad - cooldown;
    }

    protected virtual void Start()
    {
        if(character.GetComponent<StatsComponent>())
        {
            cooldown = 1 / character.GetComponent<StatsComponent>().AttacksPerSecond;
        }
        if (character.GetComponent<EnemyDetect>())
        {
            range = character.GetComponent<EnemyDetect>().DetectionRange;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.AddListener<DetectEvent>(this);

        return;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventManager.RemoveListener<DetectEvent>(this);

        return;
    }

    public IEnumerator ExecuteAction()
    {
        if (IsInCooldown() == false)
        {
            Debug.Log("Executing Action");

            if (actionStopsMovement)
                EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, character));

            yield return Action();

            SetNextActionInCharacter();

            EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Move, character));
        }
        yield break;
    }

    protected virtual IEnumerator Action()
    {
        yield break;
    }

    protected virtual void PlaySfx()
    {
        SoundManager.Instance.PlaySfx(character.CharacterAudioSource, actionSfx);
        return;
    }

    protected virtual void PlayActionVfxEffect()
    {
        if (actionVfx != null)
        {
            Debug.Log("Instantiating Particle");
            Instantiate(actionVfx, transform.position, transform.rotation);
        }
        return;
    }

    protected void SetLasActionExecuteToActualTimeInLevel()
    {
        lastExecute = Time.timeSinceLevelLoad;
        return;
    }

    public bool IsTargetInRange()
    {
        if (needsTargetToExecute)
        {
            float distance = Vector3.Distance(character.CharacterTransform.position, target.position);
            if (distance <= range)
            {
                return true;
            }
            else return false;
        }
        else return true;
    }

    protected void SetTarget(Transform _newTarget)
    {
        target = _newTarget;
        return;
    }

    private void SetNextActionInCharacter()
    {
        if (nextAction != null)
            character.ChangeCurrentAction(nextAction);
        else
            character.ChangeCurrentAction(null);

        return;
    }

    public bool IsInCooldown()
    {
        if (lastExecute + cooldown < Time.timeSinceLevelLoad)
            return false;
        else return true;
    }

    public void OnEvent(DetectEvent _detectEvent)
    {
        if (_detectEvent.character.Equals(character))
        {
            if (_detectEvent.type == DetectEventType.TargetAcquired)
            {
                SetTarget(_detectEvent.target);
            }
            else if (_detectEvent.type == DetectEventType.TargetLost)
            {
                SetTarget(null);
            }
            return;
        }
        else return;
    }
}
