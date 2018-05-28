﻿using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : SceneSingleton<LevelUIManager>, EventHandler<CharacterEvent>, EventHandler<GameEvent>
{
    [Header("UI elements")]
    [SerializeField]
    private Image cropUIBar;
    [SerializeField]
    private GameObject fireButton;
    [SerializeField]
    private Text gameMoneyText;
    [SerializeField]
    private GameObject joystick;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private Text waveCounter;
    [SerializeField]
    private Image waveProgressBar;

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

        if (cropUIBar == null)
            cropUIBar = transform.Find("CropBarFrame").Find("CropBar").GetComponent<Image>();
        if (fireButton == null)
            fireButton = transform.Find("FireButton").gameObject;
        if (gameMoneyText == null)
            gameMoneyText = transform.Find("CropBarFrame").Find("MoneyText").GetComponent<Text>();
        if (gameOverUI == null)
            gameOverUI = transform.Find("GameOverUI").gameObject;
        if (joystick == null)
            joystick = transform.Find("Joystick").gameObject;
        if (pauseButton == null)
            pauseButton = transform.Find("PauseButton").gameObject;
        if (pauseUI == null)
            pauseUI = transform.Find("PauseUI").gameObject;
        if (winUI == null)
            winUI = transform.Find("WinGameUI").gameObject;
        if (waveCounter == null)
            waveCounter = transform.Find("WaveBarFrame").Find("WaveCounter").GetComponent<Text>();
        if (waveProgressBar == null)
            waveProgressBar = transform.Find("WaveBarFrame").Find("WaveProgressBar").GetComponent<Image>();

        return;
    }

    protected void Start ()
    {
        ActivatePauseUI(false);
        ActivateGameOverUI(false);
        ActivateWinUI(false);

        UpdateCropUIBar();
        ChangemoneyDisplay();
    }

    protected void OnEnable()
    {
        EventManager.AddListener<CharacterEvent>(this);
        EventManager.AddListener<GameEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<CharacterEvent>(this);
        EventManager.RemoveListener<GameEvent>(this);
        return;
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

    private void UpdateWaveSpawnerUI()
    {
        waveCounter.text = WaveSpawner.Instance.CurrentWave.ToString();
        waveProgressBar.fillAmount = WaveSpawner.Instance.CurrentNumberOfEnemiesKilled / WaveSpawner.Instance.CurrentMaxNumberOfEnemiesLeft;
        return;
    }

    public void OnEvent(CharacterEvent _characterEvent)
    {
        if (!_characterEvent.character.CharacterID.Equals("Spooky"))
        {
            if (_characterEvent.type == CharacterEventType.Death)
                UpdateWaveSpawnerUI();
        }
        return;
    }

    public void OnEvent(GameEvent _gameManagerEvent)
    {
        if (_gameManagerEvent.type == GameEventTypes.SpawnStart)
            UpdateWaveSpawnerUI();
        else if (_gameManagerEvent.type == GameEventTypes.CropSteal)
            UpdateCropUIBar();
        return;
    }
}
