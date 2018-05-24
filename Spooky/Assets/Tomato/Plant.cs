using UnityEngine;

public enum PlantEventType { ExecuteAction, FinishExecute, Killed }

public class PlantEvent : SpookyCrowEvent
{
    public Plant plant;
    public PlantEventType type;

    public PlantEvent(PlantEventType _type, Plant _plant)
    {
        plant = _plant;
        type = _type;
    }
}

[RequireComponent(typeof(EnemyDetect), typeof(Health), typeof(PlantStats))]
public class Plant : Character, EventHandler<PlantEvent>
{
    public bool IsExecutingAction { get; protected set; }

    protected EnemyDetect enemyDetect;
    public EnemyDetect EnemyDetect { get { return enemyDetect; } }
    protected Health healthComponent;
    public Health HealthComponent { get { return healthComponent; } }
    protected PlantStats statsComponent;
    public PlantStats StatsComponent { get { return statsComponent; } }

    // Assigned by inspector.
    [SerializeField]
    private CharacterAction currentAction;
    public CharacterAction CurrentAction { get { return currentAction; } }

    protected override void Awake()
    {
        base.Awake();

        enemyDetect = GetComponent<EnemyDetect>();
        if (!enemyDetect)
            Debug.LogError(this.gameObject.ToString() + "No health component found on the enemy gameObject: ");

        healthComponent = GetComponent<Health>();
        if (!healthComponent)
            Debug.LogError(this.gameObject.ToString() + "No health component found on the enemy gameObject: ");

        statsComponent = GetComponent<PlantStats>();
        if (!statsComponent)
            Debug.LogError(this.gameObject.ToString() + "No stats component found on the enemy gameObject: ");

        if (currentAction == null)
            Debug.LogError(this.gameObject.ToString() + "No current action assigned on the enemy gameObject: ");
    }

    protected void OnEnable()
    {
        EventManager.AddListener<PlantEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<PlantEvent>(this);
        return;
    }

    protected override void Update()
    {
        if (enemyDetect.IsFirstEnemyInTheListActive() && !IsExecutingAction)
        {
            if(currentAction.IsTargetInRange())
            {
                StartCoroutine(currentAction.ExecuteAction());
            }
        }

        base.Update();

        return;
    }

    public void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
        return;
    }

    public void OnEvent(PlantEvent _plantEvent)
    {
        if (_plantEvent.plant.Equals(this))
        {
            if (_plantEvent.type == PlantEventType.ExecuteAction)
                ExecuteAction(true);
            if (_plantEvent.type == PlantEventType.FinishExecute)
                ExecuteAction(false);
        }
        return;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        enemyDetect.OnTriggerEnter(_collider);
    }

    protected void OnTriggerExit(Collider _collider)
    {
        enemyDetect.OnTriggerExit(_collider);
    }


}
