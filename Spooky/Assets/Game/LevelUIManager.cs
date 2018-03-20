using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUIManager
{
    public GameObject topUI;
    public GameObject bottomUI;
    public GameObject pauseButton;
    public GameObject pauseCanvas;

    // TODO getter
    public bool isPaused;

    public Image cropUIBar;
    public Image waveBar;
    public Text gameMoneyText;
    public Text enemiesCounter;
    public int startingMoney = 400;
    public int currentMoney;
    public int maxCrop = 800;
    public float currentCrop;

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
        enemiesCounter.text = WaveSpawner.Instance.GetNumberOfEnemies().ToString();
    }

    public void LoseCrop(int _stole)
    {
        currentCrop -= _stole;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        if (currentCrop <= 0)
        {
            //GameOver Code
            LevelManager.Instance.GameOver();
        }
    }

    public void GiveMoney(int reward)
    {
        currentMoney += reward;
        gameMoneyText.text = "$:" + currentMoney;
    }

    public void TakeMoney(int money)
    {
        currentMoney -= money;
        gameMoneyText.text = "$:" + currentMoney;
    }

    private void HideUI()
    {
        topUI.SetActive(false);
        bottomUI.SetActive(false);
        pauseCanvas.SetActive(false);
        pauseButton.SetActive(false);
    }

    private void ShowUI()
    {
        topUI.SetActive(true);
        bottomUI.SetActive(true);
    }

    private void IncreaseWave()
    {
        waveBar.fillAmount = (float)(WaveSpawner.Instance.nextWave) + 1 / (float)(WaveSpawner.Instance.waves.Length);
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadLevel(0);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        var currentScene = SceneManager.GetActiveScene();
        int scenIndex = currentScene.buildIndex;
        GameManager.Instance.LoadLevel(scenIndex);
    }

    public void PauseLevel()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            return;
        }
        else if (!isPaused)
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            return;
        }
    }
}
