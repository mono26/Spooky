using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealer : Enemy
{
    private State currentState;

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
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        if(currentState.Equals(State.Moving))
        {
            //TODO execute decision
            if (IsNextToTarget())
            {
                currentState = State.Stealing;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Stealing))
        {
            if (IsDoneStealing())
            {
                currentState = State.Escaping;
            }
            else return;
        }

        else if (currentState.Equals(State.Escaping))
        {
            if (IsDoneStealing())
            {
                //TODO release because it stealed
            }
            else return;
        }

        else if (currentState.Equals(State.Death))
        {

        }
    }

    private bool IsNextToTarget()
    {
        float distance = Vector3.Distance(transform.position, settings.Target.position);
        if (distance <= attack.range)
        {
            return true;
        }
        else return false;
    }

    private bool IsDoneStealing()
    {
        var steal = attack as Steal;
        if (steal.IsStealFinish())
        {
            return true;
        }
        else return false;
    }
}
