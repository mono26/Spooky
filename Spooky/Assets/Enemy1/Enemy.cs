using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyAttack attack;
    [SerializeField]
    public EnemyMovement movement;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    public int currentHealth;

    public virtual void Awake()
    {
        //attack = new EnemyAttack();
        movement = new EnemyMovement(this, settings.MovementSettings);
    }

    public virtual void Start()
    {
        movement.Start();
    }

    public virtual void Update()
    {
        bool isDead = CheckHealth();
    }

    public virtual void FixedUpdate()
    {
        //TOOD execute rigidBody movement. If state is true.
        movement.FixedUpdate();
    }

    private bool CheckHealth()
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
