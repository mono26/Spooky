using System.Collections;
using UnityEngine;

public class SpookyEnemyDetect : EnemyDetect
{
    [SerializeField]
    private Enemy currentEnemyTarget;
    public Enemy CurrentEnemyTarget { get { return currentEnemyTarget; } }

    private Coroutine targetClosestEnemy = null;

    private IEnumerator TargetNearestEnemy()
    {
        if (nearEnemies.Count > 0)
        {
            float distanceToTheFirstEnemy = Vector3.SqrMagnitude(character.CharacterTransform.position - nearEnemies[0].transform.position);
            Enemy temporalCurrentEnemy = nearEnemies[0];
            for (int plantPoint = 0; plantPoint < nearEnemies.Count; plantPoint++)
            {
                float distanceToTheNextEnemy = Vector3.SqrMagnitude(character.CharacterTransform.position - nearEnemies[plantPoint].transform.position);
                if (distanceToTheNextEnemy < distanceToTheFirstEnemy)
                {
                    distanceToTheFirstEnemy = distanceToTheNextEnemy;
                    temporalCurrentEnemy = nearEnemies[plantPoint];
                }
            }
            if (!currentEnemyTarget.Equals(temporalCurrentEnemy))
            {
                ChangeCurrentEnemyTarget(temporalCurrentEnemy);
            }
        }
        yield return new WaitForSeconds(Time.deltaTime);
        targetClosestEnemy = StartCoroutine(TargetNearestEnemy());
    }

    private void ChangeCurrentEnemyTarget(Enemy _enemy)
    {
        currentEnemyTarget = _enemy;
    }

    public Vector3 GetCurrentEnemyTargetDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (currentEnemyTarget)
        {
            _direction = (currentEnemyTarget.transform.position - character.CharacterTransform.position).normalized;
            _direction.y = 0;
            return _direction;
        }
        else return _direction;
    }

    public new void OnTriggerEnter(Collider _enemyCollider)
    {
        base.OnTriggerEnter(_enemyCollider);
        // Check if the collider is tagged as enemy
        if (_enemyCollider.CompareTag("Enemy"))
        {
            if (GetFirstEnemyInTheList() != null)
            {
                ChangeCurrentEnemyTarget(_enemyCollider.GetComponent<Enemy>());
                targetClosestEnemy = StartCoroutine(TargetNearestEnemy()); 
            }
            return;
        }
        else return;
    }

    public new void OnTriggerExit(Collider _enemyCollider)
    {
        base.OnTriggerExit(_enemyCollider);
        // Check if the collider is tagged as enemy
        if (_enemyCollider.CompareTag("Enemy"))
        {
            if(nearEnemies.Count.Equals(0))
            {
                StopCoroutine(targetClosestEnemy);
            }
            // TODO stop coroutine display nearest enemy if count = 0
            return;
        }
        else return;
    }

}
