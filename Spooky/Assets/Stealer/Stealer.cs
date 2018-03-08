using UnityEngine;

public class Stealer : Enemy
{
    [SerializeField]
    private State currentState;
    protected int stoleValue;

    public enum State
    {
        Moving,
        Stealing,
        Escaping,
        Death,
    }

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        basicAbility = new Steal(this, stoleValue);
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
            if(Time.timeSinceLevelLoad - lastAttackExecution < settings.BasicCooldown)
            {
                basicAbility.CloseAttack();
            }

            if (IsDoneStealing())
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
            if (IsDoneStealing())
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

    private bool IsDoneStealing()
    {
        if (ability != null)
        {
            return false;
        }
        else return true;
    }

    private void ChangeTargetToHousePoint()
    {
        var time = Time.timeSinceLevelLoad;
        Debug.Log("{0} changing target" + time);
        target = LevelManager.Instance.GetRandomHousePoint();
    }

    private void ChangeTargetToRunPoint()
    {
        target = LevelManager.Instance.GetRandomRunawayPoint();
    }
}
