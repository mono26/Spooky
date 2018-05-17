using UnityEngine;

public class Plant : Character
{
    public bool IsExecutingAction { get; protected set; }

    private PlantStats statsComponent;
    public PlantStats StatsComponent { get { return statsComponent; } }
    private EnemyDetect enemyDetect;
    public EnemyDetect EnemyDetect { get { return enemyDetect; } }
    private Health healthComponent;
    public Health HealthComponent { get { return healthComponent; } }

    protected void Awake()
    {
        /*enemyDetect = new EnemyDetect(
            this,
            settings.EnemyDetectSettings
            );

        animationComponent = new PlantAnimation(
            GetComponent<SpriteRenderer>(),
            GetComponent<Animator>()
            );*/
    }

    protected void Start()
    {

    }

    public void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
        return;
    }

    /*protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, enemyDetect.GetFirstEnemyInTheList().transform.position);
        if (distance <= settings.EnemyDetectSettings.enemyDetectionRange)
        {
            return true;
        }
        else return false;
    }*/

    protected void OnTriggerEnter(Collider _collider)
    {
        enemyDetect.OnTriggerEnter(_collider);
    }

    protected void OnTriggerExit(Collider _collider)
    {
        enemyDetect.OnTriggerExit(_collider);
    }


}
