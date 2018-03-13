using UnityEngine;

public class PlantAnimation : IAnimable
{
    private SpriteRenderer sprite;
    private Animator animator;

    public SpriteRenderer Sprite { get { return sprite; } }
    public Animator Animator { get { return animator; } }

    public PlantAnimation(SpriteRenderer _sprite, Animator _animator)
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

        return;
    }

    public void PlayAnimation(string _animation)
    {
        animator.SetTrigger(_animation);
    }
}
