﻿using System.Collections;
using UnityEngine;

public class PlantLaunchProjectile : CharacterAction
{
    [SerializeField]
    private float launchForce = 10f;
    [SerializeField]
    private PoolableObject bulletPrefab;

    private Bullet actualBullet;
    [SerializeField]
    private Transform shootPoint;

    protected override void Awake()
    {
        base.Awake();

        if(shootPoint == null)
            shootPoint = transform.Find("ShootPoint").GetComponent<Transform>();
    }

    public override void EveryFrame()
    {
        if(target != null)
        {
            if (target.position.x > character.transform.position.x)
            {
                if (!character.IsFacingRightDirection)
                    character.Flip();
            }
            // If it's negative, then we're facing left
            else if (target.position.x < character.transform.position.x)
            {
                if (character.IsFacingRightDirection)
                    character.Flip();
            }
        }

        base.EveryFrame();
    }

    protected override IEnumerator Action()
    {
        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        actualBullet = PoolsManager.GetObjectFromPools(bulletPrefab).GetComponent<Bullet>();
        actualBullet.gameObject.SetActive(true);

        if (target != null)
            RotateActualBulleTowardsDirection(GetTargetDirection());
        else
            RotateActualBulleTowardsDirection(GetTargetDirection(lastTargetPosition));

        actualBullet.Launch(launchForce);
        actualBullet = null;

        PlaySfx();

        yield break;
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

    /*protected override void UpdateState()
    {
        if (character.characterStateMachine != null)
        {
            if (character.IsExecutingAction == true)
                character.characterStateMachine.ChangeState(Character.CharacterState.ExecutingAction);
            else
                character.characterStateMachine.ChangeState(Character.CharacterState.Idle);
        }
        return;
    }*/
}
