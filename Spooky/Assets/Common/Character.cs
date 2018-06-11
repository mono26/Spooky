using UnityEngine;

public enum CharacterEventType { ExecuteAction, FinishExecute, Death, Respawn, Release }

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

[RequireComponent(typeof(AudioSource), typeof(BoxCollider))]
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Character : MonoBehaviour, EventHandler<CharacterEvent>
{
    public enum CharacterState { Idle, Moving, ExecutingAction, Dead }
    public enum CharacterType { Player, AI}
    public enum InitialFacingDirection { Right, Left}

    [Header("Settings")]
    [SerializeField]
    private string characterID = "";
    public string CharacterID { get { return characterID; } }
    public InputManager CharacterInput { get; protected set; }
    [SerializeField]
    public StateMachine<CharacterState> characterStateMachine;
    [SerializeField]
    private bool isExecutingAction;
    public bool IsExecutingAction { get { return isExecutingAction; } }
    [SerializeField]
    private InitialFacingDirection initialDirection = InitialFacingDirection.Right;
    [SerializeField]
    private CharacterType type = CharacterType.AI;
    public CharacterType Type { get { return type; } }

    [Header("Components (Set in editor)")]
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

    protected virtual void OnEnable()
    {
        EventManager.AddListener<CharacterEvent>(this);

        return;
    }

    protected virtual void OnDisable()
    {
        EventManager.RemoveListener<CharacterEvent>(this);

        return;
    }

    protected virtual void Update()
    {
        //Debug.Log(this.gameObject + "Current state: " + characterStateMachine.currentState.ToString());

        if(characterComponents != null)
        {
            foreach (CharacterComponent component in characterComponents)
            {
                component.EveryFrame();
            }
        }

        UpdateAnimator();

        return;
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

        return;
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
        isExecutingAction = _cast;
        if (_cast == true)
            characterStateMachine.ChangeState(Character.CharacterState.ExecutingAction);
        else if (_cast == false)
            characterStateMachine.ChangeState(Character.CharacterState.Idle);
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

    public virtual void OnEvent(CharacterEvent _characterEvent)
    {
        if (_characterEvent.character.Equals(this) == true)
        {
            if (_characterEvent.type == CharacterEventType.ExecuteAction)
            {
                ExecuteAction(true);
                Debug.Log(this.gameObject + "Starts executing action");
            }
            if (_characterEvent.type == CharacterEventType.FinishExecute)
            {
                ExecuteAction(false);
                Debug.Log(this.gameObject + "Finishes executing action");
            }
        }
        return;
    }
}
