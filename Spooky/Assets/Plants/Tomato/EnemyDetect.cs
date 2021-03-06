﻿using System.Collections.Generic;
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
    [Header("Settings")]
    [SerializeField]
    protected SphereCollider detectionTrigger;
    [SerializeField]
    [Range(0.7f, 5f)]
    protected float detectionRange;
    public float DetectionRange { get { return detectionRange; } }

    [Header("Editor debugging.")]
    [SerializeField]
    protected List<Character> nearEnemies = null;

    protected override void Awake()
    {
        base.Awake();

        if (detectionTrigger == null)
            detectionTrigger = transform.Find("EnemyDetector").GetComponent<SphereCollider>();

        nearEnemies = new List<Character>();
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
            SearchForInactiveEnemiesAndClearIt();
        }
        return;
    }

    private void SearchForInactiveEnemiesAndClearIt()
    {
        for (int i = 1; i < nearEnemies.Count; i++)
        {
            if (nearEnemies[i].gameObject.activeInHierarchy == false)
                nearEnemies.Remove(nearEnemies[i]);
        }
        return;
    }

    protected Character GetFirstEnemyInTheList()
    {
        if (nearEnemies.Count > 0)
            return nearEnemies[0].GetComponent<Character>();
        else return null;
    }

    protected bool IsFirstEnemyInTheListActive()
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
        EventManager.TriggerEvent<DetectEvent>(new DetectEvent(DetectEventType.TargetLost, character, nearEnemies[0].transform));
        nearEnemies.RemoveAt(0);
        if(nearEnemies.Count > 0)
            EventManager.TriggerEvent<DetectEvent>(new DetectEvent(DetectEventType.TargetAcquired, character, nearEnemies[0].transform));

        return;
    }

    private void AddEnemyToTheList(Character _enemy)
    {
        if (nearEnemies.Count == 0 || !nearEnemies.Contains(_enemy))
        {
            nearEnemies.Add(_enemy);
            return;
        }
        else return;
    }

    private void RemoveEnemyFromTheList(Character _enemy)
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
            AddEnemyToTheList(_collider.GetComponent<Character>());
            return;
        }
        else return;
    }

    public virtual void OnTriggerExit(Collider _collider)
    {
        if (_collider.CompareTag("Enemy"))
        {
            RemoveEnemyFromTheList(_collider.GetComponent<Character>());
            if(nearEnemies.Count == 0)
            {
                EventManager.TriggerEvent<DetectEvent>(new DetectEvent(DetectEventType.TargetLost, character, _collider.transform));
            }
            return;
        }
        else return;
    }
}
