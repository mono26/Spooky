using UnityEngine;

public enum CharacterEventType { ExecuteAction, FinishExecute, Death, Respawn }

public class CharacterEvent : SpookyCrowEvent
{
    public CharacterEventType type;
    public Character character;

    public CharacterEvent(CharacterEventType _type, Character _character)
    {
        type = _type;
        character = _character;
    }
}

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Character : MonoBehaviour, EventHandler<CharacterEvent>, EventHandler<FightCloudEvent>
{
    public enum CharacterState { Idle, Moving, ExecutingAction, Dead }
    public enum CharacterType { Player, AI}
    public enum InitialFacingDirection { Right, Left}

    [SerializeField]
    private string characterID = "";
    public string CharacterID { get { return characterID; } }
    public InputManager CharacterInput { get; protected set; }
    [SerializeField]
    public StateMachine<CharacterState> characterStateMachine;
    [SerializeField]
    private InitialFacingDirection initialDirection = InitialFacingDirection.Right;
    public bool IsExecutingAction { get; protected set; }
    [SerializeField]
    private CharacterType type = CharacterType.AI;
    public CharacterType Type { get { return type; } }

    // Obligatory Components
    [SerializeField]
    private Animator characterAnimator;
    public Animator CharacterAnimator { get { return characterAnimator; } }
    [SerializeField]
    private AudioSource characterAudioSource;
    public AudioSource CharacterAudioSource { get { return characterAudioSource; } }
    [SerializeField]
    private BoxCollider characterCollider;
    public BoxCollider CharacterCollider { get { return characterCollider; } }
    [SerializeField]
    private SpriteRenderer characterSprite;
    public SpriteRenderer CharacterSprite { get { return characterSprite; } }
    [SerializeField]
    private Transform characterTransform;
    public Transform CharacterTransform { get { return characterTransform; } }
    [SerializeField]
    private Rigidbody characterBody;
    public Rigidbody CharacterBody { get { return characterBody; } }

    protected CharacterComponent[] characterComponents;
    [SerializeField]
    private CharacterAction currentAction;
    public CharacterAction CurrentAction { get { return currentAction; } }
    protected CharacterAction[] characterActions;

    public bool IsFacingRightDirection { get; protected set; }

    protected virtual void Awake()
    {
        if(characterAnimator == null)
            characterAnimator = GetComponent<Animator>();
        if (characterAudioSource == null)
            characterAudioSource = GetComponent<AudioSource>();
        if (characterBody == null)
            characterBody = GetComponent<Rigidbody>();
        if (characterCollider == null)
            characterCollider = GetComponent<BoxCollider>();
        if (characterSprite == null)
            characterSprite = GetComponent<SpriteRenderer>();
        if (characterTransform == null)
            characterTransform = GetComponent<Transform>();

        characterComponents = GetComponents<CharacterComponent>();
        characterStateMachine = new StateMachine<CharacterState>();

        if (initialDirection == InitialFacingDirection.Left)
        {
            IsFacingRightDirection = false;    
        }
        else if (initialDirection == InitialFacingDirection.Right)
        {
            IsFacingRightDirection = true;
        }

        LinkCharacterInput();
    }

    protected virtual void Update()
    {
        if(characterComponents != null)
        {
            foreach (CharacterComponent component in characterComponents)
            {
                component.EveryFrame();
            }
        }

        if(currentAction != null)
        {
            if (currentAction.IsTargetInRange() && !IsExecutingAction)
                StartCoroutine(currentAction.ExecuteAction());
        }
        else if(currentAction == null && characterActions != null)
        {
            foreach (CharacterAction action in characterComponents)
            {
                if (!action.IsInCooldown())
                {
                    ChangeCurrentAction(action);
                    break;
                }
            }
        }

        UpdateAnimator();
    }

    protected virtual void FixedUpdate()
    {
        foreach (CharacterComponent component in characterComponents)
        {
            component.FixedFrame();
        }
    }

    protected virtual void LateUpdate()
    {
        foreach (CharacterComponent component in characterComponents)
        {
            component.LateFrame();
        }
    }

    private void LinkCharacterInput()
    {
        if (type == CharacterType.Player)
        {
            if (!string.IsNullOrEmpty(characterID))
            {
                CharacterInput = null;
                InputManager[] foundInputManagers = FindObjectsOfType(typeof(InputManager)) as InputManager[];
                foreach (InputManager foundInputManager in foundInputManagers)
                {
                    if (foundInputManager.playerID == characterID)
                    {
                        CharacterInput = foundInputManager;
                        return;
                    }
                }
            }
        }
        else return;
    }

    protected void ActivateCollider(bool _activate)
    {
        characterCollider.enabled = _activate;
        return;
    }

    protected void UpdateAnimator()
    {
        if(characterAnimator != null)
        {
            characterAnimator.SetBoolWithParameterCheck(
                "Idle", AnimatorControllerParameterType.Bool, (characterStateMachine.CurrentState == CharacterState.Idle)
                );
            characterAnimator.SetBoolWithParameterCheck(
                "Dead", AnimatorControllerParameterType.Bool, (characterStateMachine.CurrentState == CharacterState.Dead)
                );
            characterAnimator.SetBoolWithParameterCheck(
                "Moving", AnimatorControllerParameterType.Bool, (characterStateMachine.CurrentState == CharacterState.Moving)
                );
            characterAnimator.SetBoolWithParameterCheck(
                "Action", AnimatorControllerParameterType.Bool, (characterStateMachine.CurrentState == CharacterState.ExecutingAction)
                );
        }
    }

    public void Flip()
    {
        if (characterSprite != null)
        {
            characterSprite.flipX = !characterSprite.flipX;
        }
        IsFacingRightDirection = !IsFacingRightDirection;
        return;
    }

    protected void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
        return;
    }

    public void ChangeCurrentAction(CharacterAction _newAction)
    {
        currentAction = _newAction;
        return;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        if(characterID.Equals("Spooky"))
        {
            if(_collider.CompareTag("EnemyMeleeCollider"))
            {
                FightCloud.Instance.PrepareFight(this, _collider.GetComponentInParent<Character>());
            }
        }
        return;
    }

    public void OnEvent(CharacterEvent _enemyEvent)
    {
        if (_enemyEvent.character.Equals(this))
        {
            if (_enemyEvent.type == CharacterEventType.ExecuteAction)
                ExecuteAction(true);
            if (_enemyEvent.type == CharacterEventType.FinishExecute)
                ExecuteAction(false);
        }
        return;
    }

    public void OnEvent(FightCloudEvent _fightCloudEvent)
    {
        if (CharacterID == "Attacker")
        {
            if (_fightCloudEvent.enemy.Equals(this))
            {
                if (_fightCloudEvent.type == FightCloudEventType.StartFight)
                    EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, this));
                if (_fightCloudEvent.type == FightCloudEventType.StartFight)
                    EventManager.TriggerEvent<MovementEvent>(new MovementEvent(MovementEventType.Stop, this));
            }
        }
        return;
    }
}
