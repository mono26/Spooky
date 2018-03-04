using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField]
    private Rigidbody rigidbody;
    public NavMeshAgent navMesh;
    private Settings settings;
    private NavMeshPath path;

    private float currentSpeed;
    [SerializeField]
    private Transform target;

    public EnemyMovement(Rigidbody _rigidbody, NavMeshAgent _navMesh, Transform _target, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        rigidbody = _rigidbody;
        navMesh = _navMesh;
        target = _target;
        settings = _settings;

        path = new NavMeshPath();
    }

    public void Start()
    {
        // When the game starts set values
        //navMesh.updatePosition = false;
        navMesh.updateRotation = false;
        navMesh.updateUpAxis = false;

        navMesh.CalculatePath(target.position, path);

        currentSpeed = settings.MaxSpeed;
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        if (!path.Equals(null))
        {
            // TODO calculate dot and cross product for input values moving the rigidBody
            Vector3 desiredDirection = (path.corners[1] - rigidbody.transform.position).normalized;
            desiredDirection.y = 0;
            float horizontal = Vector3.Dot(rigidbody.transform.right, desiredDirection);
            float vertical = -Vector3.Cross(rigidbody.transform.right, desiredDirection).y;

            if (!horizontal.Equals(0f) || !vertical.Equals(0f))
            {
                // We need to pass a movement in y because of the rotation of the object, so the sprite can be seen.
                Vector3 direction = new Vector3(horizontal, vertical, 0);
                Move(direction);

                navMesh.CalculatePath(target.position, path);
                return;
            }
            else
            {
                navMesh.CalculatePath(target.position, path);
                return;
            }
        }
        else return;
    }

    private void Move(Vector3 _direction)
    {
        // For moving the rigidbody in the desired direction
        Vector3 newPosition = rigidbody.position + rigidbody.transform.TransformDirection(_direction) * currentSpeed * Time.fixedDeltaTime;
        rigidbody.MovePosition(newPosition);
        return;
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
