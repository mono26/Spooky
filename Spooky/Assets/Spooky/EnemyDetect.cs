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
    private List<Enemy> nearEnemies = null;

    // Create a stack to store all the enemies that come in range
    public EnemyDetect(GameObject _owner, Settings _settings)
    {
        // Constructor, sets all needed dependencies.
        owner = _owner;
        settings = _settings;

        nearEnemies = new List<Enemy>();
    }

    public void Start()
    {

        //enemyList = new List<Enemy>();
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
        return nearEnemies[0].GetComponent<Enemy>();
    }

    public Vector3 GetEnemyDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (HasTarget() && IsTargetActive())
        {
            _direction = (nearEnemies[0].transform.position - owner.transform.position).normalized;
            _direction.y = 0;
            return _direction;
        }
        else return _direction;
    }

    public bool HasTarget()
    {
        if (nearEnemies.Count > 0)
        {
            return true;
        }
        else return false;
    }

    public bool IsTargetActive()
    {
        if (HasTarget())
        {
            return nearEnemies[0].gameObject.activeInHierarchy;
        }
        else return false;
    }

    private void ClearCurrentTarget()
    {
        // Clear the top of the stack if its destroyes, null, or inactive, out of range
        nearEnemies.RemoveAt(0);
    }

    private void AddEnemy(Enemy _enemy)
    {
        if (nearEnemies.Count == 0 || !nearEnemies.Contains(_enemy))
        {
            nearEnemies.Add(_enemy);
            return;
        }
        else return;
    }

    private void RemoveEnemy(Enemy _enemy)
    {
        if (nearEnemies.Count > 0 || nearEnemies.Contains(_enemy))
        {
            nearEnemies.Remove(_enemy);
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
            // TODO start coroutine display nearest enemy
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
            // TODO stop coroutine display nearest enemy if count = 0
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
