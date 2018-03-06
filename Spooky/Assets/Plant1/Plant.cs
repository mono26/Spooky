using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    protected SpookyEnemyDetect enemyDetect;
    [SerializeField]
    public Settings settings;
    public Coroutine ability;

    public Vector3 enemyDirection;
    protected float lastBasicExecute;
    protected int currentHealth;

    public virtual void Awake()
    {
        enemyDetect = new SpookyEnemyDetect(this.gameObject, settings.EnemyDetectTrigger, settings.EnemyDetectSettings);
    }

    public virtual void Start()
    {
        currentHealth = settings.MaxHealth;
    }

    public bool IsDead()
    {
        if (currentHealth > 0)
        {
            return false;
        }
        else return true;
    }

    private void LoseHealth(int _damage)
    {
        currentHealth -= _damage;
    }

    [System.Serializable]
    public class Settings
    {
        public Transform Target;
        public PlantAttack MainAbility;
        public float BasicCooldown;
        public float BasicRange;
        public int MaxHealth;
        public SphereCollider EnemyDetectTrigger;

        public SpookyEnemyDetect.Settings EnemyDetectSettings;
    }
}
