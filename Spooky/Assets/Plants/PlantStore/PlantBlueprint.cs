using UnityEngine;

[CreateAssetMenu(menuName = "Plants/PlantBluePrint")]
//Asset para poder almacenar el precio y el prefab de la torre
public class PlantBlueprint : ScriptableObject
{
    public int price;
    public int upgradePrice;
    public Character plant;
    public Character upgradePlant;
}
