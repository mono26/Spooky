using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plants/PlantBluePrint")]
//Asset para poder almacenar el precio y el prefab de la torre
public class PlantBlueprint : ScriptableObject
{
    public int price;
    public int upgradePrice;
    public Plant plant;
    public Plant upgradePlant;
}
