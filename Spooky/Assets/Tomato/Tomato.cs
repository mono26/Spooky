using UnityEngine;

public class Tomato : Plant
{
    [SerializeField]
    protected float launchForce;
    [SerializeField]
    protected Bullet bullet;
    [SerializeField]
    protected Transform launchPosition;
    [SerializeField]
    protected PlantBasicAttack basicAttack;
    [SerializeField]
    private State currentState;

    public enum State
    {
        Waiting,
        Attacking,
        Death,
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        basicAttack = new PlantBasicAttack(this, bullet, settings.AttackSpeed, launchForce, launchPosition);
        currentState = State.Waiting;
    }

    // Update is called once per frame
    public void Update()
    {
        enemyDetect.Detect();

        if (IsDead())
        {
            currentState = State.Death;
        }

        if (currentState.Equals(State.Waiting))
        {
            //TODO execute decision
            if (enemyDetect.HasTarget() && IsTargetInRange())
            {
                currentState = State.Attacking;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Attacking))
        {
            if (enemyDetect.HasEnemyDirection(out enemyDirection))
            {
                // TODO Need to pass direction of enemy
                basicAttack.RangeAttack();
            }
            else return;
        }

        else if (currentState.Equals(State.Death))
        {
            // TODO release enemy
        }

        else return;
    }

    private bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, enemyDetect.GetCurrentTarget().transform.position);
        if (distance <= settings.ViewRange)
        {
            return true;
        }
        else return false;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        enemyDetect.OnTriggerEnter(_collider);
    }

    protected void OnTriggerExit(Collider _collider)
    {
        enemyDetect.OnTriggerExit(_collider);
    }

}
