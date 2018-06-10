using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoOrderInLayer : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer sprite;
    protected const string autoOderLayer = "AutoOrderLayer";
    [SerializeField]
    protected float updatesPerSecond = 0;

    protected void Awake()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        return;
    }

    protected void Start()
    {
        sprite.sortingLayerName = autoOderLayer;
        AutoAssignOrderInLayer();

        // Because not all the objects move in the scene and need update
        if (updatesPerSecond > 0)
            StartCoroutine(UpdateLayerOrder());

        return;
	}

    protected IEnumerator UpdateLayerOrder()
    {
        AutoAssignOrderInLayer();
        yield return new WaitForSeconds(1 / updatesPerSecond);
        StartCoroutine(UpdateLayerOrder());
    }

    protected void AutoAssignOrderInLayer()
    {
        // We need to invert the value to get proper rendering
        sprite.sortingOrder = Mathf.RoundToInt(transform.position.z) * -1;
        return;
    }
}
