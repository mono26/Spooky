using UnityEngine;

public class Tomato : Plant
{
    [SerializeField]
    protected Bullet BasicBullet;
    [SerializeField]
    private State currentState;
    private float attackRange;

    public enum State
    {
        Waiting,
        Attacking,
        Death,
    }

    public override void Start()
    {
        base.Start();

        currentState = State.Waiting;
    }

    // Update is called once per frame
    public void Update()
    {
        if (IsDead())
        {
            currentState = State.Death;
        }

        if (currentState.Equals(State.Waiting))
        {
            //TODO execute decision
            if (enemyDetect.EnemyDirection(out enemyDirection))
            {
                currentState = State.Attacking;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Attacking))
        {
            if (ability != null && 
                Time.timeSinceLevelLoad - lastBasicExecute < settings.BasicCooldown &&
                enemyDetect.EnemyDirection(out enemyDirection)
                )
            {
                // TODO Need to pass direction of enemy
                settings.MainAbility.Execute(this);
            }
            else return;
        }

        else if (currentState.Equals(State.Death))
        {
            // TODO release enemy
        }

        else return;
    }

    private bool IsNextToTarget()
    {
        float distance = Vector3.Distance(transform.position, settings.Target.position);
        if (distance <= settings.BasicRange)
        {
            return true;
        }
        else return false;
    }

    private void ChangeTarget(Transform _target)
    {
        //TODO change this one for a random in the level manager runaway points.
        settings.Target = GameObject.Find("Runaway Point").transform;
    }
}
