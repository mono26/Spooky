using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[System.Serializable]
public class JoystickEvent : UnityEvent<Vector2> { }

[RequireComponent(typeof(Rect))]
[RequireComponent(typeof(CanvasGroup))]
public class TouchJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Camera")]
    [SerializeField]
    protected Camera TargetCamera;

    [Header("Axis")]
    [SerializeField]
    protected float MaxRange = 1.5f;

    [Header("Binding")]
    [SerializeField]
    protected JoystickEvent JoystickValue;

    public RenderMode ParentCanvasRenderMode { get; protected set; }

    [SerializeField]
    protected RectTransform joystick;
    [SerializeField]
    protected RectTransform handle;

    protected Vector2 joystickValue;
    protected Vector3 newTargetPosition;
    protected Vector3 newJoystickPosition;
    protected float initialYPosition;

    protected virtual void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        if (TargetCamera == null)
        {
            throw new Exception("TouchJoystick : you have to set a target camera");
        }
        ParentCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
        initialYPosition = transform.position.y;
    }

    protected virtual void OnEnable()
    {
        Initialize();
        return;
    }

    protected virtual void Update()
    {
        if (JoystickValue != null)
        {
            JoystickValue.Invoke(joystickValue);
        }
        return;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
        {
            newTargetPosition = TargetCamera.ScreenToWorldPoint(eventData.position);
        }
        else
        {
            newTargetPosition = eventData.position;
        }

        newTargetPosition = Vector3.ClampMagnitude(newTargetPosition - joystick.position, MaxRange);

        joystickValue.x = EvaluateInputValue(newTargetPosition.x);
        joystickValue.y = EvaluateInputValue(newTargetPosition.z);

        newJoystickPosition = joystick.position + newTargetPosition;
        newJoystickPosition.y = initialYPosition;

        transform.position = newJoystickPosition;

        return;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        newJoystickPosition = joystick.position;
        newJoystickPosition.y = initialYPosition;
        transform.position = newJoystickPosition;
        joystickValue.x = 0f;
        joystickValue.y = 0f;
        return;
    }

    protected virtual float EvaluateInputValue(float vectorPosition)
    {
        return Mathf.InverseLerp(0, MaxRange, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
    }
}
