using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    public void Launch(float _speed)
    {
        rigidBody.AddForce(transform.right * _speed, ForceMode.Impulse);
    }
}
