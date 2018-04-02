using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDetect : IDetect
{
    // Give the value of the range in settings to the radius of the collider
    // it must be in a separate layer to wrk properly.
    protected MonoBehaviour owner;
    private Settings settings;

    private Coroutine detection = null;
    [SerializeField]
    protected List<Enemy> nearEnemies = null;

    // Create a stack to store all the enemies that come in range
    public EnemyDetect(MonoBehaviour _owner, Settings _settings)
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
        if (nearEnemies.Count > 0)
        {
            if (!HasAValidTarget())
                ClearCurrentTarget();
        }
        else return;
    }

    public Enemy GetFirstEnemyInTheList()
    {
        if (nearEnemies.Count > 0)
            return nearEnemies[0].GetComponent<Enemy>();
        else return null;
    }

    public Vector3 GetEnemyDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (nearEnemies.Count > 0 && HasAValidTarget())
        {
            _direction = (nearEnemies[0].transform.position - owner.transform.position).normalized;
            _direction.y = 0;
            return _direction;
        }
        else return _direction;
    }

    public bool HasAValidTarget()
    {
        if (nearEnemies.Count > 0)
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

    private void AddEnemyToTheList(Enemy _enemy)
    {
        if (nearEnemies.Count == 0 || !nearEnemies.Contains(_enemy))
        {
            nearEnemies.Add(_enemy);
            return;
        }
        else return;
    }

    private void RemoveEnemyFromTheList(Enemy _enemy)
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
        if (_collider.CompareTag("Enemy"))
        {
            AddEnemyToTheList(_collider.GetComponent<Enemy>());
            return;
        }
        else return;
    }

    public void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            RemoveEnemyFromTheList(_collider.GetComponent<Enemy>());
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
