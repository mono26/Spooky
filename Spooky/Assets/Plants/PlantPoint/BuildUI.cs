using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : PlantPointUI
{
    public static event PlantUIEvent BuildPlantEvent;

    public void RequestBuild(PlantBlueprint blueprintToBuild)
    {
        BuildPlantEvent?.Invoke(currentPlantPoint, blueprintToBuild);
    }
}
