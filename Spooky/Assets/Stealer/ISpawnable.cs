using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable <T>

{
    List<T> Pool { get;}
    Transform PoolPosition { get; }

    T Spawn(Transform _position);
}
