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

    public void InitializePicakble(int _cornValue)
    {
        _cornGain = _cornValue;
        return;
    }

    protected void OnTriggerEnter(Collider _collider)
    {
        if(_collider.CompareTag("Spooky") == true)
        {
            Debug.Log("Here i am");
            Pick(_collider.gameObject);
        }
    }
}
