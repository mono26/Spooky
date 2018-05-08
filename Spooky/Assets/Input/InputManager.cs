using UnityEngine;

public class InputManager : SceneSingleton<InputManager>
{
    public string playerID;

    public bool AutoDetectMovile { get; protected set; }
    public bool IsOnMobile { get; protected set; }
    public CustomButton FireButton { get; protected set; }

    public bool hideControlsInEditor = false;

    [SerializeField]
    private Vector3 movement = Vector3.zero;
    public Vector3 Movement { get { return movement; } }


    // Use this for initialization
    void Start ()
    {
        DetectTypeOfInput();

        FireButton = new CustomButton("FireButton", FireButtonDown, FireButtonPressed, FireButtonUp);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsOnMobile)
        {
            SetMovePosition();
            GetInputFromButton();
        }
    }

    private void DetectTypeOfInput()
    {
        if(LevelUIManager.Instance)
        {
            LevelUIManager.Instance.ActivatePlayerControls(false);
            IsOnMobile = false;
            
            if(AutoDetectMovile)
            {
                #if UNITY_ANDROID || UNITY_IPHONE
		        LevelUIManager.Instance.ActivatePlayerControls(true);
		        IsOnMobile = true;
                #endif
            }

            if (hideControlsInEditor)
            {
                #if UNITY_EDITOR
                LevelUIManager.Instance.ActivatePlayerControls(false);
                IsOnMobile = false;
                #endif
            }
        }
    }

    public void SetMovePosition()
    {
        if (!IsOnMobile)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }
    }

    public void GetInputFromButton()
    {
        if (Input.GetButton(FireButton.ButtonID))
        {
            FireButton.InvokeDown();
        }
        if (Input.GetButtonDown(FireButton.ButtonID))
        {
            FireButton.InvokePressed();
        }
        if (Input.GetButtonUp(FireButton.ButtonID))
        {
            FireButton.InvokeUp();
        }
    }

    public void SetMovePosition(Vector2 _position)
    {
        if (IsOnMobile)
        {
            movement.x = _position.x;
            movement.y = _position.y;
        }
    }

    public virtual void FireButtonDown() { FireButton.ChangeButtonState(CustomButton.ButtonStates.Down); }
    public virtual void FireButtonPressed() { FireButton.ChangeButtonState(CustomButton.ButtonStates.Pressed); }
    public virtual void FireButtonUp() { FireButton.ChangeButtonState(CustomButton.ButtonStates.Up); }

}
