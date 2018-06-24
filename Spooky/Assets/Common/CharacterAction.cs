using System.Collections;
using UnityEngine;

public abstract class CharacterAction : CharacterComponent, EventHandler<DetectEvent>
{
    [Header("Action settings")]
    [SerializeField]
    protected AudioClip actionSfx = null;
    [SerializeField]
    protected bool actionStopsMovement = true;
    [SerializeField]
    protected GameObject actionVfx = null;
    [SerializeField]
    protected float cooldown = 0;
    [SerializeField]
    protected float delayAfterAnimationIsFinished = 0.15f;
    [SerializeField]
    protected bool needsTargetToExecute = true;
    [SerializeField]
    protected CharacterAction nextAction = null;
    [SerializeField]
    protected float range = 0;

    [Header("Editor debugging")]
    [SerializeField]
    protected AICharacter aICharacter = null;
    protected float lastExecute;
    [SerializeField]
    protected Vector3 lastTargetPosition;
    [SerializeField]
    protected Transform target = null;
    public Transform Target { get { return target; } }

    protected override void Awake()
    {
        base.Awake();

        if(aICharacter == null)
            aICharacter = character as AICharacter;
        // Because if its set to 0 the enemy wont be able to execute the ability
        // Until the timesinceLevelLoad is greatter that the cooldown
        lastExecute = Time.timeSinceLevelLoad - cooldown;
    }

    protected virtual void Start()
    {
        if(cooldown.Equals(0) == true && character.GetComponent<StatsComponent>())
        {
            cooldown = 1 / character.GetComponent<StatsComponent>().AttacksPerSecond;
        }
        else if (cooldown.Equals(0) == false)
        {
            cooldown = 1 / cooldown;
        }
        if (range.Equals(0) == true && character.GetComponent<EnemyDetect>())
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
            if (actionStopsMovement)
                EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, character));

            EventManager.TriggerEvent(new CharacterEvent(CharacterEventType.ExecuteAction, character));
            yield return null;

            yield return Action();

            EventManager.TriggerEvent(new CharacterEvent(CharacterEventType.FinishExecute, character));
            SetLasActionExecuteToActualTimeInLevel();
            yield return null;

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
        if (needsTargetToExecute == true)
        {
            if (target != null)
            {
                float distance = Vector3.Distance(character.transform.position, target.position);
                if (distance <= range)
                {
                    return true;
                }
                else return false;
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
        if(aICharacter != null)
        {
            if (nextAction != null)
                aICharacter.ChangeCurrentAction(nextAction);
            else
                aICharacter.ChangeCurrentAction(null);
        }
        return;
    }

    public bool IsInCooldown()
    {
        if (lastExecute + cooldown < Time.timeSinceLevelLoad)
            return false;
        else return true;
    }

    private void SetLastPositionOfTarget(Vector3 _position)
    {
        lastTargetPosition = _position;
        return;
    }

    protected Vector3 GetTargetDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (target != null)
        {
            _direction = (target.position - character.transform.position).normalized;
            _direction.y = _direction.z;
            _direction.z = 0;
        }

        return _direction;
    }

    protected Vector3 GetTargetDirection(Vector3 _position)
    {
        Vector3 _direction = Vector3.zero;
        if (target != null)
        {
            _direction = (_position - character.transform.position).normalized;
            _direction.y = _direction.z;
            _direction.z = 0;
        }

        return _direction;
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
                SetLastPositionOfTarget(_detectEvent.target.position);
            }
            return;
        }
        else return;
    }
}
