using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlantLaunchProyectile : IRangeAttack
{
    protected Plant plant;

    public ISpawnable<Bullet> bulletPrefab;
    public float launchForce = 1;
    public Transform shootPosition;
    public AudioClip soundEffect;

    public PlantLaunchProyectile(Plant _owner, ISpawnable<Bullet> _bullet, float _launchforce, Transform _shootPosition, AudioClip _soundEffect)
    {
        plant = _owner;
        bulletPrefab = _bullet;
        launchForce = _launchforce;
        shootPosition = _shootPosition;
        soundEffect = _soundEffect;
        return;
    }

    public void RangeAttack()
    {
        plant.StartCastingAbility(true);
        plant.CastAbility(plant.StartCoroutine(BasicAtack()));
        return;
    }

    protected IEnumerator BasicAtack()
    {
        plant.animationComponent.PlayAnimation("Attack");

        yield return new WaitForSecondsRealtime(
            plant.animationComponent.Animator.GetCurrentAnimatorStateInfo(0).length
            );

        Bullet tempBullet = CreateBullet(bulletPrefab);
        ThrowBasicAtack(tempBullet, plant.enemyDetect.GetEnemyDirection());

        yield return 0;
    }

    protected Bullet CreateBullet(ISpawnable<Bullet> _bullet)
    {
        return _bullet.Spawn(plant.transform);
    }

    protected void ThrowBasicAtack(Bullet _bullet, Vector3 _direction)
    {
        RotateBullettowardsDirection(_bullet.transform, _direction);

        _bullet.Launch(launchForce);
        GameManager.Instance.SoundManager.PlayClip(soundEffect);
        plant.StartCastingAbility(false); //Private set of the variable; only by method and giving a false to end cast
        return;
    }

    protected void RotateBullettowardsDirection(Transform _bullet, Vector3 _direction)
    {
        var angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
        var delta = angle - _bullet.localRotation.eulerAngles.z;
        //TODO check if transform.Rotate
        _bullet.localRotation = Quaternion.RotateTowards(
            _bullet.localRotation,
            Quaternion.Euler(new Vector3(90, 0, angle)),
            Mathf.Abs(delta)
            );
        return;
    }
}
