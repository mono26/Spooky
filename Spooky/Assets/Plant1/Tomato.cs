using UnityEngine;

public class Tomato : Plant
{
    [SerializeField]
    protected PlantBasicAttack basicAttack;
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

        basicAttack = new PlantBasicAttack(this, settings.AttackSpeed);
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
            if (enemyDetect.HasEnemyDirection(out enemyDirection))
            {
                currentState = State.Attacking;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Attacking))
        {
            if (ability != null && enemyDetect.HasEnemyDirection(out enemyDirection))
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

    protected void OnTriggerEnter(Collider _collider)
    {
        enemyDetect.OnTriggerEnter(_collider);
    }

    protected void OnTriggerExit(Collider _collider)
    {
        enemyDetect.OnTriggerExit(_collider);
    }

}
