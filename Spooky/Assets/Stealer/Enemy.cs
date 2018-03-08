using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected EnemyMovement movement;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    public ICloseRangeAttack basicAbility;
    [SerializeField]
    protected Coroutine ability;
    public Transform target;

    [SerializeField]
    protected float lastAttackExecution;
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

    protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= settings.BasicRange)
        {
            return true;
        }
        else return false;
    }

    protected void LoseHealth(int _damage)
    {
        currentHealth -= _damage;
    }

    public void CastAbility(Coroutine _cast)
    {
        ability = _cast;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Bullet"))
        {
            int damage = _collision.gameObject.GetComponent<Bullet>().GetBulletDamage();
            LoseHealth(damage);
        }
    }

    [System.Serializable]
    public class  Settings
    {
        public Rigidbody Rigidbody;
        public NavMeshAgent NavMesh;
        public float BasicCooldown;
        public float BasicRange;

        public int MaxHealth;

        [SerializeField]
        public EnemyMovement.Settings MovementSettings;
    }
}
