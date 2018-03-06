using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : PlantAttack
{
    public override void Execute(Plant _plant)
    {
        _plant.ability = StartCoroutine(
            BasicAtack(_plant.transform.position, 
                _plant.enemyDirection)
                );
    }

    private IEnumerator BasicAtack(Vector3 _origin ,Vector3 _direction)
    {
        //controller.animationHandler.Ability1Animation();

            //yield return new WaitForSecondsRealtime(
                //animationHandler.animator.GetCurrentAnimatorStateInfo(0).length
                //);

        ThrowBasicAtack(_origin, _direction);

        yield return 0;
    }
    private void ThrowBasicAtack(Vector3 _origin, Vector3 _direction)
    {
        Debug.Log("Lanzando bala");
        //var tempBullet = PoolsManagerBullets.Instance.GetBullet(controller.plant.settings.BasicBullet.info.objectIndex).GetComponent<BulletController>();
        //tempBullet.timerHandler.SetBasicCoolDown(tempBullet.settings.CooldownHanlderSettings.BasicCooldown);
        //tempBullet.transform.position = controller.transform.position;
        //tempBullet.bullet.ShootBullet(direction * controller.plant.settings.objectForce);
        //SoundHandler.Instance.PlayClip(basicAtackSound);
    }
}
