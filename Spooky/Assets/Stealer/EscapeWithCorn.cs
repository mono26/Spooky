using System.Collections;
using UnityEngine;

public class EscapeWithCorn : CharacterAction
{
    protected Enemy enemyCharacter;

    protected override void Awake()
    {
        base.Awake();

        enemyCharacter = GetComponent<Enemy>();
        target = LevelManager.Instance.GetRandomEscapePoint();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        target = LevelManager.Instance.GetRandomEscapePoint();
    }

    protected override IEnumerator Action()
    {
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
