using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/StatCollection")]
public class EnemyStatsCollection : ScriptableObject
{
    [Header("Stealer Settings")]
    [SerializeField]
    public static EnemyStats stealerStats = new EnemyStats();

    [Header("Attacker Settings")]
    [SerializeField]
    public static EnemyStats attackerStats = new EnemyStats();
}
