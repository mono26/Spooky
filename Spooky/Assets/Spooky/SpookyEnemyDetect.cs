using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookEnemyDetect : MonoBehaviour
{
    // Give the value of the range in settings to the radius of the collider
    // it must be in a separate layer to wrk properly.
    private Collider detectionTrigger;
    private Settings settings;

    // Create a stack to store all the enemies that come in range
    public SpookEnemyDetect()
    {
        // Constructor, sets all needed dependencies.
    }

    public void Update()
    {
        // To check if the top of the stack is out of range or dead, or any other condition for clearing it
    }
    private void ClearCurrentTarget()
    {
        // Clear the top of the stack if its destroyes, null, or inactive, out of range
    }

    public void OnTriggerEnter(Collider other)
    {
        // When an enemy enters the collider
    }

    public class Settings
    {
        public float EnemyDetectionRange;
    }
}
