using UnityEngine;

[RequireComponent(typeof(PoolableObject), typeof(EnemyStats), typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
public class Enemy : Character
{
    [SerializeField]
    private Transform target;
    public Transform Target { get { return target; } }
    public bool IsExecutingAction { get; protected set; }

    private EnemyStats statsComponent;
    public EnemyStats StatsComponent { get { return statsComponent; } }
    private Health healthComponent;
    public Health HealthComponent { get { return healthComponent; } }

    // Assigned by inspector.
    [SerializeField]
    private CharacterAction basicAbility;
    public CharacterAction BasicAbility { get { return basicAbility; } }

    protected override void Awake()
    {
        base.Awake();

        statsComponent = GetComponent<EnemyStats>();
        if (!statsComponent)
            Debug.LogError("No stats component found on the enemy gameObject: " + this.gameObject.ToString());

        healthComponent = GetComponent<Health>();
        if (!statsComponent)
            Debug.LogError("No health component found on the enemy gameObject: " + this.gameObject.ToString());

        basicAbility = GetComponent<CharacterAction>();
        if (!basicAbility)
            Debug.LogError("No action component found on the enemy gameObject: " + this.gameObject.ToString());
    }

    protected void OnEnable()
    {
        IsExecutingAction = false;
        ActivateCollider(true);
        SetTargetForFirstTime(CharacterID);
    }

    protected override void Update()
    {
        if (IsTargetInRange() && !IsExecutingAction)
        {
            GetComponent<EnemyMovement>().StopEnemy(true);
            basicAbility.ExecuteAction();
        }

        base.Update();
    }

    protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, Target.position);
        if (distance <= statsComponent.BasicRange)
        {
            return true;
        }
        else return false;
    }

    public void ExecuteAction(bool _cast)
    {
        IsExecutingAction = _cast;
        return;
    }

    public void ChangeEnemyTarget(Transform _newTarget)
    {
        target = _newTarget;
        return;
    }

    protected void SetTargetForFirstTime(string _enemyID)
    {
        switch(_enemyID)
        {
            case "Stealer":
                ChangeEnemyTarget(LevelManager.Instance.GetRandomHousePoint());
                break;

            case "Attacker":
                ChangeEnemyTarget(GameObject.Find("Spooky").transform);
                break;

            default:
                break;
        }
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
