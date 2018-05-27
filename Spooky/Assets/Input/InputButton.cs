public class InputButton
{
    public enum ButtonStates { Off, Down, Pressed, Up }

    public string ButtonID { get; protected set; }
    public ButtonStates CurrentState { get; protected set; }

    public delegate void DownMethodDelegate();
    public delegate void PressedMethodDelegate();
    public delegate void UpMethodDelegate();

    public DownMethodDelegate downMethod;
    public PressedMethodDelegate pressedMethod;
    public UpMethodDelegate upMethod;

    public InputButton(string _buttonID, DownMethodDelegate btnDown, PressedMethodDelegate btnPressed, UpMethodDelegate btnUp)
    {
        ButtonID = _buttonID;
        downMethod = btnDown;
        pressedMethod = btnPressed;
        upMethod = btnUp;
    }

    public void ChangeButtonState(ButtonStates _newState)
    {
        CurrentState = _newState;
    }

    public virtual void InvokePressed()
    {
        if (pressedMethod != null)
        {
            pressedMethod.Invoke();
        }
    }

    public virtual void InvokeUp()
    {
        if (upMethod != null)
        {
            upMethod.Invoke();
        }
    }

    public virtual void InvokeDown()
    {
        if (downMethod != null)
        {
            downMethod.Invoke();
        }
    }
}
