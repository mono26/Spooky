using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Plantpoint : MonoBehaviour
{
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

    private bool isUpgraded;

    protected void Awake()
    {
        soundSource = GetComponent<AudioSource>();

        return;
    }

    public void BuildPlant(PlantBlueprint blueprint)
    {
        LevelManager.Instance.TakeMoney(blueprint.price);
        LevelManager.Instance.UpdateMoneyDisplay();

        if (buySound != null)
            SoundManager.Instance.PlaySfx(soundSource, buySound);

        currentPlant = Instantiate(blueprint.plant.gameObject, transform.position, transform.rotation).GetComponent<Character>();
        currentBlueprint = blueprint;
        //PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();

        VisualEffects.CreateVisualEffect(buyVfx, transform);

        return;
    }

    public void SellPlant()
    {
        if (!isUpgraded)
        {
            LevelManager.Instance.GiveMoney(currentBlueprint.price);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        }
        else if (isUpgraded)
        {
            LevelManager.Instance.GiveMoney(currentBlueprint.upgradePrice);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        }

        if (sellSound != null)
            SoundManager.Instance.PlaySfx(soundSource, sellSound);

        return;
    }

    public void UpgradePlant()
    {
        LevelManager.Instance.TakeMoney(currentBlueprint.upgradePrice);
        Destroy(currentPlant.gameObject);

        if (upgradeSound != null)
            SoundManager.Instance.PlaySfx(soundSource, upgradeSound);

        currentPlant = Instantiate(currentBlueprint.upgradePlant.gameObject, transform.position, transform.rotation).GetComponent<Character>();
        isUpgraded = true;
        PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();

        VisualEffects.CreateVisualEffect(upgradeVfx, transform);

        return;
    }

    public void ClearPlantPoint()
    {
        currentBlueprint = null;
        currentPlant = null;
        return;
    }
}
