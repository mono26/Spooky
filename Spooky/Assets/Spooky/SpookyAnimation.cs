using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyAnimation : IAnimationHandler
{
    private SpriteRenderer sprite;
    private SpriteRenderer handSprite;
    private Animator animator;

    public SpriteRenderer Sprite { get { return sprite; } }
    public Animator Animator { get { return animator; } }

    public SpookyAnimation(SpriteRenderer _sprite, SpriteRenderer _handSprite, Animator _animator)
    {
        sprite = _sprite;
        handSprite = _handSprite;
        animator = _animator;
    }

    public void CheckViewDirection(Vector3 nextPosition)
    {
        // With this we check the hypotetic newPosition of spooky so we can know if its going left or right
        Vector3 nexPosition = sprite.transform.position + nextPosition;
        if (sprite.transform.position.x < nextPosition.x)
        {
            sprite.flipX = false;
            handSprite.flipX = false;
        }
        else if (sprite.transform.position.x > nextPosition.x)
        {
            sprite.flipX = true;
            handSprite.flipX = true;
        }
        IsMoving(nexPosition);
        var time = Time.timeSinceLevelLoad;
        Debug.Log(string.Format("{0} velocidad {1}", time, nexPosition));
        return;
    }

    public bool IsMoving(Vector3 _velocity)
    {

        if (_velocity.x.Equals(0f))
        {
            animator.SetInteger("Velocity", 0);
            return false;
        }
        else
        {
            animator.SetInteger("Velocity", (int)_velocity.x);
            return true;
        }

    }

    public void StopMoving()
    {
        animator.SetBool("Moving", false);
    }
}
