using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(DamageOnTouch))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : PoolableObject
{
    protected BoxCollider bulletCollider;
    public BoxCollider BulletCollider { get { return bulletCollider; } }
    protected Rigidbody bulletBody;
    public Rigidbody BulletBody { get { return bulletBody; } }
    protected SpriteRenderer bulletSprite;
    public SpriteRenderer BulletSprite { get { return bulletSprite; } }
    protected DamageOnTouch damageComponent;
    public DamageOnTouch DamageComponent { get { return damageComponent; } }

    protected PoolableObject poolable;

    [SerializeField]
    protected float bulletDamage;
    private float bulletLaunchTime;

    protected virtual void Awake()
    {
        bulletBody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<BoxCollider>();
        bulletSprite = GetComponent<SpriteRenderer>();
        damageComponent = GetComponent<DamageOnTouch>();
        poolable = GetComponent<PoolableObject>();

        return;
    }

    protected virtual void Start()
    {
        damageComponent.SetDamageOnTouch(bulletDamage);
        return;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnRelease += Restart;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnRelease -= Restart;
    }

    public void Launch(float _speed)
    {
        bulletBody.AddForce(transform.right * _speed, ForceMode.Impulse);
        bulletLaunchTime = Time.timeSinceLevelLoad;
        damageComponent.SetDamageOnTouch(bulletDamage);
        return;
    }

    protected virtual void Restart()
    {
        bulletBody.velocity = Vector3.zero;
        transform.localScale = Vector3.one;
        return;
    }

    protected virtual void OnCollisionEnter(Collision _collision)
    {
        foreach(string tag in damageComponent.DamageTags)
        {
            if (_collision.gameObject.CompareTag(tag))
            {
                poolable.Release();
                return;
            }
        }
    }
}
