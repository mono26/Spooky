using UnityEngine;

public class AICharacter : Character, EventHandler<FightCloudEvent>
{
    [Header("AI settings")]
    [SerializeField]
    private CharacterAction startingAction;
    protected CharacterAction[] characterActions;
    [SerializeField]
    protected int reward;
    public int Reward { get { return reward; } }

    [Header("For debugging in editor")]
    [SerializeField]
    private CharacterAction currentAction;
    public CharacterAction CurrentAction { get { return startingAction; } }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.AddListener<FightCloudEvent>(this);

        currentAction = startingAction;

        return;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventManager.RemoveListener<FightCloudEvent>(this);

        return;
    }

    protected override void Update()
    {
        if (startingAction != null && startingAction.IsInCooldown() == false)
        {
            if (startingAction.IsTargetInRange() && IsExecutingAction == false)
                StartCoroutine(startingAction.ExecuteAction());
        }
        else if (startingAction == null || startingAction.IsInCooldown() == true)
        {
            ChangeNonExecutableCurrentAction();
        }

        base.Update();

        return;
    }

    public void ChangeCurrentAction(CharacterAction _newAction)
    {
        startingAction = _newAction;
        return;
    }

    protected void ChangeNonExecutableCurrentAction()
    {
        if(characterActions != null)
        {
            foreach (CharacterAction action in characterActions)
            {
                if (action.IsInCooldown() == false)
                {
                    ChangeCurrentAction(action);
                    break;
                }
            }
        }
        return;
    }

    public virtual void OnEvent(FightCloudEvent _fightCloudEvent)
    {
        if (CharacterID == "Attacker")
        {
            if (_fightCloudEvent.enemy.Equals(this))
            {
                if (_fightCloudEvent.type == FightCloudEventType.StartFight)
                    EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, this));
                if (_fightCloudEvent.type == FightCloudEventType.EndFight)
                    EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Move, this));
            }
        }
        return;
    }

    public override void OnEvent(CharacterEvent _characterEvent)
    {
        if (_characterEvent.character.Equals(this))
        {
            if (_characterEvent.type == CharacterEventType.Death)
                LevelManager.Instance.GiveMoney(reward);
        }

        base.OnEvent(_characterEvent);

        return;
    }
}
