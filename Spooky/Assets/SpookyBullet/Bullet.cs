using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody rigidBody;
    public Rigidbody RigidBody { get { return rigidBody; } }
    [SerializeField]
    protected float bulletDamaga;
    protected float bulletLaunchTime;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    protected bool IsBulletTimerOver()
    {
        if (bulletLaunchTime + 10f < Time.timeSinceLevelLoad)
        {
            return true;
        }
        else return false;
    }

    public void Launch(float _speed)
    {
        rigidBody.AddForce(transform.right * _speed, ForceMode.Impulse);
        bulletLaunchTime = Time.timeSinceLevelLoad;
        return;
    }

    public float GetBulletDamage()
    {
        return bulletDamaga;
    }

    protected void Restart(Bullet _bullet)
    {
        _bullet.rigidBody.velocity = Vector3.zero;
        _bullet.transform.localScale = Vector3.one;
        return;
    }
}
