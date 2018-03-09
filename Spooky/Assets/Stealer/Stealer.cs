using UnityEngine;

public class Stealer : Enemy
{
    [SerializeField]
    private State currentState;
    [SerializeField]
    public int stoleValue;
    public bool hasLoot = false;

    public enum State
    {
        Moving,
        Stealing,
        Escaping,
        Death,
    }

	// Use this for initialization
	public new void Start ()
    {
        base.Start();

        basicAbility = new Steal(this);
        ChangeTargetToHousePoint();
        currentState = State.Moving;
	}
	
	// Update is called once per frame
	public void Update ()
    {
        if(IsDead())
        {
            currentState = State.Death;
        }

        if (currentState.Equals(State.Moving))
        {
            //TODO execute decision
            if (IsTargetInRange())
            {
                currentState = State.Stealing;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Stealing))
        {
            if(Time.timeSinceLevelLoad > lastAttackExecution + settings.BasicCooldown)
            {
                basicAbility.CloseAttack();
            }

            if (hasLoot)
            {
                currentState = State.Escaping;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Escaping))
        {
            if (!target.CompareTag("Runaway Point"))
            {
                ChangeTargetToRunPoint();
            }

            if (IsTargetInRange())
            {
                //TODO release because it stealed
            }
            else return;
        }

        else if (currentState.Equals(State.Death))
        {
            // TODO release enemy
        }

        else return;
    }

    public void FixedUpdate()
    {
        if (currentState.Equals(State.Moving) || currentState.Equals(State.Escaping))
            movement.FixedUpdate();
        else return;
    }

    private void ChangeTargetToHousePoint()
    {
        target = LevelManager.Instance.GetRandomHousePoint();
    }

    private void ChangeTargetToRunPoint()
    {
        target = LevelManager.Instance.GetRandomRunawayPoint();
    }

    public bool HasLoot(bool _hasLoot)
    {
        hasLoot = _hasLoot;
        return hasLoot;
    }
}
