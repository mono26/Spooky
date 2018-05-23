using System.Collections;
using UnityEngine;

public class CorrosiveBullet : Bullet
{
    [SerializeField]
    protected float startTime;
    [SerializeField]
    protected float corrosiveDamage = 0.5f;
    [SerializeField]    // Per second
    protected float corrosiveTickRate = 0.5f;
    [SerializeField]    // In seconds
    protected float corrosiveDuration = 5.0f;
    [SerializeField]
    protected GameObject corrosiveField;

    public delegate void CorrosiveDamage(float _damage);
    public event CorrosiveDamage OnCorrosiveDamage;

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

        base.OnDisable();

        return;
    }

    protected IEnumerator CorrosiveEffect()
    {
        corrosiveField.SetActive(true);
        startTime = Time.timeSinceLevelLoad;
        while(Time.timeSinceLevelLoad < startTime + corrosiveDuration)
        {
            if (OnCorrosiveDamage != null)
            {
                OnCorrosiveDamage(corrosiveDamage);
                yield return new WaitForSeconds(1 / corrosiveTickRate);
            }
            else yield return new WaitForSeconds(1 / corrosiveTickRate);
        }

        poolable.Release();
        yield return 0;
    }

    private void AddEnemy(Enemy _enemy)
    {
        OnCorrosiveDamage += _enemy.HealthComponent.TakeDamage;
        return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        OnCorrosiveDamage -= _enemy.HealthComponent.TakeDamage;
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
        if (_collider.CompareTag("Enemy"))
        {
            AddEnemy(_collider.GetComponent<Enemy>());
            return;
        }
        else return;
    }

    protected void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            RemoveEnemy(_collider.GetComponent<Enemy>());
            return;
        }
        else return;
    }
}
