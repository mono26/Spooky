using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath
{
    private Enemy enemy;

    [SerializeField]
    private bool isDying;
    public bool IsDying { get { return isDying; } set { isDying = value; } }
    private Coroutine dieProcess;
    public Coroutine DieProcess { get { return dieProcess; } set { dieProcess = value; } }

    public EnemyDeath(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void OnEnable()
    {
        isDying = false;
    }

    public bool IsDead()
    {
        if (enemy.HealthComponent.GetCurrentHealth() > 0)
        {
            return false;
        }
        else if (enemy.HealthComponent.GetCurrentHealth() == 0)
        {
            return true;
        }
        else return false;
    }
}
