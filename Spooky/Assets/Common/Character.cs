using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Character : MonoBehaviour
{
    public enum CharacterState { Idle, Moving, ExecutingAction, Dead }
    public enum CharacterType { Player, AI}

    [SerializeField]
    private CharacterType type = CharacterType.AI;
    public CharacterType Type { get { return type; } }
    [SerializeField]
    private string characterID = "";
    public string CharacterID { get { return characterID; } }
    public InputManager CharacterInput { get; protected set; }
    [SerializeField]
    public StateMachine<CharacterState> characterStateMachine;

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

        LinkCharacterInput();
    }

    protected virtual void Update()
    {
        foreach(CharacterComponent component in characterComponents)
        {
            component.EveryFrame();
        }
    }

    protected virtual void FixedUpdate()
    {
        foreach (CharacterComponent component in characterComponents)
        {
            component.FixedFrame();
        }
        /*else
        {
            spooky.AnimationComponent.IsMoving(new Vector3(0, 0, 0));
            return;
        }*/
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
            // We get the corresponding input manager
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
            characterAnimator.SetBool("Idle", (characterStateMachine.CurrentState == CharacterState.Idle));
            characterAnimator.SetBool("Dead", (characterStateMachine.CurrentState == CharacterState.Dead));
            characterAnimator.SetBool("Moving", (characterStateMachine.CurrentState == CharacterState.Moving));
            characterAnimator.SetBool("Action", (characterStateMachine.CurrentState == CharacterState.ExecutingAction));
        }
    }

    protected virtual void OnTriggerEnter(Collider _collider)
    {

    }

    protected virtual void OnTriggerExit(Collider _collider)
    {

    }

}
