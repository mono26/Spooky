using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyMovement movement;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    public EnemyAttack mainAbility;
    public Coroutine ability;

    protected float basicCooldown;
    protected int currentHealth;

    public virtual void Awake()
    {
        movement = new EnemyMovement(this, settings.MovementSettings);
    }

    public virtual void Start()
    {
        currentHealth = settings.MaxHealth;
        movement.Start();
    }

    public bool IsDead()
    {
        if (currentHealth > 0)
        {
            return false;
        }
        else return true;
    }

    private void LoseHealth(int _damage)
    {
        currentHealth -= _damage;
    }

    [System.Serializable]
    public class  Settings
    {
        public Rigidbody Rigidbody;
        public NavMeshAgent NavMesh;
        public Transform Target;

        public int MaxHealth;

        [SerializeField]
        public EnemyMovement.Settings MovementSettings;
    }
}
