using System.Collections;
using UnityEngine;

public class EscapeWithCorn : CharacterAction
{
    protected Enemy enemyCharacter;

    protected override void Awake()
    {
        base.Awake();

        enemyCharacter = GetComponent<Enemy>();

        return;
    }

    protected virtual void Start()
    {
        target = LevelManager.Instance.GetRandomEscapePoint();

        return;
    }

    protected override IEnumerator Action()
    {
        EventManager.TriggerEvent(new EnemyEvent(EnemyEventType.ExecuteAction, enemyCharacter));
        yield return 0;

        SetLasActionExecuteToActualTimeInLevel();

        EventManager.TriggerEvent(new EnemyEvent(EnemyEventType.FinishExecute, enemyCharacter));

        // Stop the action executiong because the animation has already end.
        if (enemyCharacter != null)
        {
            GetComponent<PoolableObject>().Release();
        }
        else
            character.gameObject.SetActive(false);

        yield break;
    }

}
