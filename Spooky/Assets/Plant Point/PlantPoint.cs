using UnityEngine;

public class PlantPoint : MonoBehaviour
{
    public PlantBlueprint currentBlueprint;
    public Plant currentPlant;

    //public AudioClip[] plantPointSounds;

    void Start()
    {

    }
    public void OnClicked()
    {
        //Si ya hay una planta y un blueprint se selecciona el nodo
        if (currentPlant && currentBlueprint)
        {
            PlantStore.Instance.SelectPlantPoint(this);
            //Ejecutar codigo para poder subir de nivel o vender la torre
        }
        if (!currentPlant && !currentBlueprint)
        {
            PlantStore.Instance.SelectBuildPoint(this);
            //Ejecutar codigo para que salga el canvas con los botones.
        }
    }
    public void BuildPlant(PlantBlueprint blueprint)       //Luego de que se tenga una planta seleccionada cuando se escoja un nodo se construira ahi
    {
        GameObject plant =Instantiate(blueprint.plant.gameObject, transform.position, transform.rotation);
        currentPlant = plant.GetComponent<Plant>();
        currentBlueprint = blueprint;
        LevelManager.Instance.TakeMoney(blueprint.price);
        //SoundHandler.Instance.PlayClip(plantPointSounds[0]);
    }
    public void SellPlant()
    {
        LevelManager.Instance.GiveMoney(currentBlueprint.price);
        Destroy(currentPlant.gameObject);
        ClearPlantPoint();
        PlantStore.Instance.DeselectPlantPoint();
        //SoundHandler.Instance.PlayClip(plantPointSounds[1]);
    }
    public void UpgradePlant()
    {
        LevelManager.Instance.TakeMoney(currentBlueprint.upgradePrice);
        //PoolsManagerPlants.Instance.ReleasePlant(currentPlant.gameObject);
        //currentPlant = PoolsManagerPlants.Instance.GetPlant(plantBluePrint.upgradePlant.plantInfo.objectIndex).GetComponent<PlantController>();
        currentPlant.transform.position = transform.position;
        PlantStore.Instance.DeselectPlantPoint();
        //SoundHandler.Instance.PlayClip(plantPointSounds[2]);
    }
    public void ClearPlantPoint()
    {
        currentBlueprint = null;
        currentPlant = null;
    }
}
