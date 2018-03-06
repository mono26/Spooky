using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyMovement
{
    [SerializeField]
    private Enemy enemy;
    private Settings settings;
    private NavMeshPath path;

    private float currentSpeed;

    public EnemyMovement(Enemy _enemy, Settings _settings)
    {
        // Constructor for the class passes all the required components into the class.
        // Done before the game starts
        enemy = _enemy;
        settings = _settings;
        path = new NavMeshPath();
    }

    public void Start()
    {
        // When the game starts set values
        //navMesh.updatePosition = false;
        enemy.settings.NavMesh.updateRotation = false;
        enemy.settings.NavMesh.updateUpAxis = false;

        enemy.settings.NavMesh.CalculatePath(enemy.settings.Target.position, path);

        currentSpeed = settings.MaxSpeed;
    }

    // TODO check if it's better to use Update() or FixedUpdate()
    public void FixedUpdate()
    {
        if (!path.Equals(null))
        {
            // TODO calculate dot and cross product for input values moving the rigidBody
            Vector3 desiredDirection = (path.corners[1] - enemy.settings.Rigidbody.transform.position).normalized;
            desiredDirection.y = 0;
            float horizontal = Vector3.Dot(enemy.settings.Rigidbody.transform.right, desiredDirection);
            float vertical = -Vector3.Cross(enemy.settings.Rigidbody.transform.right, desiredDirection).y;

            if (!horizontal.Equals(0f) || !vertical.Equals(0f))
            {
                // We need to pass a movement in y because of the rotation of the object, so the sprite can be seen.
                Vector3 direction = new Vector3(horizontal, vertical, 0);
                Move(direction);
                var time = Time.realtimeSinceStartup;
                Debug.Log(string.Format("time {0} is moving", time));
                enemy.settings.NavMesh.CalculatePath(enemy.settings.Target.position, path);
                return;
            }
            else
            {
                enemy.settings.NavMesh.CalculatePath(enemy.settings.Target.position, path);
                return;
            }
        }
        else return;
    }

    private void Move(Vector3 _direction)
    {
        // For moving the rigidbody in the desired direction
        Vector3 newPosition = (enemy.settings.Rigidbody.position + 
                                enemy.settings.Rigidbody.transform.TransformDirection(_direction) * 
                                currentSpeed * 
                                Time.fixedDeltaTime);

        enemy.settings.Rigidbody.MovePosition(newPosition);
        return;
    }

    [System.Serializable]
    public class Settings
    {
        public float MaxSpeed;
        public float SlowMotionSpeed;

        public float MaxXValue;
        public float MaxYValue;
    }
}
