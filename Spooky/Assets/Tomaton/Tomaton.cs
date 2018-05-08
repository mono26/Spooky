using UnityEngine;

public class Tomaton : Tomato
{
    [SerializeField]
    protected CorrosiveBullet specialBullet;
    protected PlantLaunchProyectile specialAttack;
    [SerializeField]
    protected float specialCooldown;
    public float lastSpecialShoot;

    protected new void Start ()
    {
        base.Start();

        specialAttack = new PlantLaunchProyectile(this, specialBullet, launchForce, launchPosition, soundEffect);
        lastSpecialShoot = -specialCooldown;    // Because it will only execute after 15 secondas passed into the level.
	}

    // Update is called once per frame
    protected new void Update ()
    {
        enemyDetect.Detect();

        if (IsPlantDead())
        {
            currentState = State.Death;
        }

        if (currentState.Equals(State.Waiting))
        {
            //TODO execute decision
            /*if (enemyDetect.HasAValidTarget() && IsTargetInRange())
            {
                currentState = State.Attacking;
                return;
            }
            else return;*/
        }

        else if (currentState.Equals(State.Attacking))
        {
            if (!IsCasting &&
                Time.timeSinceLevelLoad > lastSpecialShoot + specialCooldown &&
                enemyDetect.HasAValidTarget()
                )
            {
                animationComponent.CheckViewDirection(enemyDetect.GetEnemyDirection());
                specialAttack.RangeAttack();
                lastSpecialShoot = Time.timeSinceLevelLoad;
            }
            else if (!IsCasting &&
                Time.timeSinceLevelLoad > lastShoot + settings.AttackSpeed &&
                enemyDetect.HasAValidTarget()
                )
            {
                animationComponent.CheckViewDirection(enemyDetect.GetEnemyDirection());
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
