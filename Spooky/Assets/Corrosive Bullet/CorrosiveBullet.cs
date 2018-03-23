using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBullet : Bullet, ISpawnable<Bullet>
{
    [SerializeField]
    protected float startTime;
    [SerializeField]
    protected float corrosiveDamage;
    [SerializeField]    // Per second
    protected float corrosiveTickRate;
    [SerializeField]    // In seconds
    protected float corrosiveDuration;
    [SerializeField]
    protected GameObject corrosiveTrigger;

    public delegate void CorrosiveDamage(float _damage);
    public event CorrosiveDamage OnCorrosiveDamage;

    [SerializeField]
    private static List<Bullet> bulletList = new List<Bullet>();
    public List<Bullet> Pool { get { return bulletList; } }

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
            if (OnCorrosiveDamage != null)
            {
                OnCorrosiveDamage(corrosiveDamage);
                yield return new WaitForSeconds(1 / corrosiveTickRate);
            }
            else yield return new WaitForSeconds(1 / corrosiveTickRate);
        }

        ReleaseBullet(this);
    }

    private void AddEnemy(Enemy _enemy)
    {
        OnCorrosiveDamage += _enemy.healthComponent.TakeDamage;
        return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        OnCorrosiveDamage -= _enemy.healthComponent.TakeDamage;
        return;
    }

    public Bullet Spawn(Transform _position)
    {
        if (Pool.Count == 0)
            AddToPool();
        Bullet enemy = Pool[Pool.Count - 1];
        Pool.RemoveAt(Pool.Count - 1);
        SetBulletPosition(enemy, _position);
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    private void AddToPool()
    {
        var parentPool = GameObject.Find("Bullets");    //Can't store a transform inside a prefab. Ensure always a tranform Enemies on level.
        Bullet bullet = Instantiate(
            gameObject,
            parentPool.transform.position,
            Quaternion.Euler(90f, 0f, 0f)
            ).GetComponent<Bullet>();
        bullet.gameObject.SetActive(false);
        Pool.Add(bullet);
    }

    private void SetBulletPosition(Bullet _bullet, Transform target)
    {
        _bullet.transform.position = target.position;
    }

    public void ReleaseBullet(Bullet _bullet)
    {
        Restart(_bullet);
        _bullet.gameObject.SetActive(false);
        Pool.Add(_bullet);
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<Collider>().enabled = false;   // So the only collidr activated is the trigger for the area damage.
            rigidBody.velocity = Vector3.zero;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationZ;   // TODO fin better way to do this.
            StartCoroutine(CorrosiveEffect());
        }
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
