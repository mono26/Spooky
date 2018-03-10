using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBullet : Bullet
{
    [SerializeField]
    private float startTime;

    [SerializeField]
    protected int corrosiveDamage;
    [SerializeField]    // Per second
    protected float corrosiveTickRate;
    [SerializeField]
    protected float corrosiveRange;
    [SerializeField]    // In seconds
    protected float corrosiveDuration;
    [SerializeField]
    protected GameObject corrosiveTrigger;
    [SerializeField]
    protected GameObject specialEffect;

    protected List<Enemy> enemyList;

    protected new void Awake()
    {
        base.Awake();

        rigidBody.constraints = RigidbodyConstraints.None;   //Because the rigidBody when it hits the enemy stays in rotation
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        corrosiveTrigger.SetActive(false);
    }
    protected IEnumerator CorrosiveEffect()
    {
        corrosiveTrigger.SetActive(true);
        startTime = Time.timeSinceLevelLoad;
        while(Time.timeSinceLevelLoad < startTime + corrosiveDuration)
        {
            if (enemyList.Count > 0)
            {
                for (int i = 0; i < enemyList.Count; i++) //TODO With a foreach???
                {
                    enemyList[i].GetComponent<Enemy>().LoseHealth(corrosiveDamage);
                    yield return new WaitForSeconds(1 / corrosiveTickRate);
                }
            }
            else yield return new WaitForSeconds(1 / corrosiveTickRate);
        }
    }

    private void AddEnemy(Enemy _enemy)
    {
        if (!enemyList.Contains(_enemy))
        {
            enemyList.Add(_enemy);
            return;
        }
        else return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        if (enemyList.Contains(_enemy))
        {
            enemyList.Remove(_enemy);
            return;
        }
        else return;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        GetComponent<Collider>().enabled = false;   // So the only collidr activated is the trigger for the area damage.
        rigidBody.velocity = Vector3.zero;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ;   // TODO fin better way to do this.
        StartCoroutine(CorrosiveEffect());
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            AddEnemy(_collider.GetComponent<Enemy>());
        }
        else return;
    }

    protected void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            RemoveEnemy(_collider.GetComponent<Enemy>());
        }
        else return;
    }
}
