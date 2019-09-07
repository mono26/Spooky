using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Plantpoint : MonoBehaviour
{
    public delegate void PlantoPointEvent(Plantpoint plantPoint);
    public static event PlantoPointEvent PlantPointClickedEvent;

    [Header("Visual Effects")]
    [SerializeField]
    protected GameObject buyVfx;
    [SerializeField]
    protected GameObject upgradeVfx;

    [Header("Sounds")]
    [SerializeField]
    protected AudioClip buySound;
    [SerializeField]
    protected AudioClip sellSound;
    [SerializeField]
    protected AudioClip upgradeSound;

    [Header("Editor debugging")]
    [SerializeField]
    protected PlantBlueprint currentBlueprint;
    public PlantBlueprint CurrentBlueprint { get { return currentBlueprint; } }
    [SerializeField]
    protected Character currentPlant;
    public Character CurrentPlant { get { return currentPlant; } }

    protected AudioSource soundSource;

    private bool isUpgraded = false;
    private bool canBeSelected = false;
    
    protected void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }

    public void BuildPlant(PlantBlueprint blueprint)
    {
        if(blueprint.plant != null)
        {
            LevelManager.Instance.TakeMoney(blueprint.price);
            LevelManager.Instance.UpdateMoneyDisplay();

            if (buySound != null)
                SoundManager.Instance.PlaySfx(soundSource, buySound);

            currentPlant = Instantiate(blueprint.plant.gameObject, transform.position, transform.rotation).GetComponent<Character>();
            currentBlueprint = blueprint;
            //PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();

            VisualEffects.CreateVisualEffect(buyVfx, transform);
        }

        return;
    }

    public void SellPlant()
    {
        if (!isUpgraded)
        {
            LevelManager.Instance.GiveMoney(currentBlueprint.price);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            // PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        }
        else if (isUpgraded)
        {
            LevelManager.Instance.GiveMoney(currentBlueprint.upgradePrice);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            // PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        }

        if (sellSound != null)
            SoundManager.Instance.PlaySfx(soundSource, sellSound);

        return;
    }

    public void UpgradePlant()
    {
        if(currentBlueprint.upgradePlant != null)
        {
            LevelManager.Instance.TakeMoney(currentBlueprint.upgradePrice);
            Destroy(currentPlant.gameObject);

            if (upgradeSound != null)
                SoundManager.Instance.PlaySfx(soundSource, upgradeSound);

            currentPlant = Instantiate(currentBlueprint.upgradePlant.gameObject, transform.position, transform.rotation).GetComponent<Character>();
            isUpgraded = true;
            // PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();

            VisualEffects.CreateVisualEffect(upgradeVfx, transform);
        }

        return;
    }

    public void ClearPlantPoint()
    {
        currentBlueprint = null;
        currentPlant = null;
        return;
    }

    // public void Detect()
    // {
    //     if (!currentPlantPoint && PlantStore.Instance.CurrentPlantPoint)
    //     {
    //         PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();
    //         PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
    //         return;
    //     }

    //     return;
    // }

    // private void ClearCurrentPlantPoint()
    // {
    //     if (currentPlantPoint == true)
    //     {
    //         if (currentPlant == true)
    //             PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
    //         else if (currentPlant == false)
    //             PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();

    //         currentPlantPoint = null;
    //     }

    //     else return;
    // }

    public void SelectNewPlantpoint()
    {
        Debug.Log("hi");
       // ClearCurrentPlantPoint();
        ChangeCurrentPlantPointAndDisplay(this);
    }

    private void ChangeCurrentPlantPointAndDisplay(Plantpoint _plantPoint)
    {
        Plantpoint currentPlantPoint = this;
        if (currentPlant == true)
        {
            // PlantStore.Instance.ActivatePlantUI(currentPlantPoint);
            return;
        }
        else if (currentPlant == false)
        {
            // PlantStore.Instance.ActivateBuildUI(currentPlantPoint);
            return;
        }
    }

    public void CanSelectPlantPoint(bool select)
    {
        canBeSelected = select;
    }

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
        if (currentBlueprint && currentPlant)
        {
            isEmpty = true;
        }
        return isEmpty;
    }
}
