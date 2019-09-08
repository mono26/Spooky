using UnityEngine;

public class PlantStore : MonoBehaviour
{
    //Singleton part
    private static PlantStore instance;
    public static PlantStore Instance { get { return instance; } }

#region Unity Functions
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    //Parte del singleton en donde se asigna la unica instancia de la clase
        }
        else
            Destroy(gameObject);
    }

    
    private void Start() 
    {
        Plantpoint.PlantPointClickedEvent += ActivatePlantPointUI;
        Plantpoint.BuildPlantEvent += BuyPlantForPlantPoint;
        Plantpoint.SellPlantEvent += SellPlantInPlantPoint;
        Plantpoint.UpgradePlantEvent += UpgradePlantInPlantPoint;
    }

    private void OnDestroy()
    {
        Plantpoint.PlantPointClickedEvent -= ActivatePlantPointUI;
    }
#endregion

    private void OnPlantPointCLicked(Plantpoint target)
    {
        DeselectCurrentPlantPoint();
        ActivatePlantPointUI(target);
    }

    public void ActivatePlantPointUI(Plantpoint target)
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
        LevelUIManager.Instance.ActivatePlantUI(plantPoint);
    }

    private void ActivateBuildUI(Plantpoint plantPoint)
    {
        LevelUIManager.Instance.ActivateBuildUI(plantPoint);
    }

    private void DeselectCurrentPlantPoint()
    {
        DeselectEmptyPlantpoint();
        DeselectPlantpointWithPlant();
    }

    private void DeselectPlantpointWithPlant()        //Function for deselection the plantpoint
    {
        LevelUIManager.Instance.ActivatePlantUI(false);
    }

    private void DeselectEmptyPlantpoint()        //Function for deselection the plantpoint
    {
        LevelUIManager.Instance.ActivateBuildUI(false);
    }

    public void BuyPlantForPlantPoint(Plantpoint point, PlantBlueprint blueprint)
    {
        if(blueprint.plant != null)
        {
            LevelManager.Instance.TakeMoney(blueprint.price);
            LevelManager.Instance.UpdateMoneyDisplay();
            point.PlantPlant(blueprint);
            DeselectCurrentPlantPoint();
            LevelUIManager.Instance.ActivatePlantUI(point);
        }
    }

    public void SellPlantInPlantPoint(Plantpoint point, PlantBlueprint blueprint)
    {
        if (!point.IsUpgraded)
        {
            LevelManager.Instance.GiveMoney(blueprint.price);
        }
        else
        {
            LevelManager.Instance.GiveMoney(blueprint.upgradePrice);
        }
        LevelManager.Instance.UpdateMoneyDisplay();
        point.RemovePlant();
        DeselectCurrentPlantPoint();
    }

    public void UpgradePlantInPlantPoint(Plantpoint point, PlantBlueprint blueprint)
    {
        if(blueprint.upgradePlant != null)
        {
            LevelManager.Instance.TakeMoney(blueprint.upgradePrice);
            LevelManager.Instance.UpdateMoneyDisplay();
            point.UpgradePlant();
            DeselectCurrentPlantPoint();
        }
    }
}
