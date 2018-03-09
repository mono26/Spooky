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

    protected void Awake()
    {
        enemyDetect = new EnemyDetect(
            this.gameObject, 
            settings.EnemyDetectTrigger, 
            settings.ViewRange
            );

        var time = Time.timeSinceLevelLoad;
        Debug.Log(string.Format("{0} llamando al padre ", time));
    }

    protected void Start()
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

    protected void OnTriggerEnter(Collider _collider)
    {
        enemyDetect.OnTriggerEnter(_collider);
    }

    protected void OnTriggerExit(Collider _collider)
    {
        enemyDetect.OnTriggerExit(_collider);
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
