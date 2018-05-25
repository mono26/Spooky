using UnityEngine;

public class UpgradedPlant : Plant
{
    // Assigned by inspector.
    [SerializeField]
    private CharacterAction specialAction;
    public CharacterAction SpecialAction { get { return specialAction; } }

    protected override void Awake()
    {
        if (specialAction == null)
            Debug.LogError(this.gameObject.ToString() + "No special action assigned on the enemy gameObject: ");

        base.Awake();
    }

    protected override void Update()
    {
        if (enemyDetect.IsFirstEnemyInTheListActive() && !IsExecutingAction)
        {
            if (specialAction.IsTargetInRange())
            {
                StartCoroutine(specialAction.ExecuteAction());
            }
        }

        base.Update();

        return;
    }
}
