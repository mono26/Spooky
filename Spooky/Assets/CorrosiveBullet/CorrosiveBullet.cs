using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBullet : Bullet
{
    [SerializeField]
    protected float startTime;
    [SerializeField]
    protected float corrosiveDamage = 0.1f;
    [SerializeField]    // Per second
    protected float tickPerSecond = 10f;
    [SerializeField]    // In seconds
    protected float corrosiveDuration = 3.0f;
    [SerializeField]
    protected GameObject corrosiveField;

    [SerializeField]
    protected List<Enemy> enemiesOnField;

    protected new void Awake()
    {
        base.Awake();
        return;
    }

    protected override void OnEnable()
    {
        bulletBody.constraints = RigidbodyConstraints.None;   //Because the rigidBody when it hits the enemy stays in rotation
        bulletBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        bulletSprite.enabled = true;
        bulletCollider.enabled = true;
        corrosiveField.SetActive(false);

        poolable.OnRelease += StopEffect;

        base.OnEnable();

        return;
    }

    protected override void OnDisable()
    {
        poolable.OnRelease -= StopEffect;
        enemiesOnField.Clear();

        base.OnDisable();

        return;
    }

    protected void Update()
    {
        for (int i = 0; i < enemiesOnField.Count; i++)
        {
            if (!enemiesOnField[i].gameObject.activeInHierarchy)
                enemiesOnField.Remove(enemiesOnField[i]);
        }
        return;
    }

    protected IEnumerator CorrosiveEffect()
    {
        corrosiveField.SetActive(true);
        startTime = Time.timeSinceLevelLoad;
        while(Time.timeSinceLevelLoad < startTime + corrosiveDuration)
        {
            foreach(Enemy enemy in enemiesOnField)
            {
                enemy.HealthComponent.TakeDamage(corrosiveDamage);
            }
            yield return new WaitForSeconds(1 / tickPerSecond);
        }
        poolable.Release();
        yield break;
    }

    private void AddEnemy(Enemy _enemy)
    {
        enemiesOnField.Add(_enemy);
        return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        enemiesOnField.Remove(_enemy);
        return;
    }

    private void StopEffect()
    {
        StopCoroutine(CorrosiveEffect());
        return;
    }

    protected override void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;   // So the only collidr activated is the trigger for the area damage.
            bulletBody.velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(new Vector3(90,0,0));    // Standar rotation for every object in the game!
            bulletBody.constraints = RigidbodyConstraints.FreezeRotationZ;   // TODO fin better way to do this.
            StartCoroutine(CorrosiveEffect());
            return;
        }
        return;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        foreach (string tag in damageComponent.DamageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                if (!enemiesOnField.Contains(_collider.GetComponent<Enemy>()))
                    AddEnemy(_collider.GetComponent<Enemy>());
            }
        }
        return;
    }

    protected void OnTriggerStay(Collider _collider)
    {
        foreach (string tag in damageComponent.DamageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                if(!enemiesOnField.Contains(_collider.GetComponent<Enemy>()))
                    AddEnemy(_collider.GetComponent<Enemy>());
            }
        }
        return;
    }

    protected void OnTriggerExit(Collider _collider)
    {
        foreach (string tag in damageComponent.DamageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                RemoveEnemy(_collider.GetComponent<Enemy>());

            }
        }
        return;
    }
}
