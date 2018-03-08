using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantBasicAttack : IRangeAttack
{
    protected Plant owner;

    public Bullet bulletPrefab;
    public float launchForce = 1;
    public Transform shootPosition;

    public PlantBasicAttack(Plant _owner, Bullet _bullet, float _launchforce, Transform _shootPosition)
    {
        owner = _owner;
        bulletPrefab = _bullet;
        launchForce = _launchforce;
        shootPosition = _shootPosition;
    }

    public void RangeAttack()
    {
        owner.CastAbility(owner.StartCoroutine(BasicAtack(owner.enemyDirection)));
        return;
    }

    private IEnumerator BasicAtack(Vector3 _direction)
    {
        //controller.animationHandler.Ability1Animation();

            //yield return new WaitForSecondsRealtime(
                //animationHandler.animator.GetCurrentAnimatorStateInfo(0).length
                //);

        yield return owner.StartCoroutine(ThrowBasicAtack(_direction)); ;

    }
    private IEnumerator ThrowBasicAtack(Vector3 _direction)
    {
        GameObject tempBullet = GameObject.Instantiate(
            bulletPrefab.gameObject, 
            shootPosition.position, 
            shootPosition.rotation
            );

        RotateBullettowardsDirection(tempBullet.transform, _direction);

        yield return 0;

        var bulletComponent = tempBullet.GetComponent<Bullet>();
        bulletComponent.Launch(launchForce);
        owner.timeSinceLastShoot = Time.timeSinceLevelLoad;
    }

    private void RotateBullettowardsDirection(Transform _bullet, Vector3 _direction)
    {
        var angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
        var delta = angle - _bullet.rotation.z;
        //TODO check if transform.Rotate
        _bullet.localRotation = Quaternion.RotateTowards(_bullet.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), Mathf.Abs(delta));
        return;
    }
}
