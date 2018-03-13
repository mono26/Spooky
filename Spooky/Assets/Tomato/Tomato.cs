using UnityEngine;

public class Tomato : Plant
{
    [SerializeField]
    protected float launchForce;
    [SerializeField]
    protected Bullet bullet;
    [SerializeField]
    protected Transform launchPosition;
    protected PlantLaunchProyectile basicAttack;

    [SerializeField]
    protected State currentState;

    protected enum State
    {
        Waiting,
        Attacking,
        Death,
    }

    public new void Start()
    {
        base.Start();

        basicAttack = new PlantLaunchProyectile(this, bullet, launchForce, launchPosition);
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
            if (!IsCasting &&
                Time.timeSinceLevelLoad > lastShoot + settings.AttackSpeed &&
                enemyDetect.HasEnemyDirection(out enemyDirection)
                )
            {
                animationComponent.CheckViewDirection(enemyDirection);
                basicAttack.RangeAttack();
                lastShoot = Time.timeSinceLevelLoad;
            }
            else
            {
                currentState = State.Waiting;
                return;
            }
        }

        else if (currentState.Equals(State.Death))
        {
            // TODO release enemy
        }

        else return;
    }
}
