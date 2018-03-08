using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPoint : MonoBehaviour
{
    public PlantBlueprint plantBluePrint;
    public Plant currentPlant;

    //public AudioClip[] plantPointSounds;

    void Start()
    {

    }
    public void OnClicked()
    {
        //Si ya hay una planta y un blueprint se selecciona el nodo
        if (currentPlant && plantBluePrint)
        {
            PlantStore.Instance.SelectPlantPoint(this);
            //Ejecutar codigo para poder subir de nivel o vender la torre
        }
        if (!currentPlant && !plantBluePrint)
        {
            PlantStore.Instance.SelectBuildPoint(this);
            //Ejecutar codigo para que salga el canvas con los botones.
        }
    }
    public void BuildPlant(PlantBlueprint blueprint)       //Luego de que se tenga una planta seleccionada cuando se escoja un nodo se construira ahi
    {
        //GameObject plant = PoolsManagerPlants.Instance.GetPlant(blueprint.plant.plantInfo.objectIndex);
        //plant.transform.position = transform.position;
        //currentPlant = plant.GetComponent<Plant>();
        plantBluePrint = blueprint;
        LevelManager.Instance.TakeMoney(blueprint.price);
        //SoundHandler.Instance.PlayClip(plantPointSounds[0]);
    }
    public void SellPlant()
    {
        //GameManager.Instance.GiveMoney(currentPlant.GetComponent<AIPlantController>().plantInfo.plantReward);
        plantBluePrint = null;
        //PoolsManagerPlants.Instance.ReleasePlant(currentPlant.gameObject);
        currentPlant = null;
        PlantStore.Instance.DeselectPlantPoint();
        //SoundHandler.Instance.PlayClip(plantPointSounds[1]);
    }
    public void UpgradePlant()
    {
        LevelManager.Instance.TakeMoney(plantBluePrint.upgradePrice);
        //PoolsManagerPlants.Instance.ReleasePlant(currentPlant.gameObject);
        //currentPlant = PoolsManagerPlants.Instance.GetPlant(plantBluePrint.upgradePlant.plantInfo.objectIndex).GetComponent<PlantController>();
        currentPlant.transform.position = transform.position;
        PlantStore.Instance.DeselectPlantPoint();
        //SoundHandler.Instance.PlayClip(plantPointSounds[2]);
    }
    public void DestroyPlant()
    {
        plantBluePrint = null;
        currentPlant = null;
    }
}
