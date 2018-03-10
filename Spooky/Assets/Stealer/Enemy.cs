using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, ISpawnable<Enemy>
{
    protected EnemyMovement movement;
    // TODO check if we have to set settings in constructor.
    public Settings settings;

    public ICloseRangeAttack basicAbility;
    [SerializeField]
    protected Coroutine ability;
    public Transform target;

    [SerializeField]
    protected float lastAttackExecution;
    protected int currentHealth;

    [SerializeField]
    readonly static Transform poolPosition;
    [SerializeField]
    readonly static List<Enemy> enemyList;
    public List<Enemy> Pool { get { return enemyList; } }
    public Transform PoolPosition { get { return poolPosition; } }

    protected void Awake()
    {
        movement = new EnemyMovement(this, settings.MovementSettings);
    }

    protected void Start()
    {
        currentHealth = settings.MaxHealth;
        movement.Start();
    }

    public bool IsDead()
    {
        if (currentHealth > 0)
        {
            return false;
        }
        else return true;
    }

    protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= settings.BasicRange)
        {
            return true;
        }
        else return false;
    }

    public void LoseHealth(int _damage) // TODO put inside a interface
    {
        currentHealth -= _damage;
    }

    public void CastAbility(Coroutine _cast)
    {
        ability = _cast;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Bullet"))
        {
            int damage = _collision.gameObject.GetComponent<Bullet>().GetBulletDamage();
            LoseHealth(damage);
        }
    }

    public Enemy Spawn(Transform _position)
    {
        if (Pool.Count == 0)
            AddToPool();
        Enemy enemy = Pool[Pool.Count - 1];
        Pool.RemoveAt(Pool.Count - 1);
        SetEnemyPosition(enemy, _position);
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    private void AddToPool()
    {
        Enemy enemy = Instantiate(gameObject, PoolPosition.position, PoolPosition.rotation).GetComponent<Enemy>();
        enemy.gameObject.SetActive(false);
        Pool.Add(enemy);
    }

    private void SetEnemyPosition(Enemy _enemy, Transform target)
    {
        _enemy.transform.position = target.position;
        _enemy.transform.rotation = target.rotation;
    }

    public void ReleaseEnemy(Enemy _enemy)
    {
        _enemy.gameObject.SetActive(false);
        Pool.Add(_enemy);
    }

    [System.Serializable]
    public class  Settings
    {
        public Rigidbody Rigidbody;
        public NavMeshAgent NavMesh;
        public float BasicCooldown;
        public float BasicRange;

        public int MaxHealth;

        [SerializeField]
        public EnemyMovement.Settings MovementSettings;
    }
}
