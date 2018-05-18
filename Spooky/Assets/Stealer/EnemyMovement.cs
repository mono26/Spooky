using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class EnemyMovement : HorizontalAndVerticalMovement
{
    protected Enemy enemy;
    protected NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    protected NavMeshPath pathToTheTarget;

    protected bool isEnemyStopped;
    public bool IsEnemyStopped { get { return isEnemyStopped; } }

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        pathToTheTarget = new NavMeshPath();
    }

    protected override void Start()
    {
        maxSpeed = enemy.StatsComponent.MovementSpeed;
        slowMotionSpeed = maxSpeed * 0.5f;

        base.Start();

        // When the game starts set values
        //navMesh.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    public override void FixedFrame()
    {
        if (isEnemyStopped)
        {
            StopMovement();
        }
        else if (!isEnemyStopped)
        {
            CalculateDirectionToTarget();
        }

        base.FixedFrame();
    }

    protected void CalculateDirectionToTarget()
    {
        if (enemy.CurrentAction.Target && navMeshAgent != null)
        {
            navMeshAgent.CalculatePath(enemy.CurrentAction.Target.position, pathToTheTarget);

            if (!pathToTheTarget.Equals(null))
            {
                // TODO calculate dot and cross product for input values moving the rigidBody
                Vector3 desiredDirection = (pathToTheTarget.corners[1] - character.CharacterTransform.position).normalized;
                desiredDirection.y = 0;
                movementDirection.x = desiredDirection.x;
                movementDirection.y = desiredDirection.z;
                /*movementDirection.x = Vector3.Dot(GetComponent<Rigidbody>().transform.right, desiredDirection);
                movementDirection.z = -Vector3.Cross(GetComponent<Rigidbody>().transform.right, desiredDirection).y;*/
                return;
            }
            else return;
        }
        else return;
    }

    public void StopEnemy(bool _stop)
    {
        isEnemyStopped = _stop;
        return;
    }

    protected override void OnCharacterDeath()
    {
        base.OnCharacterDeath();

        StopEnemy(true);
        return;
    }

    protected override void OnCharacterRespawn()
    {
        base.OnCharacterRespawn();

        StopEnemy(false);
        return;
    }
}
