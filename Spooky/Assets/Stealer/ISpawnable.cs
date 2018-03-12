using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable <T>

{
    List<T> Pool { get;}

    T Spawn(Transform _position);
}
