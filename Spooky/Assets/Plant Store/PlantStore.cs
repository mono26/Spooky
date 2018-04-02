using UnityEngine;

public class PlantStore : MonoBehaviour
{
    //Singleton part
    private static PlantStore instance;
    public static PlantStore Instance { get { return instance; } }

    [SerializeField]
    private GameObject buildCanvasUI;
    [SerializeField]
    private GameObject plantCanvasUI;

    private Plantpoint currentlyActivePlantPoint;
    public Plantpoint CurrentPlantPoint { get { return currentlyActivePlantPoint; } }

    private AudioClip[] uiSounds;

    [SerializeField]
    private float zOffsetForCanvasLocation = 0.7f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    //Parte del singleton en donde se asigna la unica instancia de la clase
        }
        else
            Destroy(gameObject);

        buildCanvasUI = GameObject.FindGameObjectWithTag("BuildCanvas");
        plantCanvasUI = GameObject.FindGameObjectWithTag("PlantCanvas");

        HideBuildUI();
        HidePlantUI();
    }

    //Metodos para el manejo de los plantpoints y la UI
    public void ActivatePlantUI(Plantpoint plantPoint)     //Metodo que se llamara cada vez que el jugador haga click sobre un plant point.
    {
        DeselectCurrentActivePlantpointWithPlant();
        currentlyActivePlantPoint = plantPoint;
        SetCurrentActivePlantPoint(currentlyActivePlantPoint);
        return;
    }

    public void ActivateBuildUI(Plantpoint plantPoint)     //Metodo que se llamara cada vez que el jugador haga click sobre un plant point.
    {
        DeselectCurrentActivePlantpointWithPlant();
        currentlyActivePlantPoint = plantPoint;
        SetActiveBuildpoint(currentlyActivePlantPoint);
        return;
    }

    public void DeselectCurrentActivePlantpointWithPlant()        //Function for deselection the plantpoint
    {
        currentlyActivePlantPoint = null;
        HidePlantUI();
    }

    public void DeselectCurrentActiveEmptyPlantpoint()        //Function for deselection the plantpoint
    {
        currentlyActivePlantPoint = null;
        HideBuildUI();
    }

    public void SetCurrentActivePlantPoint(Plantpoint plantPoint)
    {
        //Si el plantPoint tiene una planta se activa el plantpointUI con la informacion de la planta.
        currentlyActivePlantPoint = plantPoint;
        plantCanvasUI.transform.position = currentlyActivePlantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        plantCanvasUI.SetActive(true);
    }

    public void SetActiveBuildpoint(Plantpoint plantPoint)
    {
        //Cuadno el plant poin esta vacio para sacar el buildUI
        currentlyActivePlantPoint = plantPoint;
        buildCanvasUI.transform.position = currentlyActivePlantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        buildCanvasUI.SetActive(true);
    }

    public void HideBuildUI()
    {
        buildCanvasUI.SetActive(false);
    }

    public void HidePlantUI()
    {
        plantCanvasUI.SetActive(false);
    }

    //Esta parte del script esta dedicada a las funiones de la UI de las plantas. Tanto para la UI de la
    //planta como el UI de construccion.
    public void PurchasePlantForCurrentActivePlantpoint(PlantBlueprint bluePrint)
    {
        if (LevelManager.Instance.UiManager.CurrentMoney >= bluePrint.price)
        {
            currentlyActivePlantPoint.BuildPlant(bluePrint);
            //SoundHandler.Instance.PlayClip(uiSounds[0]);
            //SoundHandler.Instance.PlayClip(uiSounds[1]);
            DeselectCurrentActiveEmptyPlantpoint();
        }
        else return;
    }
    public void UpgradeCurrentActivePlantInPlantpoint()
    {
        if (LevelManager.Instance.UiManager.CurrentMoney > currentlyActivePlantPoint.currentBlueprint.upgradePrice)
        {
            currentlyActivePlantPoint.UpgradePlant();
            //SoundHandler.Instance.PlayClip(uiSounds[0]);
            DeselectCurrentActivePlantpointWithPlant();
        }
        else return;
    }
    public void SellPlantInCurrentActivePlantpoint()
    {
        currentlyActivePlantPoint.SellPlant();
        //SoundHandler.Instance.PlayClip(uiSounds[0]);
        DeselectCurrentActivePlantpointWithPlant();
    }
}
