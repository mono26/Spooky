using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Plantpoint : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField]
    protected AudioClip buySound;
    [SerializeField]
    protected AudioClip sellSound;
    [SerializeField]
    protected AudioClip upgradeSound;

    [Header("Plantpoint info (For debugging only")]
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

    public void BuildPlant(PlantBlueprint blueprint)       //Luego de que se tenga una planta seleccionada cuando se escoja un nodo se construira ahi
    {
        LevelManager.Instance.TakeMoney(blueprint.price);
        LevelUIManager.Instance.UpdateMoneyDisplay();

        if (buySound != null)
            SoundManager.Instance.PlaySfx(soundSource, buySound);

        currentPlant = Instantiate(blueprint.plant.gameObject, transform.position, transform.rotation).GetComponent<Character>();
        currentBlueprint = blueprint;

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
        currentPlant.transform.position = transform.position;
        PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();

        return;
    }

    public void ClearPlantPoint()
    {
        currentBlueprint = null;
        currentPlant = null;
        return;
    }
}
