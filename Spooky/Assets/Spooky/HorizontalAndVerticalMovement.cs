using System.Collections;
using UnityEngine;

public class HorizontalAndVerticalMovement : CharacterComponent
{
    private Vector3 movementDirection;

    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float slowMotionSpeed;

    [SerializeField]
    private float MaxXValue;
    [SerializeField]
    private float MaxYValue;

    private bool onSloweffect;

    protected void Start()
    {
        // When the game starts set values
        currentSpeed = maxSpeed;
    }

    public override void FixedFrame()
    {
        base.FixedFrame();

        Move(movementDirection);

        ClampPosition();
    }

    protected override void HandleInput()
    {
        movementDirection = character.characterInput.Movement;
    }

    private void Move(Vector3 _direction)
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
