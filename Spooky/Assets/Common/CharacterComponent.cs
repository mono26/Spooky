using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour, EventHandler<CharacterEvent>
{
    protected Character character;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
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

    public virtual void EveryFrame()
    {
        HandleInput();

        UpdateState();
    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }

    protected virtual void HandleInput()
    {

    }

    protected virtual void UpdateState()
    {

    }

    protected virtual void OnCharacterDeath()
    {

    }

    protected virtual void OnCharacterRespawn()
    {

    }

    public virtual void OnEvent(CharacterEvent _characterEvent)
    {
        if(_characterEvent.character.Equals(character))
        {
            if (_characterEvent.type == CharacterEventType.Death)
                OnCharacterDeath();
            else if (_characterEvent.type == CharacterEventType.Respawn)
                OnCharacterRespawn();
        }
        return;
    }
}
