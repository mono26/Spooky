using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StaticJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [System.Serializable]
    public class JoystickEvent : UnityEvent<Vector2> { }

    [Header("Options")]
    [SerializeField]
    [Range(0f, 2f)] protected float maxRangeOfInput = 1.0f;

    [Header("Components")]
    [SerializeField]
    protected RectTransform background;
    [SerializeField]
    protected RectTransform handle;

    [SerializeField]
    protected JoystickEvent joystickInput;

    protected Vector2 knobTargetPosition;
    protected Vector2 newKnobPosition;
    protected Vector2 joystickPosition;

    [SerializeField]
    protected Vector2 joystickValue;

    protected virtual void Awake()
    {
        background = GetComponentInParent<RectTransform>();
        handle = GetComponent<RectTransform>();
        return;
    }

    protected virtual void Start()
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, background.position);
        return;
    }
	
	void Update ()
    {
        if (joystickInput != null)
        {
            joystickInput.Invoke(joystickValue);
            return;
        }
        else return;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Getting the position of the toucj inside the camera plane.
        knobTargetPosition = eventData.position - joystickPosition;
        // Converting to localposition.
        //knobTargetLocalPosition = GetComponent<RectTransform>().InverseTransformPoint(knobTargetLocalPosition);

        knobTargetPosition = Vector3.ClampMagnitude(knobTargetPosition - joystickPosition, maxRangeOfInput);

        joystickValue.x = EvaluateInputValue(knobTargetPosition.x);
        joystickValue.y = EvaluateInputValue(knobTargetPosition.y);

        newKnobPosition = (knobTargetPosition * background.sizeDelta.x / 2f) * maxRangeOfInput;

        handle.anchoredPosition = newKnobPosition;

        return;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        joystickValue.x = 0f;
        joystickValue.y = 0f;

        return;
    }

    protected virtual float EvaluateInputValue(float vectorPosition)
    {
        return Mathf.InverseLerp(0, maxRangeOfInput, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
    }
}
