using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private EnemyMovement movementComponent;
    public EnemyMovement MovementComponent { get { return movementComponent; } }
    private EnemyHealth healthComponent;
    public EnemyHealth HealthComponent { get { return healthComponent; } }
    private EnemyAnimation animationComponent;
    public EnemyAnimation AnimationComponent { get { return animationComponent; } }

    [SerializeField]
    protected Settings settings;

    protected ICloseRangeAttack basicAbility;
    [SerializeField]
    protected Coroutine ability;
    [SerializeField]
    private Transform target;
    public Transform Target { get { return target; } }
    [SerializeField]
    protected float lastAttackExecution;

    // TODO extraer interfaz IKillable o IRelesable???
    public delegate void Release();
    public event Release OnReleased;

    public bool isDying;
    private bool isCasting;
    protected bool IsCasting { get { return isCasting; } private set { isCasting = value; } }

    protected void Awake()
    {
        movementComponent = new EnemyMovement(
            this,
            GetComponent<Rigidbody>(),
            settings.MovementSettings
            );

        healthComponent = new EnemyHealth(
            this,
            settings.HealthSettings
            );

        animationComponent = new EnemyAnimation(
            GetComponent<SpriteRenderer>(),
            GetComponent<Animator>()
            );
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
        GetComponent<Collider>().enabled = true;
    }

    protected void DeactivateCollider()
    {
        GetComponent<Collider>().enabled = false;
    }

    protected void Die()
    {
        OnReleased();
    }

    protected void SetTarget(Transform _target)
    {
        target = _target;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Bullet"))
        {
            float damage = _collision.gameObject.GetComponent<Bullet>().GetBulletDamage();
            healthComponent.TakeDamage(damage);
            animationComponent.PlayAnimation("Damage");
        }
    }

    [System.Serializable]
    public class  Settings
    {
        public float BasicCooldown;
        public float BasicRange;

        [SerializeField]
        public EnemyMovement.Settings MovementSettings;
        [SerializeField]
        public EnemyHealth.Settings HealthSettings;
    }
}
