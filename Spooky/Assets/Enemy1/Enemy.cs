using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyAttack attack;
    [SerializeField]
    public EnemyMovement movement;
    public EnemyState state;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    public int currentHealth;

    public virtual void Awake()
    {
        attack = new EnemyAttack();
        movement = new EnemyMovement(settings.Rigidbody, settings.NavMesh, settings.Target , settings.MovementSettings);
        state = new EnemyState();
    }

    public virtual void Start()
    {
        movement.Start();
    }

    public virtual void Update()
    {
        if (currentHealth.Equals(0))
        {
            // TODO change state to dead
        }
    }

    public virtual void FixedUpdate()
    {
        //TOOD execute rigidBody movement. If state is true.
        movement.FixedUpdate();
    }

    private void LoseHealth(int _damage)
    {

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
