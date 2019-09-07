using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(DamageOnTouch))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : PoolableObject
{
    [Header("Settings")]
    [SerializeField]
    protected float bulletDamage;
    [SerializeField]
    protected GameObject hitVfx;

    protected BoxCollider bulletCollider = null;
    public BoxCollider GetBulletCollider { get { return bulletCollider; } }
    protected Rigidbody bulletBody = null;
    public Rigidbody GetBulletBody { get { return bulletBody; } }
    protected SpriteRenderer bulletSprite = null;
    public SpriteRenderer GetBulletSprite { get { return bulletSprite; } }
    protected DamageOnTouch damageComponent = null;
    public DamageOnTouch GetDamageComponent { get { return damageComponent; } }
    protected ParticleSystem bulletVFX = null;

    private float bulletLaunchTime;

#region Unity Functions
    protected override void Awake()
    {
        base.Awake();

        bulletBody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<BoxCollider>();
        bulletSprite = GetComponent<SpriteRenderer>();
        damageComponent = GetComponent<DamageOnTouch>();
        bulletVFX = GetComponent<ParticleSystem>();

        EnterPoolEvent += OnEnterPool;
    }

    protected virtual void Start()
    {
        damageComponent.SetDamageOnTouch(bulletDamage);
    }

    void OnDestroy()
    {
        EnterPoolEvent -= OnEnterPool;
    }
#endregion

#region Custom Functions
    // Launchs the bullet with force.
    public void Launch(float force)
    {
        bulletBody.AddForce(transform.right * force, ForceMode.Impulse);
        bulletLaunchTime = Time.timeSinceLevelLoad;
    }

    // Called when the bullet enter the pool. Here the bullet should enter a "sleep" state to prevent actions in the scene.
    protected virtual void OnEnterPool()
    {
        bulletBody.velocity = Vector3.zero;
        bulletCollider.enabled = false;
        bulletSprite.enabled = false;
        bulletVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Called when the bullet leaves the pool. Here the bullet should enter a "awoke" state and starts performing actions in the scene.
    protected virtual void OnExitPool()
    {
        bulletCollider.enabled = true;
        bulletSprite.enabled = true;
        bulletVFX.Play(true);
    } 

    protected virtual void OnCollisionEnter(Collision _collision)
    {
        foreach(string tag in damageComponent.DamageTags)
        {
            if (_collision.gameObject.CompareTag(tag))
            {
                VisualEffects.CreateVisualEffect(hitVfx, transform);
                PoolsManager.ReturnObjectToPools(this);
            }
        }
    }
#endregion
}
