using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField]
    protected string[] damageTags;
    public string[] DamageTags { get { return damageTags; } }

    [SerializeField]
    protected float damageOnTouch;

    protected virtual void OnCollisionEnter(Collision _collision)
    {
        foreach(string tag in damageTags)
        {
            if(_collision.gameObject.CompareTag(tag))
            {
                Health damagaleComponent = _collision.gameObject.GetComponent<Health>();
                if (damagaleComponent != null)
                {
                    CollidingWithDamagable(damagaleComponent);
                }
                else return;
            }
        }
    }

    protected virtual void CollidingWithDamagable(Health _damagable)
    {
        _damagable.TakeDamage(damageOnTouch);
        return;
    }

    public void SetDamageOnTouch(float _newDamage)
    {
        damageOnTouch = _newDamage;
        return;
    }
}
