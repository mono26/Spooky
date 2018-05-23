using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get { return maxHealth; } }
    [SerializeField]
    private float attacksPerSecond;
    public float AttacksPerSecond { get { return attacksPerSecond; } }
}
