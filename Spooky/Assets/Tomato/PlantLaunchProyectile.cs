using System.Collections;
using UnityEngine;


[RequireComponent(typeof(EnemyDetect),typeof(SingleObjectPool))]
public class PlantLaunchProyectile : CharacterAction
{
    protected Plant plantCharacter;

    [SerializeField]
    private float launchForce = 10f;
    [SerializeField]
    private AudioClip soundEffect;

    private Bullet actualBullet;
    private SingleObjectPool bulletPool;
    private Transform shootPoint;

    protected override void Awake()
    {
        base.Awake();

        plantCharacter = GetComponent<Plant>();
        shootPoint = transform.Find("ShootPoint").GetComponent<Transform>();
        bulletPool = GetComponent<SingleObjectPool>();
    }

    protected void Start()
    {
        cooldown = plantCharacter.StatsComponent.AttackSpeed;
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

    public override void ExecuteAction()
    {
        if (lastExecute + cooldown < Time.timeSinceLevelLoad)
        {
            StartCoroutine(BasicAtack());
            return;
        }
        else return;
    }

    protected IEnumerator BasicAtack()
    {
        if (plantCharacter != null)
        {
            plantCharacter.ExecuteAction(true);
            yield return 0;
        }

        yield return new WaitForSecondsRealtime(
                    character.CharacterAnimator.GetCurrentAnimatorStateInfo(0).length + delayAfterAnimationIsFinished
                    );

        SetLasActionExecuteToActualTimeInLevel();

        actualBullet = bulletPool.GetObjectFromPool().GetComponent<Bullet>();
        actualBullet.gameObject.SetActive(true);
        RotateActualBulleTowardsDirection(plantCharacter.EnemyDetect.GetFirstEnemyTargetDirection());
        actualBullet.Launch(launchForce);
        actualBullet = null;

        if (plantCharacter != null)
        {
            plantCharacter.ExecuteAction(false);
            yield return 0;
        }

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
