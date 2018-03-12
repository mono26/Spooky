using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody rigidBody;

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

    void OnCollisionEnter(Collision _collision)
    {
        if(_collision.gameObject.CompareTag("Enemy"))
            Destroy(this.gameObject);
    }
}
