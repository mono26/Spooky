using UnityEngine;

public class Tomaton : Tomato
{
    protected Bullet specialBullet;
    protected PlantLaunchProyectile specialAttack;
    protected float specialCooldown;

    protected void Start ()
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
            if (Time.timeSinceLevelLoad > timeSinceLastShoot + settings.AttackSpeed && enemyDetect.HasEnemyDirection(out enemyDirection))
            {
                // TODO Need to pass direction of enemy
                basicAttack.RangeAttack();
            }
            if (Time.timeSinceLevelLoad > timeSinceLastShoot + specialCooldown && enemyDetect.HasEnemyDirection(out enemyDirection))
            {
                // TODO Need to pass direction of enemy
                specialAttack.RangeAttack();
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
