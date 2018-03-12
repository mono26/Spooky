using UnityEngine;

public interface IAnimationHandler
{
    SpriteRenderer Sprite { get; }
    Animator Animator { get; }

    void CheckViewDirection(Vector3 nextPosition);
}
