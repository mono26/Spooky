using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBullet : Bullet
{
    protected float corrosiveDamage;
    protected float corrosiveTickRate;
    protected float corrosiveDuration;

    protected IEnumerator CorrosiveEffect()
    {
        yield return 0;
    }
}
