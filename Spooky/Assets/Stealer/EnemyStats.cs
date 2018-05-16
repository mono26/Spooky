using UnityEngine;

[System.Serializable]
public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get { return maxHealth; } }
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } }
}
