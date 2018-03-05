using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpookyAttack
{
    // Variable for a bullet.
    [SerializeField]
    private Transform hand;
    private Bullet bullet;
    private Joystick joystick;
    [SerializeField]
    private Settings settings;

    public SpookyAttack(Transform _hand, Bullet _bullet, Joystick _joystick, Settings _settings)
    {
        hand = _hand;
        bullet = _bullet;
        joystick = _joystick;
        settings = _settings;
    }

    public void Update()
    {
        // TODO check if there is a target
            // if there is a target rotate towards target.

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        if (!horizontal.Equals(0f) || !vertical.Equals(0f))
        {
            var time = Time.realtimeSinceStartup;
            var angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            var delta = hand.rotation.z - angle;
            Debug.Log(string.Format("time {0} angle {1}", time, angle));
            hand.localRotation = Quaternion.RotateTowards(hand.localRotation, Quaternion.Euler(new Vector3(0,0,angle)), Mathf.Abs(delta));
        }
        else return;
    }

    [System.Serializable]
    public class Settings
    {
        public float AttackRate;
    }
}
