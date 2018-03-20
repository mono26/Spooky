using UnityEngine;

[System.Serializable]
public class SpookyAttack
{
    // Variable for a bullet.
    private Spooky spooky;
    private Settings settings;

    [SerializeField]
    private Transform hand;
    private Bullet actualBullet;
    private Transform shootTransform;
    private ISpawnable<Bullet> bulletPrefab;
    private Coroutine chargingBullet;
    private Joystick joystick;
    private float lastShoot;

    public SpookyAttack(Spooky _spooky, Transform _hand, Transform _shootTransform, SpookyBullet _bulletPrefab, Joystick _joystick, Settings _settings)
    {
        spooky = _spooky;
        hand = _hand;
        shootTransform = _shootTransform;
        bulletPrefab = _bulletPrefab;
        joystick = _joystick;
        settings = _settings;
    }

    public void OnEnable()
    {
        spooky.OnFireButtonPress += ReadyToShoot;
        spooky.OnFireButtonRelease += Shoot;
    }

    public void OnDisable()
    {
        spooky.OnFireButtonPress -= ReadyToShoot;
        spooky.OnFireButtonRelease -= Shoot;
    }

    public void Update()
    {
        // TODO check if there is a target
        Vector3 _direction;
        float angle;
        float delta;
        if (spooky.enemyDetectComponent.HasTarget())
        {
            _direction = spooky.enemyDetectComponent.GetEnemyDirection();
            angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
            delta = angle - hand.localRotation.eulerAngles.z;
            /*if (Mathf.Abs(delta) < 180)
            {
                hand.Rotate(new Vector3(0, 0, delta), Space.Self);
            }
            else
                hand.Rotate(new Vector3(0, 0, -delta), Space.Self);*/
            hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.Euler(new Vector3(0, 0, angle)), Mathf.Abs(delta));
            return;
        }
        else
        {
            _direction.x = joystick.Horizontal;
            _direction.z = joystick.Vertical;
            if (!_direction.x.Equals(0f) || !_direction.z.Equals(0f))
            {
                angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
                delta = angle - hand.localRotation.eulerAngles.z;
                //TODO check if transform.Rotate
                /*if (Mathf.Abs(delta) < 180)
                {
                    hand.Rotate(new Vector3(0, 0, delta), Space.Self);
                }
                else
                    hand.Rotate(new Vector3(0, 0, -delta), Space.Self);*/
                hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.Euler(new Vector3(0, 0, angle)), Mathf.Abs(delta));
                return;
            }
            else return;
        }
    }

    private void RotateBullettowardsDirection(Transform _bullet)
    {
        _bullet.right = hand.right;
        return;
    }

    private void ReadyToShoot()
    {
        if (Time.timeSinceLevelLoad > lastShoot + settings.AttackRate)
        {
            actualBullet = GetBulletToShoot();
            actualBullet.transform.SetParent(shootTransform);
            // TODO start charging
        }
        else return;
    }

    private void Shoot()
    {
        if(actualBullet)
            LaunchBullet(actualBullet);
    }

    private void LaunchBullet(Bullet _bullet)
    {
        RotateBullettowardsDirection(_bullet.transform);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        _bullet.Launch(settings.LaunchForce);
        lastShoot = Time.timeSinceLevelLoad;
    }

    private Bullet GetBulletToShoot()
    {
        return bulletPrefab.Spawn(shootTransform);
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackRate;
        public float LaunchForce;
    }
}
