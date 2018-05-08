using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StaticJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [System.Serializable]
    public class JoystickEvent : UnityEvent<Vector2> { }

    public float maxRangeOfInput;

    public JoystickEvent joystickInput;

    protected Vector3 knobInitialPosition;
    protected Vector3 knobTargetPosition;
    protected Vector3 newKnobPosition;

    // Constant and it depends on the camera value.
    protected float joystickYPosition;

    [SerializeField]
    protected Vector2 joystickValue;

    // Use this for initialization
    protected virtual void Start()
    {
        Initialize();
        return;
    }

    public virtual void Initialize()
    {
        SetNeutralPosition();

        // Is the y component of the position because of the camera rotation.
        joystickYPosition = transform.position.y;
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

    public virtual void SetNeutralPosition()
    {
        knobInitialPosition = GetComponent<RectTransform>().transform.position;
        return;
    }

    public virtual void SetNeutralPosition(Vector3 newPosition)
    {
        knobInitialPosition = newPosition;
        return;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Getting the position of the toucj inside the camera plane.
        knobTargetPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        // Converting to localposition.
        //knobTargetLocalPosition = GetComponent<RectTransform>().InverseTransformPoint(knobTargetLocalPosition);

        knobTargetPosition = Vector3.ClampMagnitude(knobTargetPosition - knobInitialPosition, maxRangeOfInput);

        joystickValue.x = EvaluateInputValue(knobTargetPosition.x);
        joystickValue.y = EvaluateInputValue(knobTargetPosition.z);

        newKnobPosition = knobInitialPosition + knobTargetPosition;
        newKnobPosition.y = joystickYPosition;

        transform.position = newKnobPosition;

        return;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        newKnobPosition = knobInitialPosition;
        newKnobPosition.y = joystickYPosition;
        transform.position = newKnobPosition;
        joystickValue.x = 0f;
        joystickValue.y = 0f;

        return;
    }

    protected virtual float EvaluateInputValue(float vectorPosition)
    {
        return Mathf.InverseLerp(0, maxRangeOfInput, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
    }
}
