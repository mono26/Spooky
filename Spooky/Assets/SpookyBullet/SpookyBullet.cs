using UnityEngine;


public class SpookyBullet : Bullet
{
    [SerializeField]
    private float damageIncreasePerSeconds = 1f;
    [SerializeField]
    private float sizeIncreasePerSeconds = 1f;

    public void IncreaseSize()
    {
        Vector3 newScale = transform.localScale + new Vector3(sizeIncreasePerSeconds * Time.deltaTime, sizeIncreasePerSeconds * Time.deltaTime, 0f);
        newScale.x = Mathf.Clamp(newScale.x, 1, 3);
        newScale.y = Mathf.Clamp(newScale.y, 1, 3);
        transform.localScale = newScale;   // 1 in z because z scale values is always constant
        return;
    }

    public void IncreaseDamage()
    {
        Debug.Log("Increasing bullet damage");
        bulletDamage += damageIncreasePerSeconds * Time.deltaTime;
        bulletDamage = Mathf.Clamp(bulletDamage, 1, 3);
        return;
    }

}
