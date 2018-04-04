﻿using UnityEngine;

public class Tomato : Plant
{
    protected enum State
    {
        Waiting,
        Attacking,
        Death,
    }

    [SerializeField]
    protected float launchForce;
    [SerializeField]
    protected TomatoBullet bullet;
    [SerializeField]
    protected Transform launchPosition;
    protected PlantLaunchProyectile basicAttack;

    [SerializeField]
    protected State currentState;

    public new void Start()
    {
        base.Start();

        basicAttack = new PlantLaunchProyectile(this, bullet, launchForce, launchPosition, soundEffect);
        currentState = State.Waiting;
    }

    // Update is called once per frame
    public void Update()
    {
        enemyDetect.Detect();

        if (IsPlantDead())
        {
            currentState = State.Death;
        }

        if (currentState.Equals(State.Waiting))
        {
            //TODO execute decision
            if (enemyDetect.HasAValidTarget() && IsTargetInRange())
            {
                currentState = State.Attacking;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Attacking))
        {
            if (!IsCasting &&
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
            // TODO destroy plant
        }

        else return;
    }
}
