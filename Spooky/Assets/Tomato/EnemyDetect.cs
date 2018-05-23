using System.Collections.Generic;
using UnityEngine;

public enum DetectEventType { TargetAcquired, TargetLost}

public class DetectEvent : SpookyCrowEvent
{
    public DetectEventType type;
    public Transform target;
    public Character character;

    public DetectEvent(DetectEventType _type, Character _character, Transform _target = null)
    {
        type = _type;
        character = _character;
        target = _target;
    }
}

public class EnemyDetect : CharacterComponent
{
    // TODO encapsulate Spooky, Enemy and Plant in Character.
    protected SphereCollider detectionTrigger;

    [SerializeField][Range(3f, 5f)]
    protected float detectionRange;
    public float DetectionRange { get { return detectionRange; } }

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
        detectionTrigger.radius = detectionRange;
    }

    public override void EveryFrame()
    {
        Detect();
    }

    protected virtual void Detect()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
        if (nearEnemies.Count > 0)
        {
            if (!IsFirstEnemyInTheListActive())
                ClearCurrentTarget();
        }
        return;
    }

    public Enemy GetFirstEnemyInTheList()
    {
        if (nearEnemies.Count > 0)
            return nearEnemies[0].GetComponent<Enemy>();
        else return null;
    }

    public bool IsFirstEnemyInTheListActive()
    {
        if (GetFirstEnemyInTheList() != null)
        {
            return GetFirstEnemyInTheList().gameObject.activeInHierarchy;
        }
        else return false;
    }

    public Vector3 GetFirstEnemyTargetDirection()
    {
        Vector3 _direction = Vector3.zero;
        if (nearEnemies.Count > 0)
        {
            _direction = (nearEnemies[0].transform.position - character.CharacterTransform.position).normalized;
            _direction.y = _direction.z;
            _direction.z = 0;
            return _direction;
        }
        else return _direction;
    }

    private void ClearCurrentTarget()
    {
        // Clear the top of the stack if its destroyes, null, or inactive, out of range
        nearEnemies.RemoveAt(0);
        return;
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

    public virtual void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            if (nearEnemies.Count == 0)
            {
                EventManager.TriggerEvent<DetectEvent>(new DetectEvent(DetectEventType.TargetAcquired, character, _collider.transform));
            }
            AddEnemyToTheList(_collider.GetComponent<Enemy>());
            return;
        }
        else return;
    }

    public virtual void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            RemoveEnemyFromTheList(_collider.GetComponent<Enemy>());
            if(nearEnemies.Count == 0)
                EventManager.TriggerEvent<DetectEvent>(new DetectEvent(DetectEventType.TargetLost, character));
            return;
        }
        else return;
    }
}
