using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
    protected Character character;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
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
}
