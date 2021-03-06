﻿using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField]
    protected string[] damageTags;
    public string[] DamageTags { get { return damageTags; } }

    [SerializeField]
    protected float damageOnTouch;

    protected virtual void CollidingWithHealthComponent(Health _damagable)
    {
        _damagable.TakeDamage(damageOnTouch);
        return;
    }

    public void SetDamageOnTouch(float _newDamage)
    {
        damageOnTouch = _newDamage;
        return;
    }

    protected virtual void OnCollisionEnter(Collision _collision)
    {
        foreach(string tag in damageTags)
        {
            if(_collision.gameObject.CompareTag(tag))
            {
                Health damagaleComponent = _collision.gameObject.GetComponent<Health>();
                if (damagaleComponent != null)
                {
                    CollidingWithHealthComponent(damagaleComponent);
                }
            }
        }
        return;
    }

    protected virtual void OnTriggerEnter(Collider _collider)
    {
        foreach (string tag in damageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                Health damagaleComponent = _collider.gameObject.GetComponent<Health>();
                if (damagaleComponent != null)
                {
                    CollidingWithHealthComponent(damagaleComponent);
                }
            }
        }
        return;
    }
}
