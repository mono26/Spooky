﻿using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    public EnemyDetect enemyDetect;
    public EnemyAnimation animationComponent;
    [SerializeField]
    public Settings settings;
    [SerializeField]
    protected SpriteRenderer sprite;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected AudioClip soundEffect;

    private Coroutine ability;
    private bool isCasting;

    public float lastShoot;
    protected int currentHealth;

    protected bool IsCasting { get { return isCasting; } private set { isCasting = value; } }

    protected void Awake()
    {
        enemyDetect = new EnemyDetect(
            this.gameObject, 
            settings.EnemyDetectTrigger, 
            settings.ViewRange
            );

        animationComponent = new EnemyAnimation(sprite, animator);
    }

    protected void Start()
    {
        currentHealth = settings.MaxHealth;
    }

    public void StartCast(bool _cast)
    {
        IsCasting = _cast;
    }

    public void CastAbility(Coroutine _cast)
    {
        ability = _cast;
        StartCast(true);
    }

    protected bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, enemyDetect.GetCurrentTarget().transform.position);
        if (distance <= settings.ViewRange)
        {
            return true;
        }
        else return false;
    }

    protected bool IsDead()
    {
        if (currentHealth > 0)
        {
            return false;
        }
        else return true;
    }

    protected void LoseHealth(int _damage)
    {
        currentHealth -= _damage;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        enemyDetect.OnTriggerEnter(_collider);
    }

    protected void OnTriggerExit(Collider _collider)
    {
        enemyDetect.OnTriggerExit(_collider);
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackSpeed;
        [Range(3f, 5f)]
        public float ViewRange;
        public int MaxHealth;
        public SphereCollider EnemyDetectTrigger;
    }
}
