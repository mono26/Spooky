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
    [SerializeField]
    private float basicCooldown;
    public float BasicCooldown { get { return basicCooldown; } }
    [SerializeField]
    private float basicRange;
    public float BasicRange { get { return basicRange; } }
}
