using System.Collections;
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
    protected float delayAfterAnimationIsFinished = 0.15f;
    [SerializeField]
    protected bool actionStopsMovement = true;
    [SerializeField]
    protected bool needsTargetToExecute = true;

    protected float lastExecute;

    protected override void Awake()
    {
        base.Awake();

        // Because if its set to 0 the enemy wont be able to execute the ability
        // Until the timesinceLevelLoad is greatter that the cooldown
        lastExecute = Time.timeSinceLevelLoad - cooldown;
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
        if (lastExecute + cooldown < Time.timeSinceLevelLoad)
        {
            if (actionStopsMovement)
                EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, character));

            yield return Action();

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
