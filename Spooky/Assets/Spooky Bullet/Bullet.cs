using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rigidBody;
    [SerializeField]
    protected float damage;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Launch(float _speed)
    {
        rigidBody.AddForce(transform.right * _speed, ForceMode.Impulse);
    }

    public float GetBulletDamage()
    {
        return damage;
    }

    protected void Restart(Bullet _bullet)
    {
        _bullet.rigidBody.velocity = Vector3.zero;
        _bullet.transform.localScale = Vector3.one;
    }
}
