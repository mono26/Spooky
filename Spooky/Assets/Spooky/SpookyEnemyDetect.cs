using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpookyEnemyDetect
{
    // Give the value of the range in settings to the radius of the collider
    // it must be in a separate layer to wrk properly.
    private Spooky spooky;
    private SphereCollider detectionTrigger;
    private Settings settings;

    [SerializeField]
    private List<Enemy> enemyList;

    // Create a stack to store all the enemies that come in range
    public SpookyEnemyDetect(Spooky _spooky, SphereCollider _detectionTrigger, Settings _settings)
    {
        // Constructor, sets all needed dependencies.
        spooky = _spooky;
        detectionTrigger = _detectionTrigger;
        settings = _settings;

        enemyList = new List<Enemy>();
        detectionTrigger.radius = settings.EnemyDetectionRange;
    }

    public void Update()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
        if (!HasTargetAndIsActive())
        {
           ClearCurrentTarget();
        }
    }

    public bool EnemyDirection(out Vector3 _direction)
    {
        _direction = Vector3.zero;
        if (HasTargetAndIsActive())
        {
            _direction = (enemyList[0].settings.Rigidbody.position - spooky.settings.Rigidbody.position).normalized;
            _direction.y = 0;
            return true;
        }
        else return false;
    }

    private bool HasTargetAndIsActive()
    {
        if (enemyList.Count > 0 && enemyList[0].gameObject.activeInHierarchy)
        {
            return true;
        }
        else return false;
    }

    private void ClearCurrentTarget()
    {
        // Clear the top of the stack if its destroyes, null, or inactive, out of range
        enemyList.RemoveAt(0);
    }

    private void AddEnemy(Enemy _enemy)
    {
        if (!enemyList.Contains(_enemy))
        {
            enemyList.Add(_enemy);
            return;
        }
        else return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        if (enemyList.Contains(_enemy))
        {
            enemyList.Remove(_enemy);
            return;
        }
        else return;
    }

    public void OnTriggerEnter(Collider _collider)
    {
        // Check if the collider is tagged as enemy
        if (_collider.CompareTag("Enemy"))
        {
            AddEnemy(_collider.GetComponent<Enemy>());
            return;
        }
        else return;
    }

    public void OnTriggerExit(Collider _collider)
    {
        // Check if the collider is tagged as enemy
        if (_collider.CompareTag("Enemy"))
        {
            RemoveEnemy(_collider.GetComponent<Enemy>());
            return;
        }
        else return;
    }

    [System.Serializable]
    public class Settings
    {
        [Range(3f, 5f)]
        public float EnemyDetectionRange;
    }
}
