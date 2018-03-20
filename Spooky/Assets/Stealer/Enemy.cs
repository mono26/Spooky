using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    // TODO use get component for getting dependencies?

    public EnemyMovement movementComponent;
    // TODO set a Getter for this
    public EnemyHealth healthComponent;
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

    // TODO extraer interfaz IKillable o IRelesable???
    public delegate void Release();
    public event Release OnReleased;
    public bool isDying;

    protected void Awake()
    {
        movementComponent = new EnemyMovement(this, settings.MovementSettings);
        healthComponent = new EnemyHealth(this, settings.MaxHealth, settings.HealthSettings);
        animationComponent = new EnemyAnimation(sprite, animator);
    }

    protected void OnEnable()
    {
        movementComponent.Start();
        healthComponent.Start();
        ActivateCollider();
        // When enemy starts death Coroutine the collider still detects collision and decreases number of enemies.
        isDying = false;
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

    protected void ActivateCollider()
    {
        GetComponent<Collider>().enabled = false;
    }

    protected void DeactivateCollider()
    {
        GetComponent<Collider>().enabled = false;
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
        OnReleased();
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
        public EnemyHealth.Settings HealthSettings;
    }
}
