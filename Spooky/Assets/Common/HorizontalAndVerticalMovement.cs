using System.Collections;
using UnityEngine;

public enum MovementEventType { Stop, Move}

public class MovementEvent : SpookyCrowEvent
{
    public MovementEventType type;
    public Character character;

    public MovementEvent(MovementEventType _type, Character _character)
    {
        type = _type;
        character = _character;
    }
}

public class HorizontalAndVerticalMovement : CharacterComponent, EventHandler<MovementEvent>
{
    protected Vector3 movementDirection;

    [SerializeField]
    protected float currentSpeed;
    [SerializeField]
    protected float maxSpeed;
    [SerializeField]
    protected float slowMotionSpeed;

    [SerializeField]
    protected float MaxXValue;
    [SerializeField]
    protected float MaxYValue;

    private bool onSloweffect;

    protected virtual void Start()
    {
        // When the game starts set values
        currentSpeed = maxSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentSpeed = maxSpeed;

        EventManager.AddListener<MovementEvent>(this);

        return;
    }

    protected override void OnDisable()
    {
            base.OnDisable();

            EventManager.RemoveListener<MovementEvent>(this);
    }

    public override void FixedFrame()
    {
        if (movementDirection.Equals(Vector3.zero) == false)
        {
            FlipSpriteAccordingToMovement();

            Move(movementDirection);

            ClampPosition();
        }

        return;
    }

    protected void FlipSpriteAccordingToMovement()
    {
        if (movementDirection.x > 0.1f)
        {
            if (!character.IsFacingRightDirection)
                character.Flip();
        }
        // If it's negative, then we're facing left
        else if (movementDirection.x < -0.1f)
        {
            if (character.IsFacingRightDirection)
                character.Flip();
        }
    }

    protected override void HandleInput()
    {
        if (character.Type == Character.CharacterType.Player)
            movementDirection = character.CharacterInput.Movement;
        else return;
    }

    protected void Move(Vector3 _direction)
    {
        // For moving the Rigidbody in the desired direction
        Vector3 newPosition = character.CharacterBody.position + character.CharacterTransform.TransformDirection(_direction) * currentSpeed * Time.fixedDeltaTime;
        character.CharacterBody.MovePosition(newPosition);
        return;
    }

    protected void ClampPosition()
    {
        // For clamping Spooky position inside the max and min value
    }

    private IEnumerator SlowDownEffect()
    {
        while (onSloweffect)
        {
            currentSpeed = slowMotionSpeed;
            yield return 0;
        }

        currentSpeed = maxSpeed;
        yield return 0.25f;
    }

    protected virtual void StopMovement()
    {
        movementDirection = Vector3.zero;
        character.CharacterBody.velocity = Vector3.zero;
        return;
    }

    protected override void UpdateState()
    {
        if (character.characterStateMachine != null)
        {
            if (movementDirection.Equals(Vector3.zero))
            {
                character.characterStateMachine.ChangeState(Character.CharacterState.Idle);
            }
            else
            {
                character.characterStateMachine.ChangeState(Character.CharacterState.Moving);
            }
        }
        else return;
    }

    public virtual void OnEvent(MovementEvent _movementEvent)
    {
        if (_movementEvent.character.Equals(this.character) && _movementEvent.type == MovementEventType.Stop)
            StopMovement();
        return;
    }

    protected override void OnCharacterDeath()
    {
        StopMovement();
        return;
    }

    protected override void OnCharacterRespawn()
    {
        currentSpeed = maxSpeed;
        return;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("CornField"))
        {
            onSloweffect = true;
            StartCoroutine(SlowDownEffect());
        }
        else return;
    }

    private void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            onSloweffect = false;
        }
        else return;
    }
}
