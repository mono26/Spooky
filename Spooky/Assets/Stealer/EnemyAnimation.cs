using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : IAnimable
{
    private SpriteRenderer sprite;
    private Animator animator;

    public SpriteRenderer Sprite { get { return sprite; } }
    public Animator Animator { get { return animator; } }

    public EnemyAnimation(SpriteRenderer _sprite, Animator _animator)
    {
        sprite = _sprite;
        animator = _animator;
    }

    public void CheckViewDirection(Vector3 nextPosition)
    {
        // With this we check the hypotetic newPosition of spooky so we can know if its going left or right
        Vector3 nexPosition = sprite.transform.position + nextPosition;
        if (sprite.transform.position.x < nextPosition.x)
        {
            sprite.flipX = false;
        }
        else if (sprite.transform.position.x > nextPosition.x)
        {
            sprite.flipX = true;
        }

        IsMoving(nexPosition);
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

    public void PlayAnimation(string _animation)
    {
        animator.SetTrigger(_animation);
    }
}
