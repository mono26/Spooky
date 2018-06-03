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

    private Plantpoint currentActivePlantPoint;
    public Plantpoint CurrentPlantPoint { get { return currentActivePlantPoint; } }

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
        currentActivePlantPoint = plantPoint;
        SetCurrentActivePlantPoint(currentActivePlantPoint);
        return;
    }

    public void ActivateBuildUI(Plantpoint plantPoint)     //Metodo que se llamara cada vez que el jugador haga click sobre un plant point.
    {
        DeselectCurrentActivePlantpointWithPlant();
        currentActivePlantPoint = plantPoint;
        SetCurrentActiveBuildpoint(currentActivePlantPoint);
        return;
    }

    public void DeselectCurrentActivePlantpointWithPlant()        //Function for deselection the plantpoint
    {
        currentActivePlantPoint = null;
        HidePlantUI();
    }

    public void DeselectCurrentActiveEmptyPlantpoint()        //Function for deselection the plantpoint
    {
        currentActivePlantPoint = null;
        HideBuildUI();
    }

    private void SetCurrentActivePlantPoint(Plantpoint plantPoint)
    {
        //Si el plantPoint tiene una planta se activa el plantpointUI con la informacion de la planta.
        currentActivePlantPoint = plantPoint;
        plantCanvasUI.transform.position = currentActivePlantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        plantCanvasUI.SetActive(true);
    }

    private void SetCurrentActiveBuildpoint(Plantpoint plantPoint)
    {
        //Cuadno el plant poin esta vacio para sacar el buildUI
        currentActivePlantPoint = plantPoint;
        buildCanvasUI.transform.position = currentActivePlantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        buildCanvasUI.SetActive(true);
    }

    private void HideBuildUI()
    {
        buildCanvasUI.SetActive(false);
    }

    private void HidePlantUI()
    {
        plantCanvasUI.SetActive(false);
    }

    //Esta parte del script esta dedicada a las funiones de la UI de las plantas. Tanto para la UI de la
    //planta como el UI de construccion.
    public void PurchasePlantForCurrentActivePlantpoint(PlantBlueprint bluePrint)
    {
        if (LevelManager.Instance.CurrentMoney >= bluePrint.price)
        {
            currentActivePlantPoint.BuildPlant(bluePrint);
            //SoundHandler.Instance.PlayClip(uiSounds[0]);
            //SoundHandler.Instance.PlayClip(uiSounds[1]);
            HideBuildUI();
            SetCurrentActivePlantPoint(currentActivePlantPoint);
        }
        else return;
    }
    public void UpgradeCurrentActivePlantInPlantpoint()
    {
        if (LevelManager.Instance.CurrentMoney > currentActivePlantPoint.CurrentBlueprint.upgradePrice)
        {
            currentActivePlantPoint.UpgradePlant();
            //SoundHandler.Instance.PlayClip(uiSounds[0]);
        }
        else return;
    }
    public void SellPlantInCurrentActivePlantpoint()
    {
        currentActivePlantPoint.SellPlant();
        //SoundHandler.Instance.PlayClip(uiSounds[0]);
        DeselectCurrentActivePlantpointWithPlant();
    }
}
