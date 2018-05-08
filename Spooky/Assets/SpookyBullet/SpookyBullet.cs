using UnityEngine;


public class SpookyBullet : Bullet
{
    [SerializeField]
    private float damageIncreasePerSeconds = 0.25f;
    [SerializeField]
    private float sizeIncreasePerSeconds = 0.25f;

    public void IncreaseSize(SpookyBullet _bullet)
    {
        Vector3 newScale = _bullet.transform.localScale + new Vector3(sizeIncreasePerSeconds, sizeIncreasePerSeconds, 0f);
        newScale.x = Mathf.Clamp(newScale.x, 1, 3);
        newScale.y = Mathf.Clamp(newScale.y, 1, 3);
        _bullet.transform.localScale = newScale;   // 1 in z because z scale values is always constant
        return;
    }

    public void IncreaseDamage(SpookyBullet _bullet)
    {
        _bullet.bulletDamaga += damageIncreasePerSeconds;
        _bullet.bulletDamaga = Mathf.Clamp(_bullet.bulletDamaga, 1, 3);
        return;
    }

    protected void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Enemy"))
        {
            Release();
            return;
        }
    }
}
