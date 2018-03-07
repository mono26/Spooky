using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantBasicAttack : IRangeAttack
{
    public Plant owner;
    public Bullet bullet;
    public float attackDelay;    // Can be the attack speed of the object. Plant or enemy?
    public float timeSinceLastShoot;
    public float launchForce;
    public Transform shootPosition;

    public PlantBasicAttack(Plant _owner, float _attackDelay)
    {
        owner = _owner;
        attackDelay = _attackDelay;
    }

    public void RangeAttack()
    {
        if (Time.timeSinceLevelLoad > timeSinceLastShoot + owner.settings.AttackSpeed)
            owner.ability = owner.StartCoroutine(BasicAtack(owner.enemyDirection));
    }

    private IEnumerator BasicAtack(Vector3 _direction)
    {
        //controller.animationHandler.Ability1Animation();

            //yield return new WaitForSecondsRealtime(
                //animationHandler.animator.GetCurrentAnimatorStateInfo(0).length
                //);

        ThrowBasicAtack(_direction);

        yield return 0;
    }
    private void ThrowBasicAtack(Vector3 _direction)
    {
        Debug.Log("Lanzando bala");
        GameObject tempBullet = GameObject.Instantiate(bullet.gameObject, shootPosition.position, shootPosition.rotation);
        var bulletComponent = tempBullet.GetComponent<Bullet>();
        bulletComponent.Launch(launchForce);
    }
}
