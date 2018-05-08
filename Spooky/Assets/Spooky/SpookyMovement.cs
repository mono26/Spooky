using System.Collections;
using UnityEngine;

public class SpookyMovement : MonoBehaviour
{
    /// <summary>
    /// Dependencies for the component.
    /// </summary>
    private Spooky spooky;

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

    private void Awake()
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        spooky = GetComponent<Spooky>();
        if (spooky == null)
            Debug.LogError("There is no Spooky component attached to the object.");
    }

    public void Start()
    {
        // When the game starts set values
        currentSpeed = maxSpeed;
    }

    private void Move(Vector3 _direction)
    {
        // For moving the Rigidbody in the desired direction
        Vector3 newPosition = spooky.SpookyBody.position + spooky.SpookyBody.transform.TransformDirection(_direction) * currentSpeed * Time.fixedDeltaTime;
        spooky.SpookyBody.MovePosition(newPosition);
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

    public void HandleInput()
    {
        movementDirection = InputManager.Instance.Movement;
    }

    public void FixedFrameProcess()
    {
        Move(movementDirection);

        ClampPosition();
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
