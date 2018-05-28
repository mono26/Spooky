﻿using System.Collections;
using UnityEngine;


[RequireComponent(typeof(EnemyDetect),typeof(SingleObjectPool))]
public class PlantLaunchProjectile : CharacterAction
{
    [SerializeField]
    protected Plant plantCharacter;

    [SerializeField]
    private float launchForce = 10f;

    private Bullet actualBullet;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private SingleObjectPool bulletPool;

    protected override void Awake()
    {
        base.Awake();

        if(plantCharacter == null)
            plantCharacter = GetComponent<Plant>();
        if(shootPoint == null)
            shootPoint = transform.Find("ShootPoint").GetComponent<Transform>();

        if (bulletPool == null)
            Debug.Log(this.gameObject + "You need to asign a pool to the ability");
    }

    protected void Start()
    {
        if(cooldown == 0)
            cooldown = 1 / plantCharacter.StatsComponent.AttacksPerSecond;
        if (range == 0)
            range = plantCharacter.EnemyDetect.DetectionRange;
        return;
    }

    public override void EveryFrame()
    {
        if(target != null)
        {
            if (target.position.x > character.CharacterTransform.position.x)
            {
                if (!character.IsFacingRightDirection)
                    character.Flip();
            }
            // If it's negative, then we're facing left
            else if (target.position.x < character.CharacterTransform.position.x)
            {
                if (character.IsFacingRightDirection)
                    character.Flip();
            }
        }

        base.EveryFrame();
    }

    protected override IEnumerator Action()
    {
        EventManager.TriggerEvent(new PlantEvent(PlantEventType.ExecuteAction, plantCharacter));


        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        SetLasActionExecuteToActualTimeInLevel();

        actualBullet = bulletPool.GetObjectFromPool().GetComponent<Bullet>();
        actualBullet.gameObject.SetActive(true);
        RotateActualBulleTowardsDirection(plantCharacter.EnemyDetect.GetFirstEnemyTargetDirection());
        actualBullet.Launch(launchForce);
        actualBullet = null;

        PlaySfx();

        EventManager.TriggerEvent(new PlantEvent(PlantEventType.FinishExecute, plantCharacter));

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

    protected override void UpdateState()
    {
        if (plantCharacter.IsExecutingAction == true)
        {
            character.characterStateMachine.ChangeState(Character.CharacterState.ExecutingAction);
            return;
        }
        else
        {
            character.characterStateMachine.ChangeState(Character.CharacterState.Idle);
            return;
        }
    }
}