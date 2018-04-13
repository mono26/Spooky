using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Enemy, ISpawnable<Enemy>
{
    [SerializeField]
    private EnemyStates currentState;
    protected CloseRangeAttack attackPlayer;

    [SerializeField]
    private static List<Enemy> enemyPool = new List<Enemy>();
    public List<Enemy> Pool { get { return enemyPool; } }

    new void Awake()
    {
        StatsComponent = GameManager.Instance.EnemyStats.AttackerStats;
        base.Awake();
        //attackPlayer = new StealCorn(this);

        if (enemyPool == null)
        {
            enemyPool = new List<Enemy>();
        }
        else return;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (DeathComponent.IsDead() && !currentState.Equals(EnemyStates.Death))
        {
            currentState = EnemyStates.Death;
            return;
        }

        if (currentState.Equals(EnemyStates.Moving))
        {
            if (IsTargetInRange())
            {
                currentState = EnemyStates.Attacking;
                return;
            }
            else return;
        }

        else if (currentState.Equals(EnemyStates.Attacking))
        {
            if (!IsCasting &&
                Time.timeSinceLevelLoad > lastAttackExecution + StatsComponent.basicCooldown)
            {
                attackPlayer.CloseAttack();
                lastAttackExecution = Time.timeSinceLevelLoad;
                return;
            }

            else return;
        }

        else if (currentState.Equals(EnemyStates.Death))
        {
            if (!DeathComponent.IsDying)
            {
                StopAllCoroutines();
                DeathComponent.DieProcess = StartCoroutine(StartDeath());
                return;
            }
            else return;
        }

        else return;
    }

    protected IEnumerator StartDeath()
    {
        DeathComponent.IsDying = true;
        DeactivateCollider();

        // To make sure the dead damage animation is finished before the death one.
        yield return new WaitForSecondsRealtime(
                    AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        AnimationComponent.PlayAnimation("Dead");

        yield return new WaitForSecondsRealtime(
                    AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        base.ReleaseEnemy();
        ReleaseAttacker(this);
        yield return 0;
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
        var parentPool = GameObject.Find("Enemies").transform;    //Can't store a transform inside a prefab. Ensure always a tranform Enemies on level.
        Stealer enemy = Instantiate(
            gameObject,
            parentPool.transform.position,
            Quaternion.Euler(90f, 0f, 0f)
            ).GetComponent<Stealer>();
        enemy.transform.SetParent(parentPool);
        Pool.Add(enemy);
        enemy.gameObject.SetActive(false);
        return;
    }

    private void SetEnemyPosition(Enemy _enemy, Transform target)
    {
        _enemy.transform.position = target.position;
        return;
    }

    public void ReleaseAttacker(Enemy _enemy)
    {
        _enemy.gameObject.SetActive(false);
        Pool.Add(_enemy);
        return;
    }
}
