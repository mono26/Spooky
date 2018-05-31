using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Character), typeof(NavMeshAgent))]
public class EnemyMovement : HorizontalAndVerticalMovement
{
    protected NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    protected NavMeshPath pathToTheTarget;

    protected bool isStopped;
    public bool IsStopped { get { return isStopped; } }

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent = GetComponent<NavMeshAgent>();

        pathToTheTarget = new NavMeshPath();
    }

    protected override void Start()
    {
        if (character.GetComponent<StatsComponent>())
        {
            maxSpeed = character.GetComponent<StatsComponent>().MovementSpeed;
        }
        slowMotionSpeed = maxSpeed * 0.5f;

        base.Start();

        // When the game starts set values
        //navMesh.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        isStopped = false;

        return;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void FixedFrame()
    {
        if (isStopped)
        {
            StopMovement();
        }
        else if (!isStopped)
        {
            CalculateDirectionToTarget();
        }

        base.FixedFrame();
    }

    protected void CalculateDirectionToTarget()
    {
        if (character.CurrentAction.Target && navMeshAgent != null)
        {
            navMeshAgent.CalculatePath(character.CurrentAction.Target.position, pathToTheTarget);

            if (!pathToTheTarget.Equals(null) && pathToTheTarget.corners.Length > 1)
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

    protected override void StopMovement()
    {
        base.StopMovement();
        return;
    }

    protected void StopEnemy(bool _stop)
    {
        isStopped = _stop;
        return;
    }

    public override void OnEvent(MovementEvent _movementEvent)
    {
        base.OnEvent(_movementEvent);

        if(_movementEvent.character.Equals(character))
        {
            if (_movementEvent.type == MovementEventType.Stop)
                StopEnemy(true);
            else if (_movementEvent.type == MovementEventType.Move)
                StopEnemy(false);
        }
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
