using System.Collections;
using UnityEngine;

[System.Serializable]
public class SpookyAttack
{
    // Variable for a bullet.
    private Spooky spooky;
    [SerializeField]
    private Transform hand;
    [SerializeField]
    private Transform shootTransform;
    private Settings settings;

    [SerializeField]
    private ISpawnable<Bullet> bulletPrefab;
    private SpookyBullet actualBullet;
    private bool isCharging;
    private Coroutine chargeBullet;
    // Number of charges persecond

    private float lastShoot;

    public SpookyAttack(Spooky _spooky, Transform _hand, Transform _shootTransform, SpookyBullet _bulletPrefab, Settings _settings)
    {
        spooky = _spooky;
        hand = _hand;
        shootTransform = _shootTransform;
        bulletPrefab = _bulletPrefab;
        settings = _settings;
    }

    public void OnEnable()
    {
        spooky.OnFireButtonPress += ReadyToAttack;
        spooky.OnFireButtonRelease += Attack;
    }

    public void OnDisable()
    {
        spooky.OnFireButtonPress -= ReadyToAttack;
        spooky.OnFireButtonRelease -= Attack;
    }

    public void Update()
    {
        if (actualBullet)
            RotateBullettowardsDirection(actualBullet.transform);

        Vector3 _direction = Vector3.zero;
        if (spooky.EnemyDetectComponent.CurrentEnemyTarget != null)
        {
            _direction = spooky.EnemyDetectComponent.GetCurrentEnemyTargetDirection();
            RotateHand(_direction);
            return;
        }

        else if (spooky.EnemyDetectComponent.CurrentEnemyTarget == null)
        {
            _direction.x = spooky.Joystick.Horizontal;
            _direction.z = spooky.Joystick.Vertical;
            if (!_direction.x.Equals(0f) || !_direction.z.Equals(0f))
            {
                RotateHand(_direction);
                return;
            }
            else return;
        }
    }

    private void RotateHand(Vector3 _direction)
    {
        float angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
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
        _bullet.right = hand.right;
        return;
    }

    private void ReadyToAttack()
    {
        actualBullet = GetBulletToShoot();
        actualBullet.transform.SetParent(shootTransform);
        chargeBullet = spooky.StartCoroutine(ChargeAttack());

        /* if (Time.timeSinceLevelLoad > lastShoot + settings.AttackRate)
        {
            
        }
        else return; */
    }

    private void Attack()
    {
        if (actualBullet)
        {
            isCharging = false;
            LaunchAttack(actualBullet);
            return;
        }
        else return;
    }

    private void LaunchAttack(Bullet _bullet)
    {

        spooky.StopCoroutine(chargeBullet);
        RotateBullettowardsDirection(_bullet.transform);
        actualBullet = null;
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        _bullet.Launch(settings.LaunchForce);
        lastShoot = Time.timeSinceLevelLoad;
    }

    private SpookyBullet GetBulletToShoot()
    {
        return (SpookyBullet)bulletPrefab.Spawn(shootTransform);
    }

    private IEnumerator ChargeAttack()
    {
        isCharging = true;

        while(isCharging == true)
        {
            actualBullet.IncreaseSize(actualBullet);
            actualBullet.IncreaseDamage(actualBullet);
            yield return new WaitForSeconds(1 / settings.ChargeRate);    // One charge per second
        }

        yield return 0;
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackRate;
        public float LaunchForce;
        public float ChargeRate = 2f;
    }
}
