using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected EnemyMovement movementComponent;
    public EnemyHealthComponent healthComponent;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    public ICloseRangeAttack basicAbility;
    [SerializeField]
    protected Coroutine ability;
    public Transform target;
    [SerializeField]
    protected float lastAttackExecution;

    protected void Awake()
    {
        movementComponent = new EnemyMovement(this, settings.MovementSettings);
        healthComponent = new EnemyHealthComponent(this, settings.MaxHealth, settings.HealthSettings);
    }

    protected void Start()
    {
        movementComponent.Start();
        healthComponent.Start();
    }

    public bool IsDead()
    {
        if (healthComponent.GetCurrentHealth() > 0)
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

    public void CastAbility(Coroutine _cast)
    {
        ability = _cast;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Bullet"))
        {
            int damage = _collision.gameObject.GetComponent<Bullet>().GetBulletDamage();
            healthComponent.TakeDamage(damage);
        }
    }

    [System.Serializable]
    public class  Settings
    {
        public Rigidbody Rigidbody;
        public NavMeshAgent NavMesh;
        public float BasicCooldown;
        public float BasicRange;

        public Image HealthBar;
        public int MaxHealth;

        [SerializeField]
        public EnemyMovement.Settings MovementSettings;
        [SerializeField]
        public EnemyHealthComponent.Settings HealthSettings;
    }
}
