using UnityEngine;

public class Plantpoint : MonoBehaviour
{
    public PlantBlueprint currentBlueprint;
    public Plant currentPlant;

    private bool isUpgraded;

    //public AudioClip[] plantPointSounds;

    void Start()
    {

    }

    public void BuildPlant(PlantBlueprint blueprint)       //Luego de que se tenga una planta seleccionada cuando se escoja un nodo se construira ahi
    {
        LevelManager.Instance.UiManager.TakeMoney(blueprint.price);
        currentPlant = Instantiate(blueprint.plant.gameObject, transform.position, transform.rotation).GetComponent<Plant>();
        currentBlueprint = blueprint;
        //SoundHandler.Instance.PlayClip(plantPointSounds[0]);
        return;
    }

    public void SellPlant()
    {
        if (!isUpgraded)
        {
            LevelManager.Instance.UiManager.GiveMoney(currentBlueprint.price);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        }
        else if (isUpgraded)
        {
            LevelManager.Instance.UiManager.GiveMoney(currentBlueprint.upgradePrice);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        }
        //SoundHandler.Instance.PlayClip(plantPointSounds[1]);
        return;
    }

    public void UpgradePlant()
    {
        LevelManager.Instance.UiManager.TakeMoney(currentBlueprint.upgradePrice);
        Destroy(currentPlant.gameObject);
        currentPlant = Instantiate(currentBlueprint.upgradePlant.gameObject, transform.position, transform.rotation).GetComponent<Plant>();
        isUpgraded = true;
        currentPlant.transform.position = transform.position;
        PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
        //SoundHandler.Instance.PlayClip(plantPointSounds[2]);
        return;
    }

    public void ClearPlantPoint()
    {
        currentBlueprint = null;
        currentPlant = null;
        return;
    }
}
