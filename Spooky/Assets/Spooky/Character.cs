using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Character : MonoBehaviour
{
    public enum CharacterType { Player, AI}

    public CharacterType type = CharacterType.AI;
    public string characterID = "";
    public InputManager characterInput { get; protected set; }

    // Obligatory Components
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

    private void Awake()
    {
        characterAudioSource = GetComponent<AudioSource>();
        characterCollider = GetComponent<BoxCollider>();
        characterSprite = GetComponent<SpriteRenderer>();
        characterTransform = GetComponent<Transform>();

        characterBody = GetComponent<Rigidbody>();
        characterComponents = GetComponents<CharacterComponent>();

        LinkCharacterInput();
    }

    private void Update()
    {
        foreach(CharacterComponent component in characterComponents)
        {
            component.EveryFrame();
        }
    }

    private void FixedUpdate()
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

    private void LateUpdate()
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
                characterInput = null;
                InputManager[] foundInputManagers = FindObjectsOfType(typeof(InputManager)) as InputManager[];
                foreach (InputManager foundInputManager in foundInputManagers)
                {
                    if (foundInputManager.playerID == characterID)
                    {
                        characterInput = foundInputManager;
                        return;
                    }
                }
            }
        }
        else return;
    }

    public void OnTriggerEnter(Collider _collider)
    {

    }

    public void OnTriggerExit(Collider _collider)
    {

    }

}
