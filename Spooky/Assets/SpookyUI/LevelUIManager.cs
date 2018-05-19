using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : SceneSingleton<LevelUIManager>
{
    private GameObject joystick;
    private GameObject fireButton;
    private GameObject pauseButton;
    private Image cropUIBar;
    private Image waveProgressBar;
    private Text gameMoneyText;
    private Text enemiesCounter;

    [Header("Game state UI's editor binding")]
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject winUI;
    [SerializeField]
    private GameObject pauseUI;

    protected override void Awake()
    {
        base.Awake();

        joystick = transform.Find("Joystick").gameObject;
        fireButton = transform.Find("FireButton").gameObject;
        pauseButton = transform.Find("PauseButton").gameObject;
        pauseUI = transform.Find("PauseUI").gameObject;
        gameOverUI = transform.Find("WinGameUI").gameObject;
        winUI = transform.Find("GameOverUI").gameObject;
        cropUIBar = transform.Find("CropBarFrame").Find("CropBar").GetComponent<Image>();
        waveProgressBar = transform.Find("WaveBarFrame").Find("WaveProgressBar").GetComponent<Image>();
        gameMoneyText = transform.Find("CropBarFrame").Find("MoneyText").GetComponent<Text>();
        enemiesCounter = transform.Find("WaveBarFrame").Find("WaveCounter").GetComponent<Text>();
        return;
    }

    public void OnEnable()
    {
        WaveSpawner.Instance.OnSpawnStart += UpdateWaveSpawnerUI;
        WaveSpawner.Instance.OnEnemyKilled += UpdateWaveSpawnerUI;
        LevelManager.Instance.OnCropSteal += UpdateCropUIBar;
    }

    public void OnDisable()
    {
        WaveSpawner.Instance.OnSpawnStart -= UpdateWaveSpawnerUI;
        WaveSpawner.Instance.OnEnemyKilled -= UpdateWaveSpawnerUI;
        LevelManager.Instance.OnCropSteal -= UpdateCropUIBar;
    }

    public void Start ()
    {
        ActivatePauseUI(false);
        ActivateGameOverUI(false);
        ActivateWinUI(false);

        UpdateCropUIBar();
        ChangemoneyDisplay();
    }

    protected void UpdateCropUIBar()
    {
        float num = LevelManager.Instance.CurrentCrop;
        float den = LevelManager.Instance.CurrentCrop;
        cropUIBar.fillAmount = num / den;
        return;
    }

    public void ChangemoneyDisplay()
    {
        gameMoneyText.text = "$:" + LevelManager.Instance.CurrentMoney;
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

    private void UpdateWaveSpawnerUI(float _currentWave ,float _maxEnemies, float _currentEnemiesKilled)
    {
        enemiesCounter.text = _currentWave.ToString();
        waveProgressBar.fillAmount = _currentEnemiesKilled / _maxEnemies;
        return;
    }

}
