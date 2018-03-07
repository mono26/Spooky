using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDetect : IEnemyDetect
{
    // Give the value of the range in settings to the radius of the collider
    // it must be in a separate layer to wrk properly.
    private GameObject owner;

    [Range(3f, 5f)]
    public float enemyDetectionRange;
    private SphereCollider detectionTrigger;

    [SerializeField]
    private List<Enemy> enemyList;

    // Create a stack to store all the enemies that come in range
    public EnemyDetect(GameObject _owner, SphereCollider _detectionTrigger, float _enemyDetectionRange)
    {
        // Constructor, sets all needed dependencies.
        owner = _owner;
        detectionTrigger = _detectionTrigger;
        enemyDetectionRange = _enemyDetectionRange;

        enemyList = new List<Enemy>();
        detectionTrigger.radius = enemyDetectionRange;
    }

    public void Detect()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
        if (HasTargetAndIsActive())
        {
           ClearCurrentTarget();
        }
    }

    public bool HasEnemyDirection(out Vector3 _direction)
    {
        _direction = Vector3.zero;
        if (HasTargetAndIsActive())
        {
            _direction = (enemyList[0].settings.Rigidbody.position - owner.transform.position).normalized;
            _direction.y = 0;
            return true;
        }
        else return false;
    }

    public bool HasTargetAndIsActive()
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
            var time = Time.realtimeSinceStartup;
            Debug.Log(string.Format("time {0} enemy entered trigger", time));
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
}
