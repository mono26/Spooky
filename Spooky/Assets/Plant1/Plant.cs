using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    protected EnemyDetect enemyDetect;
    [SerializeField]
    public Settings settings;

    public Coroutine ability;

    public Vector3 enemyDirection;
    protected int currentHealth;

    public virtual void Awake()
    {
        enemyDetect = new EnemyDetect(
            this.gameObject, 
            settings.EnemyDetectTrigger, 
            settings.Range
            );
    }

    public virtual void Start()
    {
        currentHealth = settings.MaxHealth;
    }

    /*protected bool IsNextToTarget()
    {
        float distance = Vector3.Distance(transform.position, settings.Target.position);
        if (distance <= settings.Range)
        {
            return true;
        }
        else return false;
    }*/

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

    [System.Serializable]
    public class Settings
    {
        public float AttackSpeed;
        public float Range;
        public int MaxHealth;
        public SphereCollider EnemyDetectTrigger;
    }
}
