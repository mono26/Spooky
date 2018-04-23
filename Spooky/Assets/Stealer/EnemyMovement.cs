using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField]
    protected Enemy owner;
    protected Rigidbody rigidbody;
    public Rigidbody Rigidbody { get { return rigidbody; } }
    protected NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }
    protected Settings settings;

    protected NavMeshPath pathToTheTarget;
    public float maxMovementSpeed;
    public float slowMotionSpeed;
    protected float currentMovementSpeed;

    public EnemyMovement(Enemy _enemy, Rigidbody _rigidbody, NavMeshAgent _navMeshAgent, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        owner = _enemy;
        rigidbody = _rigidbody;
        navMeshAgent = _navMeshAgent;
        settings = _settings;

        pathToTheTarget = new NavMeshPath();
    }

    public void Start()
    {
        // When the game starts set values
        //navMesh.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        maxMovementSpeed = owner.StatsComponent.movementSpeed;

        if (owner.Target)
            navMeshAgent.CalculatePath(owner.Target.position, pathToTheTarget);

        currentMovementSpeed = maxMovementSpeed;
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        if (owner.Target)
        {
            navMeshAgent.CalculatePath(owner.Target.position, pathToTheTarget);

            if (!pathToTheTarget.Equals(null))
            {
                // TODO calculate dot and cross product for input values moving the rigidBody
                Vector3 desiredDirection = (pathToTheTarget.corners[1] - rigidbody.transform.position).normalized;
                desiredDirection.y = 0;
                float horizontal = Vector3.Dot(rigidbody.transform.right, desiredDirection);
                float vertical = -Vector3.Cross(rigidbody.transform.right, desiredDirection).y;

                // We need to pass a movement in y because of the rotation of the object, so the sprite can be seen.
                Vector3 direction = new Vector3(horizontal, vertical, 0);
                Move(direction);
                return;
            }
            else return;
        }
        else return;
    }

    private void Move(Vector3 _direction)
    {
        // For moving the rigidbody in the desired direction
        Vector3 newPosition = (rigidbody.position +
                                rigidbody.transform.TransformDirection(_direction) * 
                                currentMovementSpeed * 
                                Time.fixedDeltaTime);

        rigidbody.MovePosition(newPosition);
        owner.AnimationComponent.CheckViewDirection(_direction);
        return;
    }

    [System.Serializable]
    public class Settings
    {
        public float LevelBoundsInX;
        public float LevelBoundsInY;
    }
}
