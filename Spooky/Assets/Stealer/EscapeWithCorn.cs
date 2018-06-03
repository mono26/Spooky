using System.Collections;

public class EscapeWithCorn : CharacterAction
{
    protected override void Start()
    {
        base.Start();

        target = LevelManager.Instance.GetRandomEscapePoint();

        return;
    }

    protected override IEnumerator Action()
    {
        // Stop the action executiong because the animation has already end.
        if (GetComponent<PoolableObject>())
        {
            GetComponent<PoolableObject>().Release();
        }
        else
            character.gameObject.SetActive(false);

        yield break;
    }

}
