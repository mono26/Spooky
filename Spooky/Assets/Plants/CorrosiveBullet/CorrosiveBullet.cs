using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBullet : Bullet
{
    [SerializeField] float startTime;
    [SerializeField] float corrosiveDamage = 0.1f;
    [SerializeField] float tickPerSecond = 10f;    // Per second
    [SerializeField] float corrosiveDuration = 3.0f;   // In seconds
    [SerializeField] SpriteRenderer corrosiveFieldSprite = null;
    [SerializeField] Collider corrosiveFieldCollider = null;

    [Header("Debugging")]
    [SerializeField] List<Character> enemiesOnField;

    WaitForSeconds timeToDamage = null;
    Coroutine corrosiveEffect = null;

#region Unity Functions
    protected new void Awake()
    {
        base.Awake();

        timeToDamage = new WaitForSeconds(1 / tickPerSecond);

        EnterPoolEvent += StopEffect;
    }
#endregion

#region Custom Functions
    protected override void OnExitPool()
    {
        base.OnExitPool();

        bulletBody.constraints = RigidbodyConstraints.None;   //Because the rigidBody when it hits the enemy stays in rotation
        bulletBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        corrosiveFieldSprite.enabled = false;
        corrosiveFieldCollider.enabled = false;
    }

    protected override void OnEnterPool()
    {
        base.OnEnterPool();

        corrosiveFieldSprite.enabled = false;
        corrosiveFieldCollider.enabled = false;
        enemiesOnField.Clear();
    }

    protected void Update()
    {
        for (int i = 0; i < enemiesOnField.Count; i++)
        {
            if (enemiesOnField[i].Equals(null) || !enemiesOnField[i].gameObject.activeInHierarchy)
            {
                enemiesOnField.Remove(enemiesOnField[i]);
            }
        }
    }

    protected IEnumerator CorrosiveEffect()
    {
        startTime = Time.timeSinceLevelLoad;
        while(Time.timeSinceLevelLoad < startTime + corrosiveDuration)
        {
            foreach(Character enemy in enemiesOnField)
            {
                var healthComponent = enemy.GetComponent<Health>();
                if(healthComponent)
                {
                    healthComponent.TakeDamage(corrosiveDamage);
                }

            }
            yield return timeToDamage;
        }
        PoolsManager.ReturnObjectToPools(this);
    }

    private void AddEnemy(Character enemy)
    {
        if (!enemy)
        {
            return;
        }

        if (!enemiesOnField.Contains(enemy))
        {
            enemiesOnField.Add(enemy);
        }
    }

    private void RemoveEnemy(Character enemy)
    {
        if (!enemy)
        {
            return;
        }

        if (enemiesOnField.Contains(enemy))
        {
            enemiesOnField.Remove(enemy);
        }
    }

    private void StopEffect()
    {
        StopCoroutine(corrosiveEffect);
    }

    protected override void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            bulletSprite.enabled = false;
            corrosiveFieldSprite.enabled = true;
            // So the only collider activated is the trigger for the area damage.
            bulletCollider.enabled = false;
            corrosiveFieldCollider.enabled = true;
            bulletBody.velocity = Vector3.zero;
            // TODO fin better way to do this.
            bulletBody.constraints = RigidbodyConstraints.FreezeRotationZ;
            // Standar rotation for every object in the game!
            transform.localRotation = Quaternion.Euler(new Vector3(90,0,0));

            corrosiveEffect = StartCoroutine(CorrosiveEffect());
        }
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        foreach (string tag in damageComponent.DamageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                AddEnemy(_collider.GetComponent<Character>());
            }
        }
    }

    protected void OnTriggerStay(Collider _collider)
    {
        foreach (string tag in damageComponent.DamageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                AddEnemy(_collider.GetComponent<Character>());
            }
        }
    }

    protected void OnTriggerExit(Collider _collider)
    {
        foreach (string tag in damageComponent.DamageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                RemoveEnemy(_collider.GetComponent<Character>());
            }
        }
    }
#endregion
}
