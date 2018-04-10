using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField]
    private Enemy enemy;
    private Rigidbody rigidbody;
    private Settings settings;

    private NavMeshPath pathToTheTarget;
    public float maxMovementSpeed;
    public float slowMotionSpeed;
    private float currentMovementSpeed;

    public EnemyMovement(Enemy _enemy, float _maxMovementSpeed, Rigidbody _rigidbody, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        enemy = _enemy;
        maxMovementSpeed = _maxMovementSpeed;
        rigidbody = _rigidbody;
        settings = _settings;

        pathToTheTarget = new NavMeshPath();
    }

    public void Start()
    {
        // When the game starts set values
        //navMesh.updatePosition = false;
        settings.navMeshAgent.updateRotation = false;
        settings.navMeshAgent.updateUpAxis = false;

        if (enemy.Target)
            settings.navMeshAgent.CalculatePath(enemy.Target.position, pathToTheTarget);

        currentMovementSpeed = maxMovementSpeed;
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        if (enemy.Target)
        {
            settings.navMeshAgent.CalculatePath(enemy.Target.position, pathToTheTarget);

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
        enemy.AnimationComponent.CheckViewDirection(_direction);
        return;
    }

    [System.Serializable]
    public class Settings
    {
        public NavMeshAgent navMeshAgent;
        public float LevelBoundsInX;
        public float LevelBoundsInY;
    }
}
