using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steal : EnemyAttack
{
    public Coroutine steal;

    public override void Execute()
    {
        steal = spooky.StartCoroutine(StealCrop());
    }

    private IEnumerator StealCrop()
    {
        //TODO cast steal
        yield return 0;
    }

    public bool IsStealFinish()
    {
        if (steal != null)
        {
            return false;
        }
        else return true;
    }
}
