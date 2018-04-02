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

        settings.basicAbility = new Steal(this);
        ChangeTargetToHousePoint();
        currentState = State.Moving;
    }
	
	// Update is called once per frame
	public void Update ()
    {
        if(IsDead() && !currentState.Equals(State.Death))
        {
            currentState = State.Death;
            return;
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
                settings.basicAbility.CloseAttack();
                lastAttackExecution = Time.timeSinceLevelLoad;
                return;
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
            if (!Target.CompareTag("Runaway Point"))
            {
                ChangeTargetToRunPoint();
                return;
            }

            if (IsTargetInRange())
            {
                base.Die(); // Fires the vent so the wave spawner decreases the number off enemies.
                ReleaseStealer(this);
                return;
                // TODO check if it has loot on it and do something
            }
            else return;
        }

        else if (currentState.Equals(State.Death))
        {
            Debug.Log("Is dead!");
            // So it can execute the Coroutine just once!
            if (!isDying)
            {
                Debug.Log("Start dead process");
                StopAllCoroutines();
                dieProcess = StartCoroutine(StartDeath());
                return;
            }
            else return;
        }

        else return;
    }

    protected void FixedUpdate()
    {
        if (currentState.Equals(State.Moving) || currentState.Equals(State.Escaping))
            MovementComponent.FixedUpdate();
        else return;
    }

    protected void ChangeTargetToHousePoint()
    {
        SetTarget(LevelManager.Instance.GetRandomHousePoint());
        return;
    }

    protected void ChangeTargetToRunPoint()
    {
        SetTarget(LevelManager.Instance.GetRandomRunawayPoint());
        return;
    }

    public bool HasLoot(bool _hasLoot)
    {
        hasLoot = _hasLoot;
        return hasLoot;
    }

    protected IEnumerator StartDeath()
    {
        var time = Time.timeSinceLevelLoad;
        Debug.Log(time + " start death. " + this.ToString());

        isDying = true;
        DeactivateCollider();

        // To make sure the dead damage animation is finished before the death one.
        yield return new WaitForSecondsRealtime(
                    AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        AnimationComponent.PlayAnimation("Dead");

        yield return new WaitForSecondsRealtime(
                    AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
                    );

        base.Die();
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
