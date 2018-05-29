using UnityEngine;

public class LevelManager : SceneSingleton<LevelManager>
{
    public string mainMenuScene;

    [SerializeField]
    private Transform[] houseStealPoints;
    [SerializeField]
    private Transform[] runAwayPoints;
    [SerializeField]
    private Transform spooky;
    public Transform Spooky { get { return spooky; } }
    [SerializeField]
    private Transform[] spawnPoints;


    [SerializeField]
    private int startingMoney = 400;
    public int StartingMoney { get { return startingMoney; } }
    public int CurrentMoney { get; protected set;}

    [SerializeField]
    private int maxCrop = 800;
    public int MaxCrop { get { return maxCrop; } }
    public int CurrentCrop { get; protected set; }

    // TODO set up automatic set in script, not in editor.
    //Variables relacionadas con el fin del nivel
    private static bool gameIsOver = false;

    [SerializeField]
    protected AudioClip backgroundMusicClip;

    protected override void Awake()
    {
        base.Awake();

        if(spooky == null)
            spooky = GameObject.FindWithTag("Spooky").transform;

        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();

        return;
    }

    private void Start()
    {
        CurrentCrop = maxCrop;
        CurrentMoney = startingMoney;
        EventManager.TriggerEvent<FadeEvent>(new FadeEvent(FadeEventType.FadeOut));
        SoundManager.Instance.PlayMusic(backgroundMusicClip);
        return;
    }

    // Caching
    private void LookForSpawnPoints()
    {
        var sp = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[sp.Length];
        for (int i = 0; i < sp.Length; i++)
        {
            spawnPoints[i] = sp[i].GetComponent<Transform>();
        }
        return;
    }
    private void LookForRunAwayPoints()
    {
        var rp = GameObject.FindGameObjectsWithTag("RunawayPoint");
        runAwayPoints = new Transform[rp.Length];
        for (int i = 0; i < rp.Length; i++)
        {
            runAwayPoints[i] = rp[i].GetComponent<Transform>();
        }
        return;
    }
    private void LookForHousePoints()
    {
        var hPoints = GameObject.FindGameObjectsWithTag("StealPoint");
        houseStealPoints = new Transform[hPoints.Length];
        for (int i = 0; i < hPoints.Length; i++)
        {
            houseStealPoints[i] = hPoints[i].GetComponent<Transform>();
        }
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
        // SoundHandler.Instance.PlayClip(gameSounds[0]);
        //loseUI.SetActive(true);
        return;
    }
    public void WinLevel()
    {
        gameIsOver = true;
        //winUI.SetActive(true);
        return;
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        LoadManager.LoadScene(mainMenuScene);
        //GameManager.Instance.SoundManager.StopMusic();
        return;
    }

    public void RetryLevel()
    {
        /*Time.timeScale = 1;
        GameManager.Instance.StartCoroutine(
            GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().name)
            );*/
        return;
    }

    public void GiveMoney(int reward)
    {
        CurrentMoney += reward;
        return;
    }

    public void TakeMoney(int money)
    {
        CurrentMoney -= money;
        return;
    }

    public void LoseCrop(int _stole)
    {
        CurrentCrop -= _stole;
        EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEventTypes.CropSteal));
        if (CurrentCrop <= 0)
        {
            //GameOver Code
            LevelManager.Instance.GameOver();
        }
        return;
    }
}
