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

    public override void ExecuteAction()
    {
        if (lastExecute + cooldown < Time.timeSinceLevelLoad)
        {
            StartCoroutine(Escape());
            return;
        }
        else return;
    }

    private IEnumerator Escape()
    {
        SetLasActionExecuteToActualTimeInLevel();

        // Stop the action executiong because the animation has already end.
        if (enemyCharacter != null)
        {
            enemyCharacter.ExecuteAction(false);
            enemyCharacter.GetComponent<EnemyMovement>().StopEnemy(false);
            GetComponent<PoolableObject>().Release();
        }
        else
            character.gameObject.SetActive(false);

        yield break;
    }

}
