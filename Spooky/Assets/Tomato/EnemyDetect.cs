using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : CharacterComponent, IDetect
{
    // TODO encapsulate Spooky, Enemy and Plant in Character.
    protected SphereCollider detectionTrigger;
    [Range(3f, 5f)]
    public float enemyDetectionRange;

    [SerializeField]
    protected List<Enemy> nearEnemies = null;

    protected override void Awake()
    {
        base.Awake();

        detectionTrigger = transform.Find("EnemyDetector").GetComponent<SphereCollider>();

        nearEnemies = new List<Enemy>();
    }

    protected void Start()
    {
        detectionTrigger.radius = enemyDetectionRange;
    }

    public override void EveryFrame()
    {
        Detect();
    }

    public void Detect()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
        if (nearEnemies.Count > 0)
        {
            if (!IsFirstEnemyInTheListActive())
                ClearCurrentTarget();
        }
        else return;
    }

    protected Enemy GetFirstEnemyInTheList()
    {
        if (nearEnemies.Count > 0)
            return nearEnemies[0].GetComponent<Enemy>();
        else return null;
    }

    public Vector3 GetTheFirstEnemyDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (nearEnemies.Count > 0 && IsFirstEnemyInTheListActive())
        {
            _direction = (nearEnemies[0].transform.position - character.CharacterTransform.position).normalized;
            _direction.y = 0;
            return _direction;
        }
        else return _direction;
    }

    public bool IsFirstEnemyInTheListActive()
    {
        if (GetFirstEnemyInTheList() != null)
        {
            return GetFirstEnemyInTheList().gameObject.activeInHierarchy;
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
}
