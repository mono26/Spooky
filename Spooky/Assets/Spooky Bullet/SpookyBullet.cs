using System.Collections.Generic;
using UnityEngine;


public class SpookyBullet : Bullet, ISpawnable<Bullet>
{
    [SerializeField]
    private float damageIncreasePerSeconds = 0.25f;
    [SerializeField]
    private float sizeIncreasePerSeconds = 0.25f;

    [SerializeField]
    private static List<Bullet> bulletList = new List<Bullet>();
    public List<Bullet> Pool { get { return bulletList; } }

    public void IncreaseSize(SpookyBullet _bullet)
    {
        Vector3 newScale = _bullet.transform.localScale + new Vector3(sizeIncreasePerSeconds, sizeIncreasePerSeconds, 0f);
        newScale.x = Mathf.Clamp(newScale.x, 1, 3);
        newScale.y = Mathf.Clamp(newScale.y, 1, 3);
        var time = Time.deltaTime;
        Debug.Log(time + " Increazing size of bullet. " + newScale);
        _bullet.transform.localScale = newScale;   // 1 in z because z scale values is always constant
        return;
    }

    public void IncreaseDamage(SpookyBullet _bullet)
    {
        _bullet.damage += damageIncreasePerSeconds;
        _bullet.damage = Mathf.Clamp(_bullet.damage, 1, 3);
        return;
    }

    public Bullet Spawn(Transform _position)
    {
        if (Pool.Count == 0)
            AddToPool();
        Bullet bullet = Pool[Pool.Count - 1];
        Pool.RemoveAt(Pool.Count - 1);
        SetBulletPosition(bullet, _position);
        bullet.gameObject.SetActive(true);
        return bullet;
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
