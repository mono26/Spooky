using UnityEngine;

[RequireComponent(typeof(SpookyAnimation), typeof(SpookyAttack), typeof(SpookyEnemyDetect))]
[RequireComponent(typeof(SpookyMovement), typeof(SpookyPlantPointDetection), typeof(Rigidbody))]
public class Spooky : MonoBehaviour
{
    // Components
    private Rigidbody spookyBody;
    public Rigidbody SpookyBody { get { return spookyBody; } }
    private Transform spookyTransform;
    public Transform SpookyTransform { get { return spookyTransform; } }

    // Movement section
    [Header("Movement Settings")]
    private SpookyMovement movementComponent;
    public SpookyMovement MovementComponent { get { return movementComponent; } }
    // Detection Section
    [Header("Detection Settings")]
    [SerializeField]
    private SpookyEnemyDetect enemyDetectComponent;
    public SpookyEnemyDetect EnemyDetectComponent { get { return enemyDetectComponent; } }
    [SerializeField]
    private SpookyPlantPointDetection plantPointDetectComponent;
    public SpookyPlantPointDetection PlantPointDetectComponent { get { return plantPointDetectComponent; } }
    // Animation section
    [Header("Animation Settings")]
    private SpookyAnimation animationComponent;
    public SpookyAnimation AnimationComponent { get { return animationComponent; } }
    // Attack section
    [Header("Attack Settings")]
    [SerializeField]
    private SpookyAttack attackComponent;
    public SpookyAttack AttackComponent { get { return attackComponent; } }

    public delegate void InFightWithEnemy();
    public event InFightWithEnemy OnFightWithEnemy;

    private void Awake()
    {
        spookyBody = GetComponent<Rigidbody>();
        spookyTransform = GetComponent<Transform>();

        movementComponent = GetComponent<SpookyMovement>();
        enemyDetectComponent = GetComponent<SpookyEnemyDetect>();
        attackComponent = GetComponent<SpookyAttack>();
        plantPointDetectComponent = GetComponent<SpookyPlantPointDetection>();
        animationComponent = GetComponent<SpookyAnimation>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        movementComponent.HandleInput();
        attackComponent.HandleInput();
        attackComponent.EveryFrameProcess();
    }

    private void FixedUpdate()
    {
        movementComponent.FixedFrameProcess();
        /*else
        {
            spooky.AnimationComponent.IsMoving(new Vector3(0, 0, 0));
            return;
        }*/
    }

    public void OnTriggerEnter(Collider _collider)
    {

    }

    public void OnTriggerExit(Collider _collider)
    {

    }

}
