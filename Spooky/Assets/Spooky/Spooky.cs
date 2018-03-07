﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spooky : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public Joystick Joystick;
    public SphereCollider EnemyDetectTrigger;
    public float DetectionRange;
    public Transform Hand;
    public Bullet Bullet;
    public Transform ShootPosition;
    public Settings settings;
    // This is going to be the class the connects all the components of spooky
    // Execute update, triggerevetns, awake, start, etc.
    private SpookyMovement movementComponent;
    private SpookyAttack attackComponent;
    [SerializeField]
    private EnemyDetect enemyDetectComponent;

    public delegate void FireButtonPress();

    public event FireButtonPress OnFireButtonPressed;

    private void Awake()
    {
        movementComponent = new SpookyMovement(
            Rigidbody,
            Joystick,
            settings.MovementSettings
            );

        enemyDetectComponent = new EnemyDetect(
            gameObject,
            EnemyDetectTrigger,
            DetectionRange
            );

        attackComponent = new SpookyAttack(
            this,
            enemyDetectComponent,
            Hand,
            ShootPosition,
            Bullet,
            Joystick,
            settings.AttackSettings
            );
    }

    private void OnEnable()
    {
        attackComponent.OnEnable();
    }

    private void OnDisable()
    {
        attackComponent.OnDisable();
    }

    private void Start()
    {
        // Execute all Start() functions in the components. Here is variable setting.
        movementComponent.Start();
    }

    private void Update()
    {
        attackComponent.Update();
        enemyDetectComponent.Detect();
    }

    private void FixedUpdate()
    {
        movementComponent.FixedUpdate();
    }

    public void OnTriggerEnter(Collider _collider)
    {
        enemyDetectComponent.OnTriggerEnter(_collider);
    }

    public void OnTriggerExit(Collider _collider)
    {
        enemyDetectComponent.OnTriggerExit(_collider);
    }

    public void FireButton()
    {
        if (OnFireButtonPressed != null)
            OnFireButtonPressed();
    }

    [System.Serializable]
    public class Settings
    {
        public SpookyMovement.Settings MovementSettings;
        public SpookyAttack.Settings AttackSettings;
    }
}
