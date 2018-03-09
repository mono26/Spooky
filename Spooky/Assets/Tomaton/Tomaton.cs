using UnityEngine;

public class Tomaton : Tomato
{
    [SerializeField]
    protected Bullet specialBullet;
    protected PlantLaunchProyectile specialAttack;
    [SerializeField]
    protected float specialCooldown;
    public float lastSpecialShoot;

    protected new void Start ()
    {
        base.Start();

        specialAttack = new PlantLaunchProyectile(this, specialBullet, launchForce, launchPosition);
	}

    // Update is called once per frame
    protected new void Update ()
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
                Time.timeSinceLevelLoad > lastSpecialShoot + specialCooldown &&
                enemyDetect.HasEnemyDirection(out enemyDirection)
                )
            {
                // TODO Need to pass direction of enemy
                specialAttack.RangeAttack();
                lastSpecialShoot = Time.timeSinceLevelLoad;
            }
            else if (!IsCasting &&
                Time.timeSinceLevelLoad > lastShoot + specialCooldown &&
                enemyDetect.HasEnemyDirection(out enemyDirection)
                )
            {
                // TODO Need to pass direction of enemy
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
