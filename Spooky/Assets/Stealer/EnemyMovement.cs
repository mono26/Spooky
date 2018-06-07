using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AICharacter))]
public class EnemyMovement : HorizontalAndVerticalMovement
{
    protected AICharacter aICharacter;
    protected NavMeshPath pathToTheTarget;
    [SerializeField]
    protected float pathUpdateRatePerSeconds;

    [Header("For Debugging in editor")]
    protected bool isStopped = false;
    public bool IsStopped { get { return isStopped; } }

    protected override void Awake()
    {
        base.Awake();

        aICharacter = character as AICharacter;

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
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(ReCalculatePath());
        isStopped = false;

        return;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StopCoroutine(ReCalculatePath());

        return;
    }

    public override void FixedFrame()
    {
        if (isStopped == true)
        {
            StopMovement();
        }
        else if (isStopped == false)
        {
            Vector3 direction = CalculateDirectionToTarget();
            movementDirection.x = direction.x;
            movementDirection.y = direction.z;
            /*movementDirection.x = Vector3.Dot(GetComponent<Rigidbody>().transform.right, desiredDirection);
            movementDirection.z = -Vector3.Cross(GetComponent<Rigidbody>().transform.right, desiredDirection).y;*/
        }

        base.FixedFrame();
    }

    protected IEnumerator ReCalculatePath()
    {
        if (aICharacter.CurrentAction.Target != null)
        {
            NavMesh.CalculatePath(
            character.CharacterTransform.position, aICharacter.CurrentAction.Target.position, NavMesh.AllAreas, pathToTheTarget
            );
        }
        yield return new WaitForSeconds(1 / pathUpdateRatePerSeconds);

        StartCoroutine(ReCalculatePath());
    }

    protected Vector3 CalculateDirectionToTarget()
    {
        if (pathToTheTarget.Equals(null) == false && pathToTheTarget.corners.Length > 0)
        {
            Vector3 desiredDirection = (pathToTheTarget.corners[1] - character.CharacterTransform.position).normalized;
            desiredDirection.y = 0;
            return desiredDirection;
        }
        else
            pathToTheTarget.ClearCorners();

        return Vector3.zero;
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
