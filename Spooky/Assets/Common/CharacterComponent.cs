using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
    protected Character character;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    protected virtual void OnEnable()
    {
        Health hasHealth = GetComponent<Health>();
        if (hasHealth != null)
        {
            hasHealth.OnDeath += OnCharacterDeath;
            hasHealth.OnRespawn += OnCharacterRespawn;
            return;
        }
        else return;
    }

    protected virtual void OnDisable()
    {
        Health hasHealth = GetComponent<Health>();
        if (hasHealth != null)
        {
            hasHealth.OnDeath -= OnCharacterDeath;
            hasHealth.OnRespawn -= OnCharacterRespawn;
            return;
        }
        else return;
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
}
