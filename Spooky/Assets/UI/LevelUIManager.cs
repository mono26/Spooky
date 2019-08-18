using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    [Header("UI elements")]
    [SerializeField]
    private GameObject fireButton;
    [SerializeField]
    private GameObject joystick;

    [Header("Game state UI's")]
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject winUI;

    protected override void Awake()
    {
        base.Awake();

        if (fireButton == null)
            fireButton = transform.Find("FireButton").gameObject;
        if (gameOverUI == null)
            gameOverUI = transform.Find("GameOverUI").gameObject;
        if (joystick == null)
            joystick = transform.Find("Joystick").gameObject;
        if (pauseUI == null)
            pauseUI = transform.Find("PauseUI").gameObject;
        if (winUI == null)
            winUI = transform.Find("WinGameUI").gameObject;

        return;
    }

    protected void Start ()
    {
        ActivatePauseUI(false);
        ActivateGameOverUI(false);
        ActivateWinUI(false);

        return;
    }

    public void ActivatePlayerControls(bool _active)
    {
        joystick.SetActive(_active);
        fireButton.SetActive(_active);
        return;
    }

    public void ActivatePauseUI(bool _active) { pauseUI.SetActive(_active); }

    public void ActivateGameOverUI(bool _active){ gameOverUI.SetActive(_active); }

    public void ActivateWinUI(bool _active) { winUI.SetActive(_active); }
}
