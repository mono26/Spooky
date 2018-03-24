using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlantLaunchProyectile : IRangeAttack
{
    protected Plant owner;

    public ISpawnable<Bullet> bulletPrefab;
    public float launchForce = 1;
    public Transform shootPosition;
    public AudioClip soundEffect;

    public PlantLaunchProyectile(Plant _owner, ISpawnable<Bullet> _bullet, float _launchforce, Transform _shootPosition, AudioClip _soundEffect)
    {
        owner = _owner;
        bulletPrefab = _bullet;
        launchForce = _launchforce;
        shootPosition = _shootPosition;
        soundEffect = _soundEffect;
    }

    public void RangeAttack()
    {
        owner.StartCast(true);
        owner.CastAbility(owner.StartCoroutine(BasicAtack()));
        return;
    }

    protected IEnumerator BasicAtack()
    {
        owner.animationComponent.PlayAnimation("Attack");

        yield return new WaitForSecondsRealtime(
            owner.animationComponent.Animator.GetCurrentAnimatorStateInfo(0).length //+
            //owner.animationComponent.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime
            );

        Bullet tempBullet = CreateBullet(bulletPrefab);
        ThrowBasicAtack(tempBullet, owner.enemyDetect.GetEnemyDirection());

    }

    protected Bullet CreateBullet(ISpawnable<Bullet> _bullet)
    {
        return _bullet.Spawn(owner.transform);
    }

    protected void ThrowBasicAtack(Bullet _bullet, Vector3 _direction)
    {
        RotateBullettowardsDirection(_bullet.transform, _direction);

        _bullet.Launch(launchForce);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        GameManager.Instance.SoundManager.PlayClip(soundEffect);
        owner.StartCast(false); //Private set of the variable; only by method and giving a false to end cast
    }

    protected void RotateBullettowardsDirection(Transform _bullet, Vector3 _direction)
    {
        var angle = Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
        var delta = angle - _bullet.rotation.z;
        //TODO check if transform.Rotate
        _bullet.localRotation = Quaternion.RotateTowards(_bullet.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), Mathf.Abs(delta));
        return;
    }
}
