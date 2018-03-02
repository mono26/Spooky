using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyEnemyDetect
{
    // Give the value of the range in settings to the radius of the collider
    // it must be in a separate layer to wrk properly.
    private Collider detectionTrigger;
    private Settings settings;

    private List<Enemy> enemyList;

    // Create a stack to store all the enemies that come in range
    public SpookyEnemyDetect(Collider _detectionTrigger, Settings _settings)
    {
        // Constructor, sets all needed dependencies.
        detectionTrigger = _detectionTrigger;
        settings = _settings;
    }

    public void Update()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
    }
    private void ClearCurrentTarget()
    {
        // Clear the top of the stack if its destroyes, null, or inactive, out of range
    }

    private void AddEnemy(Enemy _enemy)
    {
        // TODO add enemy to the list
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        // TODO remove enemy from the list
    }

    public void OnTriggerEnter(Collider _collider)
    {
        // Check if the collider is tagged as enemy
        if (_collider.CompareTag("Enemy"))
        {
            // TODO add the enemy component to de enemyList
            return;
        }
        else
            return;
    }

    public void OnTriggerExit(Collider _collider)
    {
        // Check if the collider is tagged as enemy
        if (_collider.CompareTag("Enemy"))
        {
            // TODO remove the enemy component from de enemyList
            return;
        }
        else
            return;
    }

    [System.Serializable]
    public class Settings
    {
        public float EnemyDetectionRange;
    }
}
