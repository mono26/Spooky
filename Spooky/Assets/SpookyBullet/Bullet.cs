using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody rigidBody;
    public Rigidbody RigidBody { get { return rigidBody; } }
    [SerializeField]
    protected float damage;
    protected float launchTime;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    protected bool IsBulletTimerOver()
    {
        if (launchTime + 10f < Time.timeSinceLevelLoad)
        {
            return true;
        }
        else return false;
    }

    public void Launch(float _speed)
    {
        rigidBody.AddForce(transform.right * _speed, ForceMode.Impulse);
        launchTime = Time.timeSinceLevelLoad;
        return;
    }

    public float GetBulletDamage()
    {
        return damage;
    }

    protected void Restart(Bullet _bullet)
    {
        _bullet.rigidBody.velocity = Vector3.zero;
        _bullet.transform.localScale = Vector3.one;
        return;
    }
}
