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

#region Unity Functions
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

    
    private void Start() 
    {
        Plantpoint.PlantPointClickedEvent += ActivatePlantPointUI;
    }

    private void OnDestroy()
    {
        Plantpoint.PlantPointClickedEvent -= ActivatePlantPointUI;
    }
#endregion

    private void OnPlantPointCLicked(Plantpoint target)
    {
        if (currentActivePlantPoint)
        {
            DeselectCurrentActivePlantPoint();
        }
        ActivatePlantPointUI(target);
    }

    private void ActivatePlantPointUI(Plantpoint target)
    {
        if (target.IsEmpty())
        {
            ActivateBuildUI(target);
        }
        else
        {
            ActivatePlantUI(target);
        }
    }

    //Metodos para el manejo de los plantpoints y la UI
    private void ActivatePlantUI(Plantpoint plantPoint)
    {
        currentActivePlantPoint = plantPoint;
        SetCurrentActivePlantPoint(currentActivePlantPoint);
        return;
    }

    private void ActivateBuildUI(Plantpoint plantPoint)
    {
        currentActivePlantPoint = plantPoint;
        SetCurrentActiveBuildpoint(currentActivePlantPoint);
        return;
    }

    private void DeselectCurrentActivePlantPoint()
    {
        if (currentActivePlantPoint.IsEmpty())
        {
            DeselectCurrentActiveEmptyPlantpoint();
        }
        else
        {
            DeselectCurrentActivePlantpointWithPlant();
        }
    }

    private void DeselectCurrentActivePlantpointWithPlant()        //Function for deselection the plantpoint
    {
        currentActivePlantPoint = null;
        HidePlantUI();
    }

    private void DeselectCurrentActiveEmptyPlantpoint()        //Function for deselection the plantpoint
    {
        currentActivePlantPoint = null;
        HideBuildUI();
    }

    private void SetCurrentActivePlantPoint(Plantpoint plantPoint)
    {
        currentActivePlantPoint = plantPoint;
        plantCanvasUI.transform.position = currentActivePlantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        plantCanvasUI.SetActive(true);
    }

    private void SetCurrentActiveBuildpoint(Plantpoint plantPoint)
    {
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
            HideBuildUI();
            SetCurrentActivePlantPoint(currentActivePlantPoint);
        }
    }
    public void UpgradeCurrentActivePlantInPlantpoint()
    {
        if (LevelManager.Instance.CurrentMoney > currentActivePlantPoint.CurrentBlueprint.upgradePrice)
        {
            currentActivePlantPoint.UpgradePlant();
        }
    }
    public void SellPlantInCurrentActivePlantpoint()
    {
        currentActivePlantPoint.SellPlant();
    }
}
