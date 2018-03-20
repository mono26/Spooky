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

    public void CheckViewDirection(Vector3 _nextPosition)
    {
        // With this we check the hypotetic newPosition of spooky so we can know if its going left or right
        Vector3 nextPosition = sprite.transform.position + _nextPosition;
        if (sprite.transform.position.x < nextPosition.x)
        {
            sprite.flipX = true;
        }
        else if (sprite.transform.position.x > nextPosition.x)
        {
            sprite.flipX = false;
        }
        return;
    }

    public void PlayAnimation(string _animation)
    {
        animator.SetTrigger(_animation);
    }
}
