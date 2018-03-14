using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    // TODO use get component for getting dependencies?

    public EnemyMovement movementComponent;
    // TODO set a Getter for this
    public EnemyHealthComponent healthComponent;
    public EnemyAnimation animationComponent;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    protected ICloseRangeAttack basicAbility;
    [SerializeField]
    protected Coroutine ability;
    public Transform target;
    [SerializeField]
    protected float lastAttackExecution;
    [SerializeField]
    protected SpriteRenderer sprite;
    [SerializeField]
    protected Animator animator;

    private bool isCasting;
    protected bool IsCasting { get { return isCasting; } private set { isCasting = value; } }

    public delegate void Killed();
    public event Killed OnKilled;

    //TODO event called when killed

    protected void Awake()
    {
        movementComponent = new EnemyMovement(this, settings.MovementSettings);
        healthComponent = new EnemyHealthComponent(this, settings.MaxHealth, settings.HealthSettings);
        animationComponent = new EnemyAnimation(sprite, animator);
    }

    protected void Start()
    {
        Physics.IgnoreLayerCollision(11, 11, true);

        movementComponent.Start();
        healthComponent.Start();
    }

    protected bool IsDead()
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

    public void StartCast(bool _cast)
    {
        IsCasting = _cast;
    }

    public void CastAbility(Coroutine _cast)
    {
        ability = _cast;
        StartCast(true);
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Bullet"))
        {
            int damage = _collision.gameObject.GetComponent<Bullet>().GetBulletDamage();
            healthComponent.TakeDamage(damage);
            animationComponent.PlayAnimation("Damage");
        }
    }

    protected void Die()
    {
        OnKilled();
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
        [SerializeField]
        public EnemyHealthComponent.Settings HealthSettings;
    }
}
