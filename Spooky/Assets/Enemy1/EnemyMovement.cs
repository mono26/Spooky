using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement
{
    [SerializeField]
    private Rigidbody rigidbody;
    public NavMeshAgent navMesh;
    private Settings settings;

    private float currentSpeed;
    private Transform target;

    public EnemyMovement(Rigidbody _rigidbody, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
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
        // TODO calculate dot and cross product for input values moving the rigidBody
        navMesh.SetDestination(target.position);
        Vector3 desiredDirection = navMesh.desiredVelocity.normalized;
        desiredDirection.y = 0;
        float horizontal = Vector3.Dot(rigidbody.transform.right.normalized, desiredDirection);
        float vertical = Vector3.Cross(rigidbody.transform.right.normalized, desiredDirection).z;

        if (!horizontal.Equals(0f) || !vertical.Equals(0f))
        {
            // We need to pass a movement in y because of the rotation of the object, so the sprite can be seen.
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
        Vector3 newPosition = rigidbody.position + rigidbody.transform.TransformDirection(_direction) * currentSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(newPosition);
        return;
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
