using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(DamageOnTouch))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : PoolableObject
{
    [SerializeField]
    protected float bulletDamage;
    [SerializeField]
    protected GameObject hitVfx;

    protected BoxCollider bulletCollider;
    public BoxCollider BulletCollider { get { return bulletCollider; } }
    protected Rigidbody bulletBody;
    public Rigidbody BulletBody { get { return bulletBody; } }
    protected SpriteRenderer bulletSprite;
    public SpriteRenderer BulletSprite { get { return bulletSprite; } }
    protected DamageOnTouch damageComponent;
    public DamageOnTouch DamageComponent { get { return damageComponent; } }

    protected PoolableObject poolable;

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

    private void CreateVisualEffect(GameObject _effect)
    {
        if (_effect != null)
        {
            Instantiate(_effect, transform.position, transform.rotation);
        }
        return;
    }

    protected virtual void OnCollisionEnter(Collision _collision)
    {
        foreach(string tag in damageComponent.DamageTags)
        {
            if (_collision.gameObject.CompareTag(tag))
            {
                CreateVisualEffect(hitVfx);
                poolable.Release();
                return;
            }
        }
    }
}
