﻿using UnityEngine;

public class PlantStore : MonoBehaviour
{
    //Singleton part
    private static PlantStore instance;
    public static PlantStore Instance { get { return instance; } }

    [SerializeField]
    private GameObject buildCanvas;
    [SerializeField]
    private GameObject plantCanvas;

    private Plantpoint currentPlantPoint;
    public Plantpoint CurrentPlantPoint { get { return currentPlantPoint; } }

    private AudioClip[] uiSounds;

    [SerializeField]
    private float zOffset = 0.7f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    //Parte del singleton en donde se asigna la unica instancia de la clase
        }
        else
            Destroy(gameObject);

        buildCanvas = GameObject.FindGameObjectWithTag("BuildCanvas");
        plantCanvas = GameObject.FindGameObjectWithTag("PlantCanvas");

        HideBuildUI();
        HidePlantUI();
    }

    //Metodos para el manejo de los plantpoints y la UI
    public void SelectPlantPoint(Plantpoint plantPoint)     //Metodo que se llamara cada vez que el jugador haga click sobre un plant point.
    {
        if (currentPlantPoint == plantPoint)
        {
            DeselectPlantPoint();
            return;
        }
        currentPlantPoint = plantPoint;
        SetPlantPoint(currentPlantPoint);
    }
    public void SelectBuildPoint(Plantpoint plantPoint)     //Metodo que se llamara cada vez que el jugador haga click sobre un plant point.
    {
        if (currentPlantPoint == plantPoint)
        {
            DeselectBuildPoint();
            return;
        }
        currentPlantPoint = plantPoint;
        SetBuildPoint(currentPlantPoint);
    }
    public void DeselectPlantPoint()        //Function for deselection the plantpoint
    {
        currentPlantPoint = null;
        HidePlantUI();
    }
    public void DeselectBuildPoint()        //Function for deselection the plantpoint
    {
        currentPlantPoint = null;
        HideBuildUI();
    }
    public void SetPlantPoint(Plantpoint plantPoint)
    {
        //Si el plantPoint tiene una planta se activa el plantpointUI con la informacion de la planta.
        currentPlantPoint = plantPoint;
        plantCanvas.transform.position = currentPlantPoint.transform.position + new Vector3(0, 0, zOffset);
        plantCanvas.SetActive(true);
    }
    public void SetBuildPoint(Plantpoint plantPoint)
    {
        //Cuadno el plant poin esta vacio para sacar el buildUI
        currentPlantPoint = plantPoint;
        buildCanvas.transform.position = currentPlantPoint.transform.position + new Vector3(0, 0, zOffset);
        buildCanvas.SetActive(true);
    }
    public void HideBuildUI()
    {
        buildCanvas.SetActive(false);
    }
    public void HidePlantUI()
    {
        plantCanvas.SetActive(false);
    }

    //Esta parte del script esta dedicada a las funiones de la UI de las plantas. Tanto para la UI de la
    //planta como el UI de construccion.
    public void PurchasePlant(PlantBlueprint bluePrint)
    {
        if (LevelManager.Instance.UiManager.CurrentMoney >= bluePrint.price)
        {
            currentPlantPoint.BuildPlant(bluePrint);
            //SoundHandler.Instance.PlayClip(uiSounds[0]);
            //SoundHandler.Instance.PlayClip(uiSounds[1]);
            DeselectBuildPoint();
        }
        else return;
    }
    public void Upgrade()
    {
        if (LevelManager.Instance.UiManager.CurrentMoney > currentPlantPoint.currentBlueprint.upgradePrice)
        {
            currentPlantPoint.UpgradePlant();
            //SoundHandler.Instance.PlayClip(uiSounds[0]);
        }
        else return;
    }
    public void Sell()
    {
        currentPlantPoint.SellPlant();
        //SoundHandler.Instance.PlayClip(uiSounds[0]);
        DeselectPlantPoint();
    }
}
