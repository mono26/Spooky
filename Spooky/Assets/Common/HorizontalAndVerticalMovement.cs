using System.Collections;
using UnityEngine;

public class HorizontalAndVerticalMovement : CharacterComponent
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

    public override void FixedFrame()
    {
        if (!movementDirection.Equals(Vector3.zero))
        {
            Move(movementDirection);

            ClampPosition();
        }
        else return;
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
        //spooky.AnimationComponent.CheckViewDirection(_direction);
        return;
    }

    private void ClampPosition()
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
