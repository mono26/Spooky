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
    public CharacterAction CurrentAction { get { return currentAction; } }

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
        if(characterStateMachine.CurrentState != CharacterState.Dead)
        {
            if (currentAction != null && currentAction.IsInCooldown() == false)
            {
                if (currentAction.IsTargetInRange() && IsExecutingAction == false)
                    StartCoroutine(currentAction.ExecuteAction());
            }
            else if (currentAction == null || currentAction.IsInCooldown() == true)
            {
                ChangeNonExecutableCurrentAction();
            }

            base.Update();
        }

        return;
    }

    public void ChangeCurrentAction(CharacterAction _newAction)
    {
        currentAction = _newAction;
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
        if (CharacterID.Equals("Attacker") == true)
        {
            if (_fightCloudEvent.type == FightCloudEventType.StartFight)
                EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, this));
            if (_fightCloudEvent.type == FightCloudEventType.EndFight)
                EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Move, this));
        }
        return;
    }
}
