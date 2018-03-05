using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : ScriptableObject
{
    [SerializeField]
    protected Spooky spooky;
    [SerializeField]
    protected float coolDown;

    public float range;

    public virtual void Execute()
    {

    }
}
