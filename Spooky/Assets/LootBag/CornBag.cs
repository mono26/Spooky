using UnityEngine;

public class CornBag : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    protected int _cornGain;
    [SerializeField]
    protected GameObject pickVfx;

    protected void Pick(GameObject _whoPicks)
    {
        LevelManager.Instance.GiveCrop(_cornGain);
        
        if(pickVfx != null)
        {
            Instantiate(pickVfx, transform.position, transform.rotation);
        }

        return;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        if(_collider.CompareTag("Spooky"))
        {
            Pick(_collider.gameObject);
        }
    }
}
