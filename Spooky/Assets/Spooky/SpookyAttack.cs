﻿using System.Collections;
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

        if (enemyDetector.CurrentEnemyTarget != null)
        {
            aimDirection = enemyDetector.GetCurrentEnemyTargetDirection();
            aimDirection.y = aimDirection.z;
            aimDirection.z = 0;
            RotateHand(aimDirection);
        }

        else if (enemyDetector.CurrentEnemyTarget == null)
        {
            aimDirection.x = character.CharacterInput.Movement.x;
            aimDirection.y = character.CharacterInput.Movement.y;
            if (!aimDirection.x.Equals(0f) || !aimDirection.y.Equals(0f))
            {
                RotateHand(aimDirection);
            }
        }

        if (actualBullet != null)
        {
            RotateActualBulleTowardsDirection(aimDirection);
            return;
        }
        else return;
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

    private void RotateActualBulleTowardsDirection(Vector3 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        float delta = angle - actualBullet.transform.eulerAngles.z;
        actualBullet.transform.rotation = Quaternion.RotateTowards(
            actualBullet.transform.rotation,
            Quaternion.Euler(new Vector3(90, 0, angle)),
            Mathf.Abs(delta)
            );
        actualBullet.transform.position = shootPoint.position;
        return;
    }

    private void CreateCurrentBullet()
    {
        actualBullet = bulletPool.GetObjectFromPool().GetComponent<SpookyBullet>();
        actualBullet.transform.position = shootPoint.position;
        actualBullet.gameObject.SetActive(true);
        return;
    }

    private void LaunchAttack()
    {
        if (actualBullet != null)
        {
            RotateActualBulleTowardsDirection(aimDirection);
            actualBullet.Launch(launchForce);
            actualBullet.GetComponent<PoolableObject>().OnSpawnCompleted();
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
            actualBullet.IncreaseSize();
            actualBullet.IncreaseDamage();
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
