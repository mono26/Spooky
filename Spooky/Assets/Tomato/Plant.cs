using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    public EnemyDetect enemyDetect;
    public PlantAnimation animationComponent;
    [SerializeField]
    public Settings settings;
    [SerializeField]
    protected AudioClip soundEffect;

    private Coroutine abilityCast;
    private bool isCasting;

    public float lastShoot;
    protected int currentHealth;

    protected bool IsCasting { get { return isCasting; } private set { isCasting = value; } }

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
        currentHealth = settings.maxHealth;
    }

    public void StartCastingAbility(bool _cast)
    {
        IsCasting = _cast;
    }

    public void CastAbility(Coroutine _abilityCast)
    {
        abilityCast = _abilityCast;
        StartCastingAbility(true);
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

    protected bool IsPlantDead()
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
        public int maxHealth;
    }
}
