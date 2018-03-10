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
        lastSpecialShoot = -specialCooldown;    // Because it will only execute after 15 secondas passed into the level.
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
                var time = Time.timeSinceLevelLoad;
                Debug.Log(string.Format("{0} ejecutando ataque especial ", time));
                specialAttack.RangeAttack();
                lastSpecialShoot = Time.timeSinceLevelLoad;
            }
            else if (!IsCasting &&
                Time.timeSinceLevelLoad > lastShoot + settings.AttackSpeed &&
                enemyDetect.HasEnemyDirection(out enemyDirection)
                )
            {
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
