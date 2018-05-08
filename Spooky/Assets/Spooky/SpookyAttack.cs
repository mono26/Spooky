using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SingleObjectPool), typeof(SpookyEnemyDetect))]
public class SpookyAttack : CharacterComponent
{
    // Variable for a bullet.
    private SpookyEnemyDetect enemyDetector;
    private Transform hand;
    private Transform shootPoint;

    public float attackRate = 1f;
    public float launchForce = 10f;
    public float chargeRate = 2f;

    private Vector3 aimDirection;
    private SingleObjectPool bulletPool;
    [SerializeField]
    private SpookyBullet actualBullet;

    private float lastShoot;

    protected override void Awake()
    {
        base.Awake();

        enemyDetector = GetComponent<SpookyEnemyDetect>();
        hand = transform.Find("Hand").GetComponent<Transform>();
        shootPoint = hand.Find("ShootPoint").GetComponent<Transform>();
        bulletPool = GetComponent<SingleObjectPool>();
    }

    public override void EveryFrame()
    {
        base.EveryFrame();

        if (actualBullet != null)
            RotateBullettowardsDirection(actualBullet.transform);

        if (enemyDetector.CurrentEnemyTarget != null)
        {
            aimDirection = enemyDetector.GetCurrentEnemyTargetDirection();
            RotateHand(aimDirection);
            return;
        }

        else if (enemyDetector.CurrentEnemyTarget == null)
        {
            aimDirection.x = character.characterInput.Movement.x;
            aimDirection.y = character.characterInput.Movement.y;
            if (!aimDirection.x.Equals(0f) || !aimDirection.y.Equals(0f))
            {
                RotateHand(aimDirection);
                return;
            }
            else return;
        }
    }

    private void RotateHand(Vector3 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        float delta = angle - hand.localRotation.eulerAngles.z;
        hand.localRotation = Quaternion.RotateTowards(
            hand.localRotation,
            Quaternion.Euler(new Vector3(0, 0, angle)),
            Mathf.Abs(delta)
            );
        return;
    }

    private void RotateBullettowardsDirection(Transform _bullet)
    {
        _bullet.right = shootPoint.right;
        _bullet.position = shootPoint.position;
        return;
    }

    private void CreateCurrentBullet()
    {
        Debug.Log("Creating current bullet");
        actualBullet = bulletPool.GetObjectFromPool().GetComponent<SpookyBullet>();
        actualBullet.transform.position = shootPoint.position;
        actualBullet.transform.SetParent(shootPoint);
        actualBullet.gameObject.SetActive(true);
        return;
    }

    private void Attack()
    {
        if (actualBullet)
        {
            LaunchAttack();
            return;
        }
        else return;
    }

    private void LaunchAttack()
    {
        if (actualBullet != null)
        {
            RotateBullettowardsDirection(actualBullet.transform);
            actualBullet.Launch(launchForce);
            actualBullet.GetComponent<PoolableObject>().InvokeRelease();
            actualBullet = null;
            lastShoot = Time.timeSinceLevelLoad;
            return;
        }
        else return;
    }

    private void ChargeAttack()
    {
        if (actualBullet != null)
        {
            actualBullet.IncreaseSize(actualBullet);
            actualBullet.IncreaseDamage(actualBullet);
            return;
        }
        else return;
    }

    protected override void HandleInput()
    {
        if (InputManager.Instance.FireButton.CurrentState == CustomButton.ButtonStates.Pressed)
        {
            CreateCurrentBullet();
        }

        if (InputManager.Instance.FireButton.CurrentState == CustomButton.ButtonStates.Down)
        {
            ChargeAttack();
        }

        if (InputManager.Instance.FireButton.CurrentState == CustomButton.ButtonStates.Up)
        {
            LaunchAttack();
        }
    }
}
