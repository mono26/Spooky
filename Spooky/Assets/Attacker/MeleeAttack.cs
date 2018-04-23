using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : CloseRangeAttack
{
    protected Enemy owner;
    protected Collider meleeCollider;

    public MeleeAttack(Enemy _owner, Collider _meleeCollider)
    {
        owner = _owner;
        meleeCollider = _meleeCollider;
    }

    public void CloseAttack()
    {
        owner.CastAbility(owner.StartCoroutine(StealCrop()));
        return;
    }

    private IEnumerator StealCrop()
    {
        if (owner.Target != null)        //Si no tiene target no deberia de disparar.
        {
            //Debe de ir el resto del metodo de la planta para el melee
            //DDeberia activar la ejecucion de la aniacion y esperar a que pase pare activar.
            owner.AnimationComponent.PlayAnimation("MeleeAttack");
            LocateCollider();

            yield return new WaitForSecondsRealtime(
                owner.AnimationComponent.Animator.GetCurrentAnimatorStateInfo(0).length);

            meleeCollider.gameObject.SetActive(true);

            /// This is a magic number to make sue the collider stays active enough time so the player can collide with it.
            yield return new WaitForSeconds(0.3f);

            meleeCollider.gameObject.SetActive(false);
        }
        else yield return false;
    }

    private void LocateCollider()
    {
        Vector3 direccion = owner.Target.position - owner.transform.position;
        direccion.y = 0f;
        direccion = direccion.normalized;
        meleeCollider.transform.position = owner.transform.position + (direccion * owner.StatsComponent.basicRange);
    }
}
