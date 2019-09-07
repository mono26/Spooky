using UnityEngine;

public enum PoolableStates
{
    InsidePool, OutOfPool
}

public class PoolableObject : MonoBehaviour
{
    public delegate void OnSpawnCompleteDelegate();
    public event OnSpawnCompleteDelegate ExitPoolEvent;
    public delegate void OnReleaseDelegate();
    public event OnReleaseDelegate EnterPoolEvent;

    Transform objectTransform = null;

    public string GetName { get { return gameObject.name; } }
    public Vector3 SetObjectPosition { set { transform.position = value; } }

    protected virtual void Awake() 
    {
        if (!objectTransform)
        {
            objectTransform = transform;
        }
    }

    public void SetPoolableState(PoolableStates newState)
    {
        if (newState.Equals(PoolableStates.InsidePool))
        {
            InitializeInsideOfPoolState();
        }
        else
        {
            InitializeOutOfPoolState();
        }
    }

    /// <summary>
    /// Should be used for setting the object specific state to sleep.
    /// Stoping code or routines execution.
    /// </summary>
    void InitializeInsideOfPoolState()
    {
        if (EnterPoolEvent != null)
        {
            EnterPoolEvent();
        }
    }

    /// <summary>
    /// Should be used for setting the object specific state to active.
    /// Resuming code or routines execution.
    /// </summary>
    void InitializeOutOfPoolState()
    {
        if (ExitPoolEvent != null)
        {
            ExitPoolEvent();
        }
    }
}
