using UnityEngine;
using UnityEngine.EventSystems;

public class MovingJoystick : StaticJoystick, IPointerDownHandler
{
    public bool RestorePosition = true;

    protected Vector3 initialJoystickPosition;
    protected Vector3 newJoystickPositon;

    protected override void Start()
    {
        base.Start();

        initialJoystickPosition = GetComponent<RectTransform>().transform.position;
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        newJoystickPositon = Camera.main.ScreenToWorldPoint(data.position);

        newJoystickPositon.y = transform.position.y;

        SetNeutralPosition(newJoystickPositon);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (RestorePosition)
        {
            GetComponent<RectTransform>().transform.position = initialJoystickPosition;
        }
    }
}
