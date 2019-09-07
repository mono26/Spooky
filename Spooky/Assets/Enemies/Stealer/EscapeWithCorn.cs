using UnityEngine;
using System.Collections;

public class EscapeWithCorn : CharacterAction
{
    [SerializeField]
    private PoolableObject poolableOBJ = null;

    protected override void Awake()
    {
        if (!poolableOBJ)
        {
            poolableOBJ = GetComponent<PoolableObject>();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        target = LevelManager.Instance.GetRandomEscapePoint();
    }

    protected override IEnumerator Action()
    {
        if(character != null)
        {
            EventManager.TriggerEvent<CharacterEvent>(new CharacterEvent(CharacterEventType.Release, character));
        }
        // Stop the action executiong because the animation has already end.
        if (poolableOBJ)
        {
            PoolsManager.ReturnObjectToPools(poolableOBJ);
        }
        else
        {
            character.gameObject.SetActive(false);
        }
        yield break;
    }

}
