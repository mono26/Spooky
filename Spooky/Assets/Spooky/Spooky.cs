using UnityEngine;

public class Spooky : MonoBehaviour
{
    // Common section
    [Header("Common Settings")]
    public Joystick joystick;

    // Movement section
    [Header("Movement Settings")]
    public Rigidbody Rigidbody;
    private SpookyMovement movementComponent;

    // Detection Section
    [Header("Detection Settings")]
    public SphereCollider EnemyDetectTrigger;
    public SphereCollider PlantPointDetectTrigger;
    [Range(3f,4f)]
    public float EnemyDetectionRange;
    [Range(1.5f, 2f)]
    public float PlantPointDetectionRange;
    [SerializeField]
    public SpookyDetect enemyDetectComponent;
    private PlantPointDetect plantPointDetect;

    // Animation section
    [Header("Animation Settings")]
    public SpriteRenderer SpookySprite;
    public SpriteRenderer HandSprite;
    public Animator Animator;
    public SpookyAnimation animationComponent;

    // Attack section
    [Header("Attack Settings")]
    public SpookyBullet bulletPrefab;
    public Transform hand;
    public Transform shootTransform;
    [SerializeField]
    private SpookyAttack attackComponent;

    // This is going to be the class the connects all the components of spooky
    // Execute update, triggerevetns, awake, start, etc.

    public Settings settings;

    public delegate void FireButtonPress();
    public event FireButtonPress OnFireButtonPress;

    public delegate void FireButtonRelease();
    public event FireButtonPress OnFireButtonRelease;

    public delegate void InFightWithEnemy();
    public event InFightWithEnemy OnFightWithEnemy;

    private void Awake()
    {
        movementComponent = new SpookyMovement(
            this,
            settings.MovementSettings
            );

        enemyDetectComponent = new SpookyDetect(
            gameObject,
            EnemyDetectTrigger,
            EnemyDetectionRange
            );

        attackComponent = new SpookyAttack(
            this,
            bulletPrefab,
            settings.AttackSettings
            );

        plantPointDetect = new PlantPointDetect(
            this,
            PlantPointDetectTrigger,
            PlantPointDetectionRange
            );

        animationComponent = new SpookyAnimation(
            SpookySprite,
            HandSprite,
            Animator
            );
    }

    private void OnEnable()
    {
        attackComponent.OnEnable();
    }

    private void OnDisable()
    {
        attackComponent.OnDisable();
    }

    private void Start()
    {
        // Execute all Start() functions in the components. Here is variable setting.
        movementComponent.Start();
    }

    private void Update()
    {
        attackComponent.Update();
        enemyDetectComponent.Detect();
    }

    private void FixedUpdate()
    {
        movementComponent.FixedUpdate();
    }

    public void OnTriggerEnter(Collider _collider)
    {
        enemyDetectComponent.OnTriggerEnter(_collider);
        plantPointDetect.OnTriggerEnter(_collider);
    }

    public void OnTriggerExit(Collider _collider)
    {
        enemyDetectComponent.OnTriggerExit(_collider);
        plantPointDetect.OnTriggerExit(_collider);
    }

    public void StartCharge()
    {
        if (OnFireButtonPress != null)
            OnFireButtonPress();
    }

    public void StopCharge()
    {
        if (OnFireButtonRelease != null)
            OnFireButtonRelease();
    }

    [System.Serializable]
    public class Settings
    {
        public SpookyMovement.Settings MovementSettings;
        public SpookyAttack.Settings AttackSettings;
    }
}
