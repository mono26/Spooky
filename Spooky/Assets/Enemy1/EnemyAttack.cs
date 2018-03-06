using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : ScriptableObject
{
    [SerializeField]
    public float coolDown;
    public float range;

    public virtual void Execute(Enemy _enemy)
    {

    }
}
