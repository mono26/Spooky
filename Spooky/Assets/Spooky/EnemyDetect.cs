using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDetect : IDetect
{
    // Give the value of the range in settings to the radius of the collider
    // it must be in a separate layer to wrk properly.
    private GameObject owner;
    private Settings settings;

    [SerializeField]
    private List<Enemy> enemyList;

    // Create a stack to store all the enemies that come in range
    public EnemyDetect(GameObject _owner, Settings _settings)
    {
        // Constructor, sets all needed dependencies.
        owner = _owner;
        settings = _settings;

        enemyList = new List<Enemy>();
    }

    public void Start()
    {
        settings.detectionTrigger.radius = settings.enemyDetectionRange;
    }

    public void Detect()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
        if (HasTarget())
        {
            if (!IsTargetActive())
                ClearCurrentTarget();
        }
        else return;
    }

    public Enemy GetCurrentTarget()
    {
        return enemyList[0].GetComponent<Enemy>();
    }

    public Vector3 GetEnemyDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (HasTarget() && IsTargetActive())
        {
            _direction = (enemyList[0].transform.position - owner.transform.position).normalized;
            _direction.y = 0;
            return _direction;
        }
        else return _direction;
    }

    public bool HasTarget()
    {
        if (enemyList.Count > 0)
        {
            return true;
        }
        else return false;
    }

    public bool IsTargetActive()
    {
        if (HasTarget())
        {
            return enemyList[0].gameObject.activeInHierarchy;
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
        if (enemyList.Count == 0 || !enemyList.Contains(_enemy))
        {
            enemyList.Add(_enemy);
            return;
        }
        else return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        if (enemyList.Count > 0 || enemyList.Contains(_enemy))
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
        public float enemyDetectionRange;
        public SphereCollider detectionTrigger;
    }
}
