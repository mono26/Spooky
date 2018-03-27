using UnityEngine;

public class Spooky : MonoBehaviour
{
    // Common section
    [Header("Common Settings")]
    [SerializeField]
    private Joystick joystick;
    public Joystick Joystick { get { return joystick; } }

    // Movement section
    [Header("Movement Settings")]
    private SpookyMovement movementComponent;

    // Detection Section
    [Header("Detection Settings")]
    [SerializeField]
    private EnemyDetect enemyDetectComponent;
    public EnemyDetect EnemyDetectComponent { get { return enemyDetectComponent; } }
    [SerializeField]
    private PlantPointDetect plantPointDetect;

    // Animation section
    [Header("Animation Settings")]
    private SpookyAnimation animationComponent;
    public SpookyAnimation AnimationComponent { get { return animationComponent; } }

    // Attack section
    [Header("Attack Settings")]
    [SerializeField]
    private SpookyBullet bulletPrefab;
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
            GetComponent<Rigidbody>(),
            settings.MovementSettings
            );

        enemyDetectComponent = new EnemyDetect(
            gameObject,
            settings.EnemyDetectSettings
            );

        attackComponent = new SpookyAttack(
            this,
            GameObject.FindGameObjectWithTag("SpookyHand").transform,
            GameObject.FindGameObjectWithTag("SpookyShootPoint").transform,
            bulletPrefab,
            settings.AttackSettings
            );

        plantPointDetect = new PlantPointDetect(
            this,
            settings.PlantPointDetectSettings
            );

        animationComponent = new SpookyAnimation(
            GetComponent<SpriteRenderer>(),
            GameObject.FindGameObjectWithTag("SpookyHandSprite").GetComponent<SpriteRenderer>(),
            GetComponent<Animator>()
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
        plantPointDetect.Start();
    }

    private void Update()
    {
        attackComponent.Update();
        enemyDetectComponent.Detect();
        plantPointDetect.Detect();
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
        public PlantPointDetect.Settings PlantPointDetectSettings;
        public EnemyDetect.Settings EnemyDetectSettings;
    }
}
