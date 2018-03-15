using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Stealer : Enemy, ISpawnable<Enemy>
{
    [SerializeField]
    private State currentState;
    [SerializeField]
    public int stoleValue;
    public bool hasLoot = false;

    [SerializeField]
    private static List<Enemy> enemyList = new List<Enemy>();
    public List<Enemy> Pool { get { return enemyList; } }

    public enum State
    {
        Moving,
        Stealing,
        Escaping,
        Death,
    }

    protected new void Awake()
    {
        base.Awake();

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

        basicAbility = new Steal(this);
        ChangeTargetToHousePoint();
        currentState = State.Moving;
    }
	
	// Update is called once per frame
	public void Update ()
    {
        if(IsDead())
        {
            currentState = State.Death;
        }

        if (currentState.Equals(State.Moving))
        {
            if (IsTargetInRange())
            {
                currentState = State.Stealing;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Stealing))
        {
            if(!IsCasting &&
                Time.timeSinceLevelLoad > lastAttackExecution + settings.BasicCooldown)
            {
                basicAbility.CloseAttack();
                lastAttackExecution = Time.timeSinceLevelLoad;
            }

            if (hasLoot)
            {
                currentState = State.Escaping;
                return;
            }
            else return;
        }

        else if (currentState.Equals(State.Escaping))
        {
            if (!target.CompareTag("Runaway Point"))
            {
                ChangeTargetToRunPoint();
            }

            if (IsTargetInRange())
            {
                ReleaseEnemy(); // Fires the vent so the wave spawner decreases the number off enemies.
                ReleaseStealer(this);
                // TODO check if it has loot on it and do something
            }
            else return;
        }

        else if (currentState.Equals(State.Death))
        {
            StartCoroutine(Die());
        }

        else return;
    }

    protected void FixedUpdate()
    {
        if (currentState.Equals(State.Moving) || currentState.Equals(State.Escaping))
            movementComponent.FixedUpdate();
        else return;
    }

    protected void ChangeTargetToHousePoint()
    {
        target = LevelManager.Instance.GetRandomHousePoint();
    }

    protected void ChangeTargetToRunPoint()
    {
        target = LevelManager.Instance.GetRandomRunawayPoint();
    }

    public bool HasLoot(bool _hasLoot)
    {
        hasLoot = _hasLoot;
        return hasLoot;
    }

    protected IEnumerator Die()
    {
        ReleaseEnemy();

        animationComponent.PlayAnimation("Dead");

        yield return new WaitForSecondsRealtime(
                    animationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        ReleaseStealer(this);
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
        var parentPool = GameObject.Find("Enemies");    //Can't store a transform inside a prefab. Ensure always a tranform Enemies on level.
        Stealer enemy = Instantiate(
            gameObject, 
            parentPool.transform.position, 
            Quaternion.Euler(90f,0f,0f)
            ).GetComponent<Stealer>();
        enemy.gameObject.SetActive(false);
        Pool.Add(enemy);
    }

    private void SetEnemyPosition(Enemy _enemy, Transform target)
    {
        _enemy.transform.position = target.position;
    }

    public void ReleaseStealer(Enemy _enemy)
    {
        _enemy.gameObject.SetActive(false);
        Pool.Add(_enemy);
    }
}
