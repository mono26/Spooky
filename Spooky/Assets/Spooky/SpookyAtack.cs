using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpookyAttack
{
    // Variable for a bullet.
    private Spooky spooky;
    SpookyEnemyDetect autoDetect;
    private Settings settings;

    [SerializeField]
    private Transform hand;
    private Transform shootPosition;
    private Bullet bullet;
    private Joystick joystick;

    public SpookyAttack(Spooky _spooky, SpookyEnemyDetect _autoDetect, Transform _hand, Transform _shootPosition, Bullet _bullet, Joystick _joystick, Settings _settings)
    {
        spooky = _spooky;
        autoDetect = _autoDetect;
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
        if (autoDetect.EnemyDirection(out _direction))
        {
            angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
            delta = angle - hand.rotation.z;
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
                var time = Time.realtimeSinceStartup;
                Debug.Log(string.Format("time {0} angle {1}", time, angle));
                delta = angle - hand.rotation.z;
                //TODO check if transform.Rotate
                hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.Euler(new Vector3(0, 0, angle)), Mathf.Abs(delta));
                return;
            }
            else return;
        }
    }

    private void LaunchBullet()
    {
        // TODO create the bullet and then tell the bullet to launch
        GameObject tempBullet = GameObject.Instantiate(bullet.gameObject, shootPosition.position, shootPosition.rotation);
        var bulletComponent = tempBullet.GetComponent<Bullet>();
        bulletComponent.Launch(settings.LaunchForce);
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackRate;
        public float LaunchForce;
    }
}
