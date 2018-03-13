using UnityEngine;

public interface IAnimable
{
    SpriteRenderer Sprite { get; }
    Animator Animator { get; }

    void CheckViewDirection(Vector3 nextPosition);
}
