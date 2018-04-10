using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private EnemyStats statsComponent;
    public EnemyStats StatsComponent { get { return statsComponent; } set { statsComponent = value; } }
    private EnemyMovement movementComponent;
    public EnemyMovement MovementComponent { get { return movementComponent; } }
    [SerializeField]
    private EnemyHealth healthComponent;
    public EnemyHealth HealthComponent { get { return healthComponent; } }
    private EnemyAnimation animationComponent;
    public EnemyAnimation AnimationComponent { get { return animationComponent; } }
    private EnemyDeath deathComponent;
    public EnemyDeath DeathComponent { get { return deathComponent; } }

    [SerializeField]
    protected Settings settings;

    [SerializeField]
    private Transform target;
    public Transform Target { get { return target; } }
    [SerializeField]
    protected float lastAttackExecution;

    protected Coroutine abilityCast;

    // TODO extraer interfaz IKillable o IRelesable???
    public delegate void Release();
    public event Release OnReleased;

    private bool isCasting;
    protected bool IsCasting { get { return isCasting; } private set { isCasting = value; } }

    protected void Awake()
    {
        movementComponent = new EnemyMovement(
            this,
            statsComponent.movementSpeed,
            GetComponent<Rigidbody>(),
            settings.MovementSettings
            );

        healthComponent = new EnemyHealth(
            this,
            statsComponent.maxHealth,
            settings.HealthSettings
            );

        animationComponent = new EnemyAnimation(
            GetComponent<SpriteRenderer>(),
            GetComponent<Animator>()
            );

        deathComponent = new EnemyDeath();
    }

    protected void OnEnable()
    {
        movementComponent.Start();
        healthComponent.Start();
        ActivateCollider();
        // When enemy starts death Coroutine the collider still detects collision and decreases number of enemies.
    }

    protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= statsComponent.basicRange)
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
        abilityCast = _cast;
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

    protected void ReleaseEnemy()
    {
        OnReleased();
        OnReleased = null;
    }

    protected void SetEnemyTarget(Transform _target)
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
        [SerializeField]
        public EnemyMovement.Settings MovementSettings;
        [SerializeField]
        public EnemyHealth.Settings HealthSettings;
    }
}
