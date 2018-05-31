using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField]
    private float attacksPerSecond;
    public float AttacksPerSecond { get { return attacksPerSecond; } }
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get { return maxHealth; } }
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } }
}
