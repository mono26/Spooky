using System.Collections.Generic;
using UnityEngine;

public class TomatoBullet : Bullet, ISpawnable<Bullet>
{
    [SerializeField]
    private static List<Bullet> bulletList = new List<Bullet>();
    public List<Bullet> Pool { get { return bulletList; } }

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
            ReleaseBullet(this);
    }

}
