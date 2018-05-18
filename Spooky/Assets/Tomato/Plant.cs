using UnityEngine;

[RequireComponent(typeof(EnemyDetect), typeof(Health), typeof(PlantStats))]
public class Plant : Character
{
    public bool IsExecutingAction { get; protected set; }

    private EnemyDetect enemyDetect;
    public EnemyDetect EnemyDetect { get { return enemyDetect; } }
    private Health healthComponent;
    public Health HealthComponent { get { return healthComponent; } }
    private PlantStats statsComponent;
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
            Debug.LogError("No health component found on the enemy gameObject: " + this.gameObject.ToString());

        healthComponent = GetComponent<Health>();
        if (!healthComponent)
            Debug.LogError("No health component found on the enemy gameObject: " + this.gameObject.ToString());

        statsComponent = GetComponent<PlantStats>();
        if (!statsComponent)
            Debug.LogError("No stats component found on the enemy gameObject: " + this.gameObject.ToString());
    }

    protected void Start()
    {

    }

    protected override void Update()
    {
        if (enemyDetect.IsFirstEnemyInTheListActive() && !IsExecutingAction)
        {
            currentAction.SetTarget(enemyDetect.GetFirstEnemyInTheList().transform);
            if(currentAction.IsTargetInRange())
            {
                currentAction.ExecuteAction();
            }
        }

        base.Update();
    }

    public void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
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
