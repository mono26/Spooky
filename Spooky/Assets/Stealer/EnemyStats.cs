[System.Serializable]
public struct EnemyStats
{
    public float maxHealth, movementSpeed, basicCooldown, basicRange;

    public EnemyStats(float _maxhealth, float _movementSpeed, float _basicCooldown, float _basicRange)
    {
        maxHealth = _maxhealth;
        movementSpeed = _movementSpeed;
        basicCooldown = _basicCooldown;
        basicRange = _basicRange;
    }
}
