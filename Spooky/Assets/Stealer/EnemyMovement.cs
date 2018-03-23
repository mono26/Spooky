using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField]
    private Enemy enemy;
    private Rigidbody rigidbody;
    private Settings settings;

    private NavMeshPath path;

    private float currentSpeed;

    public EnemyMovement(Enemy _enemy, Rigidbody _rigidbody, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        enemy = _enemy;
        rigidbody = _rigidbody;
        path = new NavMeshPath();
        settings = _settings;

    }

    public void Start()
    {
        // When the game starts set values
        //navMesh.updatePosition = false;
        settings.navMeshAgent.updateRotation = false;
        settings.navMeshAgent.updateUpAxis = false;

        if (enemy.Target)
            settings.navMeshAgent.CalculatePath(enemy.Target.position, path);

        currentSpeed = settings.MaxSpeed;
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        if (enemy.Target)
        {
            settings.navMeshAgent.CalculatePath(enemy.Target.position, path);

            if (!path.Equals(null))
            {
                // TODO calculate dot and cross product for input values moving the rigidBody
                Vector3 desiredDirection = (path.corners[1] - rigidbody.transform.position).normalized;
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
                                currentSpeed * 
                                Time.fixedDeltaTime);

        rigidbody.MovePosition(newPosition);
        enemy.AnimationComponent.CheckViewDirection(_direction);
        return;
    }

    [System.Serializable]
    public class Settings
    {
        public NavMeshAgent navMeshAgent;

        public float MaxSpeed;
        public float SlowMotionSpeed;

        public float MaxXValue;
        public float MaxYValue;
    }
}
