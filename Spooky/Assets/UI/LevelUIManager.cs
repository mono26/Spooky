using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    [Header("UI elements")]
    [SerializeField]
    private GameObject fireButton;
    [SerializeField]
    private GameObject joystick;
    [SerializeField]
    private GameObject buildCanvasUI;
    [SerializeField]
    private GameObject plantCanvasUI;

    [Header("Game state UI's")]
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject winUI;

    [SerializeField]
    private float zOffsetForCanvasLocation = 0.7f;

    protected override void Awake()
    {
        base.Awake();

        if (!fireButton)
        {
            fireButton = transform.Find("FireButton").gameObject;
        }
        if (!gameOverUI)
        {
            gameOverUI = transform.Find("GameOverUI").gameObject;
        }
        if (!joystick)
        {
            joystick = transform.Find("Joystick").gameObject;
        }
        if (!pauseUI)
        {
            pauseUI = transform.Find("PauseUI").gameObject;
        }
        if (!winUI)
        {
            winUI = transform.Find("WinGameUI").gameObject;
        }
        if (!buildCanvasUI)
        {
            buildCanvasUI = GameObject.FindGameObjectWithTag("BuildCanvas");
        }
        if (!plantCanvasUI)
        {
            plantCanvasUI = GameObject.FindGameObjectWithTag("PlantCanvas");
        }
    }

    protected void Start ()
    {
        ActivatePauseUI(false);
        ActivateGameOverUI(false);
        ActivateWinUI(false);
        ActivateBuildUI(false);
        ActivatePlantUI(false);
    }

    public void ActivatePlayerControls(bool _active)
    {
        joystick.SetActive(_active);
        fireButton.SetActive(_active);
        return;
    }

    public void ActivatePauseUI(bool _active) 
    {
        if (pauseUI && pauseUI.activeSelf != _active)
        {
            pauseUI.SetActive(_active); 
        }
    }

    public void ActivateGameOverUI(bool _active)
    {
        if (gameOverUI && gameOverUI.activeSelf != _active)
        {
            gameOverUI.SetActive(_active);
        } 
    }

    public void ActivateWinUI(bool _active) 
    {
        if (winUI && winUI.activeSelf != _active)
        {
            winUI.SetActive(_active);
        }
    }

    public void ActivateBuildUI(bool _active) 
    {
        if (buildCanvasUI && buildCanvasUI.activeSelf != _active)
        {
            buildCanvasUI.SetActive(_active);
        }
    }

    public void ActivatePlantUI(bool _active) 
    {
        if (plantCanvasUI && plantCanvasUI.activeSelf != _active)
        {
            plantCanvasUI.SetActive(_active);
        }
    }

    /// <summary>
    /// Activates the plant UI in a target plantpoint.
    /// </summary>
    /// <param name="plantPoint"> Plantpoint where the UI should be display.</param>
    public void ActivatePlantUI(Plantpoint plantPoint)
    {
        plantCanvasUI.transform.position = plantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        ActivatePlantUI(true);
    }

    /// <summary>
    /// Activates the build UI in a target plantpoint.
    /// </summary>
    /// <param name="plantPoint"> Plantpoint where the UI should be display.</param>
    public void ActivateBuildUI(Plantpoint plantPoint)
    {
        buildCanvasUI.transform.position = plantPoint.transform.position + new Vector3(0, 0, zOffsetForCanvasLocation);
        ActivateBuildUI(true);
    }
}
