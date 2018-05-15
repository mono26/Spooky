using System;
using UnityEngine;

// Constrained to only accept enum types.
[Serializable]
public class StateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
{
    [SerializeField]
    public T currentState;
    public T CurrentState { get { return currentState; } }

    public virtual void ChangeState(T _newState)
    {
        currentState = _newState;
    }
}
