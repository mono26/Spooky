using UnityEngine;

public class SpookyAnimation : IAnimable
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

    public void CheckViewDirection(Vector3 _nextPosition)
    {
        // With this we check the hypotetic newPosition of spooky so we can know if its going left or right
        Vector3 nextPosition = sprite.transform.position + _nextPosition;
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
        IsMoving(nextPosition);
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
}
