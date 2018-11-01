using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
{
    public enum ButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp, Disabled }

    [SerializeField]
    protected UnityEvent ButtonPressedFirstTime;
    [SerializeField]
    protected UnityEvent ButtonPressed;
    [SerializeField]
    protected UnityEvent ButtonReleased;

    public ButtonStates CurrentState { get; protected set; }
	
	void Update ()
    {
        if(CurrentState == ButtonStates.ButtonPressed)
            OnPointerPressed();
    }

    protected virtual void LateUpdate()
    {
        if (CurrentState == ButtonStates.ButtonUp)
        {
            CurrentState = ButtonStates.Off;
        }
        if (CurrentState == ButtonStates.ButtonDown)
        {
            CurrentState = ButtonStates.ButtonPressed;
        }
    }

    public virtual void OnPointerPressed()
    {
        CurrentState = ButtonStates.ButtonPressed;
        if (ButtonPressed != null)
        {
            ButtonPressed.Invoke();
        }
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        if (CurrentState != ButtonStates.Off)
        {
            return;
        }
        CurrentState = ButtonStates.ButtonDown;
        if (ButtonPressedFirstTime != null)
        {
            ButtonPressedFirstTime.Invoke();
        }
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        if (CurrentState != ButtonStates.ButtonPressed || CurrentState != ButtonStates.ButtonDown)
        {
            return;
        }

        CurrentState = ButtonStates.ButtonUp;
        if (ButtonReleased != null)
        {
            ButtonReleased.Invoke();
        }
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        if (ButtonPressedFirstTime != null)
        {
            ButtonPressedFirstTime.Invoke();
        }
        if (ButtonReleased != null)
        {
            ButtonReleased.Invoke();
        }
    }

    public virtual void DisableButton()
    {
        CurrentState = ButtonStates.Disabled;
    }

    public virtual void EnableButton()
    {
        if (CurrentState == ButtonStates.Disabled)
        {
            CurrentState = ButtonStates.Off;
        }
    }
}
