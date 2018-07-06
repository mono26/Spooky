using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>, EventHandler<GameEvent>, EventHandler<PickEvent>
{
    [Header("Level Manager settings")]
    [SerializeField]
    protected AudioClip backgroundMusicClip;
    [SerializeField]
    private ProgressBar cropLeft;
    [SerializeField]
    private Text gameMoneyText;
    [SerializeField]
    protected AudioClip loseSfx;
    [SerializeField]
    protected string mainMenuScene;
    [SerializeField]
    private int maxCrop = 800;
    [SerializeField]
    private int startingMoney = 400;
    [SerializeField]
    protected AudioClip winSfx;

    [Header("Components")]
    [SerializeField]
    private Transform[] houseStealPoints;
    [SerializeField]
    private Transform[] runAwayPoints;
    [SerializeField]
    private Transform spooky;
    public Transform Spooky { get { return spooky; } }
    [SerializeField]
    private Transform[] spawnPoints;

    [Header("Editor debugging")]
    [SerializeField]
    public int currentCrop;
    public int CurrentCrop { get { return currentCrop; } }
    [SerializeField]
    public int currentMoney;
    public int CurrentMoney { get { return currentMoney; } }
    [SerializeField]
    private static bool gameIsOver = false;

    protected override void Awake()
    {
        base.Awake();

        if (cropLeft == null)
            cropLeft = transform.Find("CropBarFrame").Find("CropBar").GetComponent<ProgressBar>();
        if (gameMoneyText == null)
            gameMoneyText = transform.Find("CropBarFrame").Find("MoneyText").GetComponent<Text>();
        if (spooky == null)
            spooky = GameObject.FindWithTag("Spooky").transform;

        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();

        return;
    }

    protected void Start()
    {
        currentCrop = maxCrop;
        currentMoney = startingMoney;
        EventManager.TriggerEvent<FadeEvent>(new FadeEvent(FadeEventType.FadeOut));
        SoundManager.Instance.PlayMusic(backgroundMusicClip);

        UpdateCropUIBar();
        UpdateMoneyDisplay();

        return;
    }

    protected void OnEnable()
    {
        EventManager.AddListener<GameEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<GameEvent>(this);
        return;
    }

    public Transform GetRandomHousePoint()
    {
        int random = Random.Range(0, houseStealPoints.Length);
        return houseStealPoints[random];
    }
    public Transform GetRandomEscapePoint()
    {
        int random = Random.Range(0, runAwayPoints.Length);
        return runAwayPoints[random];
    }
    public Transform GetRandomSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Length);
        return spawnPoints[random];
    }

    public void GameOver()
    {
        gameIsOver = true;

        if(loseSfx != null)
        {
            SoundManager.Instance.PlaySfx(GetComponent<AudioSource>(), loseSfx);
        }

        LevelUIManager.Instance.ActivateGameOverUI(true);
        return;
    }
    public void WinLevel()
    {
        gameIsOver = true;

        if(winSfx != null)
        {
            SoundManager.Instance.PlaySfx(GetComponent<AudioSource>(), winSfx);
        }

        LevelUIManager.Instance.ActivateWinUI(true);
        return;
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        LoadManager.LoadScene(mainMenuScene);
        SoundManager.Instance.StopSound();
        return;
    }

    public void RetryLevel()
    {
        EventManager.TriggerEvent(new GameEvent(GameEventTypes.UnPause));
        LoadManager.LoadScene(SceneManager.GetActiveScene().name);
        return;
    }

    public void GiveMoney(int reward)
    {
        currentMoney += reward;
        return;
    }

    public void TakeMoney(int money)
    {
        currentMoney -= money;
        return;
    }

    public void LoseCrop(int _stole)
    {
        currentCrop -= _stole;
        EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEventTypes.CropSteal));
        if (CurrentCrop <= 0)
        {
            //GameOver Code
            LevelManager.Instance.GameOver();
        }
        return;
    }

    public void UpdateMoneyDisplay()
    {
        gameMoneyText.text = "$:" + currentMoney;
        return;
    }

    protected void GiveCrop(int _gain)
    {
        currentCrop += _gain;
        EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEventTypes.CropSteal));
        return;
    }

    protected void UpdateCropUIBar()
    {
        if (maxCrop > 0)
            cropLeft.UpdateBar(currentCrop, maxCrop);
        return;
    }

    protected void LookForSpawnPoints()
    {
        var sp = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[sp.Length];
        for (int i = 0; i < sp.Length; i++)
        {
            spawnPoints[i] = sp[i].GetComponent<Transform>();
        }
        return;
    }
    protected void LookForRunAwayPoints()
    {
        var rp = GameObject.FindGameObjectsWithTag("RunawayPoint");
        runAwayPoints = new Transform[rp.Length];
        for (int i = 0; i < rp.Length; i++)
        {
            runAwayPoints[i] = rp[i].GetComponent<Transform>();
        }
        return;
    }
    protected void LookForHousePoints()
    {
        var hPoints = GameObject.FindGameObjectsWithTag("StealPoint");
        houseStealPoints = new Transform[hPoints.Length];
        for (int i = 0; i < hPoints.Length; i++)
        {
            houseStealPoints[i] = hPoints[i].GetComponent<Transform>();
        }
        return;
    }

    public void OnEvent(GameEvent _gameManagerEvent)
    {
        if (_gameManagerEvent.type == GameEventTypes.CropSteal)
            UpdateCropUIBar();

        return;
    }

    public void OnEvent(PickEvent _pickEvent)
    {
        if(_pickEvent.whoPicks.CharacterID.Equals("Spooky") == true)
        {
            if (_pickEvent.type == PickEventType.CornBag)
            {
                GiveCrop(_pickEvent.pickValue);
                UpdateCropUIBar();
            }
            else if (_pickEvent.type == PickEventType.EnemySoul)
            {
                GiveMoney(_pickEvent.pickValue);
                UpdateMoneyDisplay();
            }
        }

        return;
    }
}
