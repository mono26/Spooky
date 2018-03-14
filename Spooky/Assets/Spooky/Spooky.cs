using UnityEngine;

public class Spooky : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public Joystick Joystick;
    public SphereCollider EnemyDetectTrigger;
    public SphereCollider PlantPointDetectTrigger;
    [Range(3f,4f)]
    public float EnemyDetectionRange;
    [Range(1.5f, 2f)]
    public float PlantPointDetectionRange;
    public Transform Hand;
    public SpookyBullet Bullet;
    public Transform ShootPosition;
    public SpriteRenderer SpookySprite;
    public SpriteRenderer HandSprite;
    public Animator Animator;

    // This is going to be the class the connects all the components of spooky
    // Execute update, triggerevetns, awake, start, etc.
    private SpookyMovement movementComponent;
    private SpookyAttack attackComponent;
    public EnemyDetect enemyDetectComponent;
    private PlantPointDetect plantPointDetect;
    public SpookyAnimation animationComponent;
    public Settings settings;

    public delegate void FireButtonPress();
    public event FireButtonPress OnFireButtonPressed;
    public delegate void InFightWithEnemy();
    public event InFightWithEnemy OnFightWithEnemy;

    private void Awake()
    {
        movementComponent = new SpookyMovement(
            this,
            settings.MovementSettings
            );

        enemyDetectComponent = new EnemyDetect(
            gameObject,
            EnemyDetectTrigger,
            EnemyDetectionRange
            );

        attackComponent = new SpookyAttack(
            this,
            Hand,
            ShootPosition,
            Bullet,
            Joystick,
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

    public void FireButton()
    {
        if (OnFireButtonPressed != null)
            OnFireButtonPressed();
    }

    [System.Serializable]
    public class Settings
    {
        public SpookyMovement.Settings MovementSettings;
        public SpookyAttack.Settings AttackSettings;
    }
}
