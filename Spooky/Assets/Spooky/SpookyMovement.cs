using UnityEngine;

public class SpookyMovement
{
    private Spooky spooky;
    private Rigidbody rigidbody;
    private Settings settings;

    private float currentSpeed;

    public SpookyMovement(Spooky _spooky, Rigidbody _rigidbody, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        spooky = _spooky;
        rigidbody = _rigidbody;
        settings = _settings;
    }

    public void Start()
    {
        // When the game starts set values
        currentSpeed = settings.MaxSpeed;
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        // For moving the Rigidbody
        float horizontal = spooky.Joystick.Horizontal;
        float vertical = spooky.Joystick.Vertical;

        if (!vertical.Equals(0) || !vertical.Equals(0))
        {
            Vector3 direction = new Vector3(horizontal, vertical, 0);
            Move(direction);
            return;
        }
        // We need to pass a movement in "y" because of the rotation of the object, so the sprite can be seen.
        else
        {
            spooky.AnimationComponent.IsMoving(new Vector3(0,0,0));
            return;
        }
    }

    private void Move(Vector3 _direction)
    {
        // For moving the Rigidbody in the desired direction
        Vector3 newPosition = rigidbody.position + rigidbody.transform.TransformDirection(_direction) * currentSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(newPosition);
        spooky.AnimationComponent.CheckViewDirection(_direction);
        return;
    }

    private void ClampPosition()
    {
        // For clamping Spooky position inside the max and min value
    }

    [System.Serializable]
    public class Settings
    {
        public float MaxSpeed;
        public float SlowMotionSpeed;

        public float MaxXValue;
        public float MaxYValue;
    }
}
