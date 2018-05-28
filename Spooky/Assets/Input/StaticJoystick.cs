using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StaticJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [System.Serializable]
    public class JoystickEvent : UnityEvent<Vector2> { }

    [Header("Options")]
    [SerializeField]
    [Range(0f, 2f)] protected float maxRangeOfInput = 1.0f;

    [Header("Components")]
    [SerializeField]
    protected Image background;
    [SerializeField]
    protected Image handle;

    [SerializeField]
    protected JoystickEvent joystickInput;

    protected Vector2 handleTargetPosition;
    protected Vector2 newKnobPosition;
    protected Vector2 joystickPosition;

    [SerializeField]
    protected Vector2 joystickValue;

    protected virtual void Awake()
    {
        background = GetComponent<Image>();
        handle = transform.GetChild(0).GetComponent<Image>(); //this command is used because there is only one child in hierarchy
        joystickValue = Vector3.zero;
        return;
    }

    protected virtual void Start()
    {
        /*joystickPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, background.position);
        return;*/
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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        handleTargetPosition = Vector2.zero;

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (background.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out newKnobPosition);

        Debug.Log("click position" + eventData.position);
        Debug.Log("position in joystick" + newKnobPosition);

        handleTargetPosition.x = (handleTargetPosition.x / background.rectTransform.sizeDelta.x);
        handleTargetPosition.y = (handleTargetPosition.y / background.rectTransform.sizeDelta.y);

        float x = (background.rectTransform.pivot.x == 1f) ? handleTargetPosition.x * 2 + 1 : handleTargetPosition.x * 2 - 1;
        float y = (background.rectTransform.pivot.y == 1f) ? handleTargetPosition.y * 2 + 1 : handleTargetPosition.y * 2 - 1;

        joystickValue = new Vector2(x, y);
        joystickValue = (joystickValue.magnitude > 1) ? joystickValue.normalized : joystickValue;

        //to define the area in which joystick can move around
        handle.rectTransform.anchoredPosition = new Vector3(joystickValue.x * (background.rectTransform.sizeDelta.x / 2)
                                                               , joystickValue.y * (background.rectTransform.sizeDelta.y) / 2);

        /*
        // Getting the position of the toucj inside the camera plane.
        knobTargetPosition = eventData.position - joystickPosition;

        knobTargetPosition = Vector3.ClampMagnitude(knobTargetPosition - joystickPosition, maxRangeOfInput);

        joystickValue.x = EvaluateInputValue(knobTargetPosition.x);
        joystickValue.y = EvaluateInputValue(knobTargetPosition.y);

        newKnobPosition = (knobTargetPosition * background.sizeDelta.x / 2f) * maxRangeOfInput;

        handle.anchoredPosition = newKnobPosition;*/

        return;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndDrag(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        joystickValue = Vector2.zero;
        handle.rectTransform.anchoredPosition = Vector3.zero;
        return;
    }

    protected virtual float EvaluateInputValue(float vectorPosition)
    {
        return Mathf.InverseLerp(0, maxRangeOfInput, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
    }
}
