using UnityEngine;

public enum FightCloudEventType { StartFight, EndFight }

public class FightCloudEvent : SpookyCrowEvent
{
    public Character player;
    public Enemy enemy;
    public FightCloudEventType type;

    public FightCloudEvent(Character _player, Enemy _enemy, FightCloudEventType _type)
    {
        player = _player;
        enemy = _enemy;
        type = _type;
    }
}

public class FightCloud : SceneSingleton<FightCloud>
{
    [SerializeField]
    private Character player;
    [SerializeField]
    private Enemy enemy;

    private void Start()
    {
        gameObject.SetActive(false);
        return;
    }

    private void OnMouseDown()
    {
        EndFight();
        return;
    }

    public void PrepareFight(Character _player, Enemy _enemy)
    {
        player = _player;
        enemy = _enemy;

        StartFight();

        return;
    }

    private void StartFight()
    {
        EventManager.TriggerEvent<FightCloudEvent>(new FightCloudEvent(player, enemy, FightCloudEventType.StartFight));
        gameObject.SetActive(true);

        transform.position = player.transform.position;

        player.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);

        return;
    }

    private void EndFight()
    {
        EventManager.TriggerEvent<FightCloudEvent>(new FightCloudEvent(player, enemy, FightCloudEventType.EndFight));
        gameObject.SetActive(true);

        player.gameObject.SetActive(true);
        StartCoroutine(enemy.HealthComponent.Kill());

        player = null;
        enemy = null;
        return;
    }
}
