using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/StatsCollection")]
public class EnemyStatsCollection : ScriptableObject
{
    [Header("Stealer Settings")]
    [SerializeField]
    private EnemyStats stealerStats;
    public EnemyStats StealerStats { get { return stealerStats; } }

    [Header("Attacker Settings")]
    [SerializeField]
    private EnemyStats attackerStats;
    public EnemyStats AttackerStats { get { return attackerStats; } }
}
