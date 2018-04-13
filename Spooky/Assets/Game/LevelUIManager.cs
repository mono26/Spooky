using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUIManager
{
    public string mainMenuScene;

    private GameObject topUI;
    private GameObject bottomUI;
    private GameObject pauseButton;
    private GameObject pauseCanvas;
    private Image cropUIBar;
    private Image waveBar;
    private Text gameMoneyText;
    private Text enemiesCounter;

    private int startingMoney = 400;
    private float currentMoney;
    public float CurrentMoney { get { return currentMoney; } }

    private float maxCrop = 800;
    private float currentCrop;
    public float CurrentCrop { get { return currentCrop; } }

    private bool isPaused;
    public bool IsPaused { get { return isPaused; } }

    public LevelUIManager(
        Image _cropUIBar, Image _waveBar, Text _gameMoneyText, 
        GameObject _topUi, GameObject _bottomUi, GameObject _pauseButton, 
        GameObject _pauseCanvas, Text _enemiesCounter)
    {
        topUI = _topUi;
        bottomUI = _bottomUi;
        cropUIBar = _cropUIBar;
        waveBar = _waveBar;
        gameMoneyText = _gameMoneyText;
        pauseButton = _pauseButton;
        pauseCanvas = _pauseCanvas;
        enemiesCounter = _enemiesCounter;

        HideUI();
        //TODO better protection
        if(GameManager.Instance)
            GameManager.Instance.OnStartGame += ShowUI;

    }

    public void OnDisable()
    {
        //TODO better protection
        if (WaveSpawner.Instance)
            WaveSpawner.Instance.OnSpawnStart -= IncreaseWave;
    }

    public void Start ()
    {
        WaveSpawner.Instance.OnSpawnStart += IncreaseWave;

        currentCrop = maxCrop;
        currentMoney = startingMoney;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        gameMoneyText.text = "" + currentMoney;

        IncreaseWave();
    }

    public void Update()
    {
        enemiesCounter.text = WaveSpawner.Instance.NumberOfEnemiesInCurrentWave.ToString();
    }

    public void LoseCrop(float _stole)
    {
        currentCrop -= _stole;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        if (currentCrop <= 0)
        {
            //GameOver Code
            LevelManager.Instance.GameOver();
        }
    }

    public void GiveMoney(float reward)
    {
        currentMoney += reward;
        gameMoneyText.text = "$:" + currentMoney;
    }

    public void TakeMoney(float money)
    {
        currentMoney -= money;
        gameMoneyText.text = "$:" + currentMoney;
    }

    private void HideUI()
    {
        topUI.SetActive(false);
        bottomUI.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    private void ShowUI()
    {
        topUI.SetActive(true);
        bottomUI.SetActive(true);
    }

    private void IncreaseWave()
    {
        waveBar.fillAmount = (float)(WaveSpawner.Instance.CurrentWave) / (float)(WaveSpawner.Instance.Waves.Length);
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(mainMenuScene));
        GameManager.Instance.SoundManager.StopMusic();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        GameManager.Instance.StartCoroutine(
            GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().name)
            );
    }

    public void PauseLevel()
    {
        isPaused = !isPaused;
        if (IsPaused)
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            return;
        }
        else if (!IsPaused)
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            return;
        }
    }
}
