using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpookyAttack
{
    // Variable for a bullet.
    private Spooky spooky;
    [SerializeField]
    private Settings settings;

    [SerializeField]
    private Transform hand;
    private Transform shootPosition;
    private Bullet bullet;
    private Joystick joystick;

    public SpookyAttack(Spooky _spooky, Transform _hand, Transform _shootPosition, Bullet _bullet, Joystick _joystick, Settings _settings)
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
            // if there is a target rotate towards target.

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        if (!horizontal.Equals(0f) || !vertical.Equals(0f))
        {
            var angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            var delta = hand.rotation.z - angle;
            //TODO check if transform.Rotate
            hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.Euler(new Vector3(0,0,angle)), Mathf.Abs(delta));
        }
        else return;
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
