using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealer : Enemy
{
    [SerializeField]
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
            if (IsNextToTarget())
            {
                currentState = State.Stealing;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Stealing))
        {
            if(ability != null && Time.timeSinceLevelLoad - lastBasicExecute < basicCooldown)
            {
                mainAbility.Execute(this);
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
            if (!settings.Target.CompareTag("Runaway Point"))
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

        }

        else return;
    }

    public void FixedUpdate()
    {
        if (currentState.Equals(State.Moving) || currentState.Equals(State.Escaping))
            movement.FixedUpdate();
        else return;
    }

    private bool IsNextToTarget()
    {
        float distance = Vector3.Distance(transform.position, settings.Target.position);
        if (distance <= mainAbility.range)
        {
            return true;
        }
        else return false;
    }

    private bool IsDoneStealing()
    {
        if (ability != null)
        {
            return false;
        }
        else return true;
    }

    private void ChangeTargetToRunPoint()
    {
        //TODO change this one for a random in the level manager runaway points.
        settings.Target = GameObject.Find("Runaway Point").transform;
    }
}
