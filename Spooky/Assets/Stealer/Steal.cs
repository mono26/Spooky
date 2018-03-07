using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Enemy/Steal")]
public class Steal : EnemyAttack
{
    public override void Execute(Enemy _enemy)
    {
        _enemy.ability = _enemy.StartCoroutine(StealCrop());
        return;
    }

    private IEnumerator StealCrop()
    {
        //TODO cast steal
        yield return 0;
    }
}
