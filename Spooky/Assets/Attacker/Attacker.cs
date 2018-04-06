using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Enemy, ISpawnable<Enemy>
{
    public enum State
    {
        Moving,
        Attacking,
        Death,
    }

    [SerializeField]
    private State currentState;

    [SerializeField]
    private static List<Enemy> enemyList = new List<Enemy>();
    public List<Enemy> Pool { get { return enemyList; } }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
