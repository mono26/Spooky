using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Plantpoint : MonoBehaviour
{
    public delegate void PlantPointEvent(Plantpoint plantPoint);
    public static event PlantPointEvent PlantPointClickedEvent;
    // public delegate void PlantPointPlantEvent(Plantpoint plantPoint, PlantBlueprint blueprint);
    // public static event PlantPointPlantEvent BuildPlantEvent;
    // public static event PlantPointPlantEvent UpgradePlantEvent;
    // public static event PlantPointPlantEvent SellPlantEvent;

    [Header("Visual Effects")]
    [SerializeField]
    private GameObject buyVfx;
    [SerializeField]
    private GameObject upgradeVfx;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip buySound;
    [SerializeField]
    private AudioClip sellSound;
    [SerializeField]
    private AudioClip upgradeSound;

    [Header("Editor debugging")]
    [SerializeField]
    private PlantBlueprint currentBlueprint;
    [SerializeField]
    private Character currentPlant;
    [SerializeField]
    private bool isUpgraded = false;

    private AudioSource soundSource;
    private bool canBeSelected = false;

    public PlantBlueprint CurrentBlueprint { get { return currentBlueprint; } set { currentBlueprint = value; } }
    public Character CurrentPlant { get { return currentPlant; } set { currentPlant = value; } }
    public bool IsUpgraded { get { return isUpgraded; } }
    
    protected void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void PlantPlant(PlantBlueprint blueprint)
    {
        if(blueprint.plant != null)
        {
            // LevelManager.Instance.TakeMoney(blueprint.price);
            // LevelManager.Instance.UpdateMoneyDisplay();
            if (buySound != null)
            {
                SoundManager.Instance.PlaySfx(soundSource, buySound);
            }
            currentPlant = Instantiate(blueprint.plant, transform.position, transform.rotation);
            currentBlueprint = blueprint;
            //PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();
            VisualEffects.CreateVisualEffect(buyVfx, transform);
        }

        return;
    }

    public void RemovePlant()
    {
        // Pool plant
        Destroy(currentPlant.gameObject);
        ClearPlantPoint();
        if (sellSound != null)
        {
            SoundManager.Instance.PlaySfx(soundSource, sellSound);
        }
    }

    public void UpgradePlant()
    {
        if(currentBlueprint.upgradePlant != null)
        {
            // Pool plant
            Destroy(currentPlant.gameObject);
            // Manejarlo con eventos
            if (upgradeSound != null)
            {
                SoundManager.Instance.PlaySfx(soundSource, upgradeSound);
            }     
            currentPlant = Instantiate(currentBlueprint.upgradePlant, transform.position, transform.rotation);
            isUpgraded = true;
            VisualEffects.CreateVisualEffect(upgradeVfx, transform);
        }
    }

    public void ClearPlantPoint()
    {
        currentBlueprint = null;
        currentPlant = null;
    }

    public void CanSelectPlantPoint(bool select)
    {
        canBeSelected = select;
    }

    // Should be interface.
    /// <summary>
    /// Called when the user clicks.
    /// </summary>
    public void OnClicked()
    {
        if (canBeSelected)
        {
            Debug.Log("Plantpoint clicked");
            PlantPointClickedEvent?.Invoke(this);
        }
    }

    public bool IsEmpty()
    {
        bool isEmpty = false;
        if (!currentBlueprint && !currentPlant)
        {
            isEmpty = true;
        }
        return isEmpty;
    }
}
