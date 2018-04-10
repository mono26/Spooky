using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Stealer : Enemy, ISpawnable<Enemy>
{
    [SerializeField]
    protected EnemyStates currentState;
    [SerializeField]
    protected CloseRangeAttack stealCorn;
    [SerializeField]
    public int stoleValue;
    public bool hasLoot = false;

    [SerializeField]
    private static List<Enemy> enemyList = new List<Enemy>();
    public List<Enemy> Pool { get { return enemyList; } }

    protected new void Awake()
    {
        base.Awake();

        stealCorn = new StealCorn(this);
        StatsComponent = EnemyStatsCollection.stealerStats;

        if (enemyList == null)
        {
            enemyList = new List<Enemy>();
        }
        else return;
    }

	// Use this for initialization
	protected new void OnEnable ()
    {
        base.OnEnable();

        ChangeTargetToHousePoint();
        currentState = EnemyStates.Moving;
    }
	
	// Update is called once per frame
	public void Update ()
    {
        if(DeathComponent.IsDead() && !currentState.Equals(EnemyStates.Death))
        {
            currentState = EnemyStates.Death;
            return;
        }

        if (currentState.Equals(EnemyStates.Moving))
        {
            if (IsTargetInRange())
            {
                currentState = EnemyStates.Stealing;
                return;
            }
            else return;
        }

        else if (currentState.Equals(EnemyStates.Stealing))
        {
            if(!IsCasting &&
                Time.timeSinceLevelLoad > lastAttackExecution + StatsComponent.basicCooldown)
            {
                stealCorn.CloseAttack();
                lastAttackExecution = Time.timeSinceLevelLoad;
                return;
            }

            if (hasLoot)
            {
                currentState = EnemyStates.Escaping;
                return;
            }
            else return;
        }

        else if (currentState.Equals(EnemyStates.Escaping))
        {
            if (!Target.CompareTag("Runaway Point"))
            {
                ChangeTargetToRunPoint();
                return;
            }

            if (IsTargetInRange())
            {
                base.ReleaseEnemy(); // Fires the vent so the wave spawner decreases the number off enemies.
                ReleaseStealer(this);
                return;
                // TODO check if it has loot on it and do something
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

    protected void FixedUpdate()
    {
        if (currentState.Equals(EnemyStates.Moving) || currentState.Equals(EnemyStates.Escaping))
            MovementComponent.FixedUpdate();
        else return;
    }

    protected void ChangeTargetToHousePoint()
    {
        SetEnemyTarget(LevelManager.Instance.GetRandomHousePoint());
        return;
    }

    protected void ChangeTargetToRunPoint()
    {
        SetEnemyTarget(LevelManager.Instance.GetRandomRunawayPoint());
        return;
    }

    public bool HasLoot(bool _hasLoot)
    {
        hasLoot = _hasLoot;
        return hasLoot;
    }

    protected IEnumerator StartDeath()
    {
        DeathComponent.IsDying = true;
        DeactivateCollider();

        // To make sure the dead damage animation is finished before the death one.
        yield return new WaitForSecondsRealtime(
                    AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        //AnimationComponent.PlayAnimation("Dead");

        yield return new WaitForSecondsRealtime(
                    AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        base.ReleaseEnemy();
        ReleaseStealer(this);
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
            Quaternion.Euler(90f,0f,0f)
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

    public void ReleaseStealer(Enemy _enemy)
    {
        _enemy.gameObject.SetActive(false);
        Pool.Add(_enemy);
        return;
    }
}
