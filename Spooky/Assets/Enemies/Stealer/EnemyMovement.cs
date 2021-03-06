﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AICharacter), typeof(NavMeshAgent))]
public class EnemyMovement : HorizontalAndVerticalMovement
{
    [Header("AI movement settings")]
    [SerializeField]
    protected AICharacter aICharacter;
    [SerializeField]
    protected NavMeshAgent movementAgent;
    [SerializeField]
    protected float pathUpdateRatePerSeconds = 1;

    [Header("For Debugging in editor")]
    protected bool isStopped = false;
    public bool IsStopped { get { return isStopped; } }

    protected override void Awake()
    {
        base.Awake();

        if (aICharacter == null)
            aICharacter = character as AICharacter;
        if(movementAgent == null)
            movementAgent = GetComponent<NavMeshAgent>();

        movementAgent.updateRotation = false;
        movementAgent.updateUpAxis = false;

        return;
    }

    protected override void Start()
    {
        if (character.GetComponent<StatsComponent>()) {
            maxSpeed = character.GetComponent<StatsComponent>().MovementSpeed; }

        slowMotionSpeed = maxSpeed * 0.5f;
        movementAgent.speed = maxSpeed;

        base.Start();

        return;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.AddListener<MovementEvent>(this);

        StartCoroutine(ReCalculatePath());
        movementAgent.speed = maxSpeed;
        movementAgent.enabled = true;
        isStopped = false;

        return;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventManager.RemoveListener<MovementEvent>(this);

        StopCoroutine(ReCalculatePath());

        return;
    }

    public override void FixedFrame()
    {
        if (isStopped == true) {
            StopMovement(); }

        else if (isStopped == false)
        {
            Vector3 direction = GetMovementDirection();
            movementDirection.x = direction.x;
            movementDirection.y = direction.z;
        }

        FlipSpriteAccordingToMovement();

        ClampPosition();

        return;
    }

    protected IEnumerator ReCalculatePath()
    {
        if(movementAgent.isStopped == false)
        {
            if (aICharacter.CurrentAction.Target != null)
            {
                movementAgent.SetDestination(aICharacter.CurrentAction.Target.position);
            }
        }

        yield return new WaitForSeconds(1 / pathUpdateRatePerSeconds);

        StartCoroutine(ReCalculatePath());
    }

    protected Vector3 GetMovementDirection()
    {
        if (movementAgent.path.status == NavMeshPathStatus.PathComplete && movementAgent.path.corners.Length > 0)
        {
            Vector3 desiredDirection = movementAgent.desiredVelocity;
            desiredDirection.y = 0;
            return desiredDirection;
        }
        else
            movementAgent.path.ClearCorners();

        return Vector3.zero;
    }

    protected void StopEnemy(bool _stop)
    {
        isStopped = _stop;
        movementAgent.isStopped = _stop;
        return;
    }

    public override void OnEvent(MovementEvent _movementEvent)
    {
        base.OnEvent(_movementEvent);

        if(_movementEvent.character.Equals(character))
        {
            if (_movementEvent.type == MovementEventType.Stop)
            {
                StopEnemy(true);
            }
            else if (_movementEvent.type == MovementEventType.Move)
            {
                StopEnemy(false);
            }
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
