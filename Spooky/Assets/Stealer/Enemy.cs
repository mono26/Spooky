using UnityEngine;

public enum EnemyEventType { ExecuteAction, FinishExecute}

public class EnemyEvent : SpookyCrowEvent
{
    public Enemy enemy;
    public EnemyEventType type;

    public EnemyEvent(EnemyEventType _type, Enemy _enemy)
    {
        enemy = _enemy;
        type = _type;
    }
}

[RequireComponent(typeof(PoolableObject), typeof(EnemyStats), typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
public class Enemy : Character, EventHandler<EnemyEvent>
{
    public bool IsExecutingAction { get; protected set; }

    [SerializeField]
    private Health healthComponent;
    public Health HealthComponent { get { return healthComponent; } }
    [SerializeField]
    private EnemyMovement movementComponent;
    public EnemyMovement MovementComponent { get { return movementComponent; } }
    [SerializeField]
    private EnemyStats statsComponent;
    public EnemyStats StatsComponent { get { return statsComponent; } }

    // Assigned by inspector.
    [SerializeField]
    private CharacterAction currentAction;
    public CharacterAction CurrentAction { get { return currentAction; } }
    [SerializeField]
    private CharacterAction startingAction;

    protected override void Awake()
    {
        base.Awake();

        if(statsComponent == null)
            statsComponent = GetComponent<EnemyStats>();
        if (healthComponent == null)
            healthComponent = GetComponent<Health>();
        if (movementComponent == null)
            movementComponent = GetComponent<EnemyMovement>();

        if(currentAction == null)
            Debug.LogError(this.gameObject.ToString() + "No current action assigned on the enemy gameObject: ");
    }

    protected void OnEnable()
    {
        IsExecutingAction = false;
        ActivateCollider(true);
        currentAction = startingAction;
        EventManager.AddListener<EnemyEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<EnemyEvent>(this);
        return;
    }

    protected override void Update()
    {
        if (currentAction.IsTargetInRange() && !IsExecutingAction)
        {
            StartCoroutine(currentAction.ExecuteAction());
        }

        base.Update();

        return;
    }

    protected void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
        return;
    }

    public void OnEvent(EnemyEvent _enemyEvent)
    {
        if(_enemyEvent.enemy.Equals(this))
        {
            if (_enemyEvent.type == EnemyEventType.ExecuteAction)
                ExecuteAction(true);
            if (_enemyEvent.type == EnemyEventType.FinishExecute)
                ExecuteAction(false);
        }
        return;
    }

    public void ChangeCurrentAction(CharacterAction _newAction)
    {
        currentAction = _newAction;
        return;
    }

}
