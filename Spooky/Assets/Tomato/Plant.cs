using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    protected EnemyDetect enemyDetect;
    [SerializeField]
    public Settings settings;
    [SerializeField]
    protected Coroutine ability;

    public float timeSinceLastShoot;
    public Vector3 enemyDirection;
    protected int currentHealth;

    public virtual void Awake()
    {
        enemyDetect = new EnemyDetect(
            this.gameObject, 
            settings.EnemyDetectTrigger, 
            settings.ViewRange
            );
    }

    public virtual void Start()
    {
        currentHealth = settings.MaxHealth;
    }

    protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, enemyDetect.GetCurrentTarget().transform.position);
        if (distance <= settings.ViewRange)
        {
            return true;
        }
        else return false;
    }

    protected bool IsDead()
    {
        if (currentHealth > 0)
        {
            return false;
        }
        else return true;
    }

    protected void LoseHealth(int _damage)
    {
        currentHealth -= _damage;
    }

    public void CastAbility(Coroutine _cast)
    {
        ability = _cast;
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackSpeed;
        public float ViewRange;
        public int MaxHealth;
        public SphereCollider EnemyDetectTrigger;
    }
}
