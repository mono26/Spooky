using UnityEngine;

[RequireComponent(typeof(PoolableObject), typeof(EnemyStats), typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
public class Enemy : Character
{
    public bool IsExecutingAction { get; protected set; }

    private EnemyStats statsComponent;
    public EnemyStats StatsComponent { get { return statsComponent; } }
    private Health healthComponent;
    public Health HealthComponent { get { return healthComponent; } }
    private EnemyMovement movementComponent;
    public EnemyMovement MovementComponent { get { return movementComponent; } }

    // Assigned by inspector.
    [SerializeField]
    private CharacterAction currentAction;
    public CharacterAction CurrentAction { get { return currentAction; } }

    protected override void Awake()
    {
        base.Awake();

        statsComponent = GetComponent<EnemyStats>();
        if (!statsComponent)
            Debug.LogError("No stats component found on the enemy gameObject: " + this.gameObject.ToString());

        healthComponent = GetComponent<Health>();
        if (!statsComponent)
            Debug.LogError("No health component found on the enemy gameObject: " + this.gameObject.ToString());

        movementComponent = GetComponent<EnemyMovement>();
        if (!statsComponent)
            Debug.LogError("No movement component found on the enemy gameObject: " + this.gameObject.ToString());
    }

    protected void OnEnable()
    {
        IsExecutingAction = false;
        ActivateCollider(true);
    }

    protected override void Update()
    {
        if (currentAction.IsTargetInRange() && !IsExecutingAction)
        {
            movementComponent.StopEnemy(true);
            currentAction.ExecuteAction();
        }
        else
        {
            movementComponent.StopEnemy(false);
        }

        base.Update();
    }

    public void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
        return;
    }

    public void ChangeCurrentAction(CharacterAction _newAction)
    {
        currentAction = _newAction;
        return;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Bullet"))
        {
            float damage = _collision.gameObject.GetComponent<Bullet>().GetBulletDamage();
            healthComponent.TakeDamage(damage);
            //animationComponent.PlayAnimation("Damage");
        }
    }
}
