using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantUI : PlantPointUI
{
    public static event PlantUIEvent UpgradePlantEvent;
    public static event PlantUIEvent SellPlantEvent;

    public void RequestSell()
    {
        SellPlantEvent?.Invoke(currentPlantPoint, currentPlantPoint.CurrentBlueprint);
    }

    public void RequestUpgrade()
    {
        UpgradePlantEvent?.Invoke(currentPlantPoint, currentPlantPoint.CurrentBlueprint);
    }
}
