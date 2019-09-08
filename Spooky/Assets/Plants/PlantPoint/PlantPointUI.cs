using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPointUI : MonoBehaviour
{
    public delegate void PlantUIEvent(Plantpoint plantPoint, PlantBlueprint blueprint);
    
    [SerializeField]
    protected Plantpoint currentPlantPoint = null;

    public void SetCurrentPlantPoint(Plantpoint point)
    {
        currentPlantPoint = point;
    }

    public void ActivateUI(bool _active)
    {
        if (gameObject.activeSelf != _active)
        {
            gameObject.SetActive(_active); 
        }
    }
}
