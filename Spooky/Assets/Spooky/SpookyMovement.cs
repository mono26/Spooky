using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpookyMovement
{
    [SerializeField]
    private Rigidbody rigidbody;
    private Joystick joystick;
    private Settings settings;

    private float currentSpeed;

    public SpookyMovement(Rigidbody _rigidbody, Joystick _joystick, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        rigidbody = _rigidbody;
        joystick = _joystick;
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
        // For moving the rigidBody
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (!horizontal.Equals(0f) || !vertical.Equals(0f))
        {
            var time = Time.realtimeSinceStartup;
            Debug.Log(string.Format("{0} horizontal {1} vertical {2}", time, horizontal, vertical));

            Vector3 direction = new Vector3(-horizontal, vertical, 0);
            Move(direction);

            return;
        }
        else
            return;
    }

    private void Move(Vector3 _direction)
    {
        // For moving the rigidbody in the desired direction
        var time = Time.realtimeSinceStartup;
        Vector3 newPosition = rigidbody.position + rigidbody.transform.TransformDirection(_direction) * currentSpeed * Time.fixedDeltaTime;
        Debug.Log(string.Format(" {0} new position {1}", time, newPosition));
        rigidbody.MovePosition(newPosition);
        //return;
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
