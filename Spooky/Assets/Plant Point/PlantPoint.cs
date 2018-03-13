using UnityEngine;

public class PlantPoint : MonoBehaviour
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
        LevelManager.Instance.TakeMoney(blueprint.price);
        currentPlant = Instantiate(blueprint.plant.gameObject, transform.position, transform.rotation).GetComponent<Plant>();
        currentBlueprint = blueprint;
        //SoundHandler.Instance.PlayClip(plantPointSounds[0]);
        return;
    }

    public void SellPlant()
    {
        if (!isUpgraded)
        {
            LevelManager.Instance.GiveMoney(currentBlueprint.price);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            PlantStore.Instance.DeselectPlantPoint();
        }
        else if (isUpgraded)
        {
            LevelManager.Instance.GiveMoney(currentBlueprint.upgradePrice);
            Destroy(currentPlant.gameObject);
            ClearPlantPoint();
            PlantStore.Instance.DeselectPlantPoint();
        }
        //SoundHandler.Instance.PlayClip(plantPointSounds[1]);
        return;
    }

    public void UpgradePlant()
    {
        LevelManager.Instance.TakeMoney(currentBlueprint.upgradePrice);
        Destroy(currentPlant.gameObject);
        currentPlant = Instantiate(currentBlueprint.upgradePlant.gameObject, transform.position, transform.rotation).GetComponent<Plant>();
        currentPlant.transform.position = transform.position;
        PlantStore.Instance.DeselectPlantPoint();
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
