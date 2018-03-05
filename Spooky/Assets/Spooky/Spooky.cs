using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spooky : MonoBehaviour
{
    public Settings settings;
    // This is going to be the class the connects all the components of spooky
    // Execute update, triggerevetns, awake, start, etc.
    public SpookyMovement movementComponent;
    [SerializeField]
    public SpookyAttack attackComponent;
    public SpookyEnemyDetect enemyDetectComponent;

    public delegate void FireButtonPress();

    public event FireButtonPress OnFireButtonPressed;

    private void Awake()
    {
        movementComponent = new SpookyMovement(settings.Rigidbody, settings.Joystick, settings.MovementSettings);
        attackComponent = new SpookyAttack(this, settings.Hand, settings.ShootPosition, settings.Bullet, settings.Joystick, settings.AttackSettings);
        enemyDetectComponent = new SpookyEnemyDetect(settings.EnemyDetectTrigger, settings.EnemyDetectSettings);
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
    }

    private void FixedUpdate()
    {
        movementComponent.FixedUpdate();
    }

    public void FireButton()
    {
        if (OnFireButtonPressed != null)
            OnFireButtonPressed();
    }

    [System.Serializable]
    public class Settings
    {
        public Rigidbody Rigidbody;
        public Joystick Joystick;
        public Collider EnemyDetectTrigger;
        public Transform Hand;
        public Bullet Bullet;
        public Transform ShootPosition;

        public SpookyMovement.Settings MovementSettings;
        public SpookyEnemyDetect.Settings EnemyDetectSettings;
        public SpookyAttack.Settings AttackSettings;
    }
}
