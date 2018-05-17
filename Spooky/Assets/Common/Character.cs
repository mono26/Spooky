using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Character : MonoBehaviour
{
    public enum CharacterState { Idle, Moving, ExecutingAction, Dead }
    public enum CharacterType { Player, AI}
    public enum InitialFacingDirection { Right, Left}

    [SerializeField]
    private CharacterType type = CharacterType.AI;
    public CharacterType Type { get { return type; } }
    [SerializeField]
    private string characterID = "";
    public string CharacterID { get { return characterID; } }
    public InputManager CharacterInput { get; protected set; }
    [SerializeField]
    public StateMachine<CharacterState> characterStateMachine;
    [SerializeField]
    private InitialFacingDirection initialDirection = InitialFacingDirection.Right;

    // Obligatory Components
    private Animator characterAnimator;
    public Animator CharacterAnimator { get { return characterAnimator; } }
    private AudioSource characterAudioSource;
    public AudioSource CharacterAudioSource { get { return characterAudioSource; } }
    private BoxCollider characterCollider;
    public BoxCollider CharacterCollider { get { return characterCollider; } }
    private SpriteRenderer characterSprite;
    public SpriteRenderer CharacterSprite { get { return characterSprite; } }
    private Transform characterTransform;
    public Transform CharacterTransform { get { return characterTransform; } }

    // Optional components
    private Rigidbody characterBody;
    public Rigidbody CharacterBody { get { return characterBody; } }
    protected CharacterComponent[] characterComponents;

    public bool IsFacingRightDirection { get; protected set; }

    protected virtual void Awake()
    {
        characterAudioSource = GetComponent<AudioSource>();
        characterAnimator = GetComponent<Animator>();
        characterBody = GetComponent<Rigidbody>();
        characterComponents = GetComponents<CharacterComponent>();
        characterCollider = GetComponent<BoxCollider>();
        characterSprite = GetComponent<SpriteRenderer>();
        characterStateMachine = new StateMachine<CharacterState>();
        characterTransform = GetComponent<Transform>();

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
        foreach(CharacterComponent component in characterComponents)
        {
            component.EveryFrame();
        }

        UpdateAnimator();

        Debug.Log(this.gameObject + " current state: " + characterStateMachine.currentState);
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
}
