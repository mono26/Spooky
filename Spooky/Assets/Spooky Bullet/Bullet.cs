using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rigidBody;
    [SerializeField]
    protected int damage;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Launch(float _speed)
    {
        rigidBody.AddForce(transform.right * _speed, ForceMode.Impulse);
    }

    public int GetBulletDamage()
    {
        return damage;
    }
}
