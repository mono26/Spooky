using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath
{
    private Enemy owner;

    [SerializeField]
    private bool isDying;
    public bool IsDying { get { return isDying; } set { isDying = value; } }
    private Coroutine dieProcess;
    public Coroutine DieProcess { get { return dieProcess; } set { dieProcess = value; } }

    public void OnEnable()
    {
        isDying = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsDead()
    {
        if (owner.HealthComponent.GetCurrentHealth() > 0)
        {
            return false;
        }
        else if (owner.HealthComponent.GetCurrentHealth() == 0)
        {
            return true;
        }
        else return false;
    }
}
