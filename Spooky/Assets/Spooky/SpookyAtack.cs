using UnityEngine;

[System.Serializable]
public class SpookyAttack
{
    // Variable for a bullet.
    private Spooky spooky;
    private Settings settings;

    [SerializeField]
    private Transform hand;
    private Transform shootPosition;
    private ISpawnable<Bullet> bullet;
    private Joystick joystick;
    private float lastShoot;

    public SpookyAttack(Spooky _spooky, Transform _hand, Transform _shootPosition, SpookyBullet _bullet, Joystick _joystick, Settings _settings)
    {
        spooky = _spooky;
        hand = _hand;
        shootPosition = _shootPosition;
        bullet = _bullet;
        joystick = _joystick;
        settings = _settings;
    }

    public void OnEnable()
    {
        spooky.OnFireButtonPressed += LaunchBullet;
    }

    public void OnDisable()
    {
        spooky.OnFireButtonPressed -= LaunchBullet;
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

    protected void RotateBullettowardsDirection(Transform _bullet)
    {
        _bullet.right = hand.right;
        return;
    }

    private void LaunchBullet()
    {
        // TODO create the bullet and then tell the bullet to launch
        if (Time.timeSinceLevelLoad > lastShoot + settings.AttackRate)
        {
            Bullet tempBullet = bullet.Spawn(shootPosition);
            RotateBullettowardsDirection(tempBullet.transform);
            tempBullet.transform.SetParent(GameObject.Find("Bullets").transform);
            tempBullet.Launch(settings.LaunchForce);
            lastShoot = Time.timeSinceLevelLoad;
        }
        else return;
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackRate;
        public float LaunchForce;
    }
}
