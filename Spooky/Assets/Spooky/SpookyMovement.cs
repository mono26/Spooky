using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMovement
{
    private Rigidbody rigidbody;
    private Settings settings;

    private float currentSpeed;

    public SpookyMovement()
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
    }

    public void Start()
    {
        // When the game starts set values
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        // For moving the rigidBody
    }

    private void OnAnimatorMove(Vector3 _direction)
    {
        // _For moving the rigidbody in the desired direction
    }
    private void ClampPosition()
    {
        // For clamping Spooky position inside the max and min value
    }

    public class Settings
    {
        public float MaxSpeed;
        public float SlowMotionSpeed;

        public float MaxXValue;
        public float MaxYValue;
    }
}
