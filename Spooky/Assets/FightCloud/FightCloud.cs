using System.Collections;
using UnityEngine;

public enum FightCloudEventType { StartFight, EndFight }

public class FightCloudEvent : SpookyCrowEvent
{
    public Character player;
    public Character enemy;
    public FightCloudEventType type;

    public FightCloudEvent(Character _player, Character _enemy, FightCloudEventType _type)
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
    private Character enemy;

    private void Start()
    {
        gameObject.SetActive(false);
        return;
    }

    private void OnMouseDown()
    {
        Debug.Log("Pressing FightCloud");
        StartCoroutine(EndFight());
        return;
    }

    public void PrepareFight(Character _player, Character _enemy)
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

    private IEnumerator EndFight()
    {
        EventManager.TriggerEvent<FightCloudEvent>(new FightCloudEvent(player, enemy, FightCloudEventType.EndFight));

        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
        yield return null;

        Health hasHealthComponent = enemy.GetComponent<Health>();
        if (hasHealthComponent != null)
        {
            hasHealthComponent.StartCoroutine(hasHealthComponent.Kill());
        }

        player = null;
        enemy = null;

        gameObject.SetActive(false);
        yield break;
    }
}
