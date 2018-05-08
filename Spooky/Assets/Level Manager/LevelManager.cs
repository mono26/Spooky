using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SceneSingleton<LevelManager>
{
    public string mainMenuScene;

    // HouseSteal points used by the Stealer to note setting the target to the middle of the house. (out of navmesh)
    [SerializeField]
    private Transform[] houseStealPoints;
    // Used by the waveSpawner for spawning the waves.
    [SerializeField]
    private Transform[] spawnPoints;
    // used by the stealers to run when they have the loot.
    [SerializeField]
    private Transform[] runAwayPoints;
    //TODO add get for spooky
    public Transform spooky;

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

    public AudioClip backgroundMusicClip;

    public delegate void OnStartLevelDelegate();
    public event OnStartLevelDelegate OnStartLevel;

    protected override void Awake()
    {
        base.Awake();

        spooky = GameObject.FindGameObjectWithTag("Spooky").transform;
        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnFinishloading += StartLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnFinishloading -= StartLevel;
    }

    private void Start()
    {
        CurrentCrop = maxCrop;
        CurrentMoney = startingMoney;
    }

    // Caching
    private void LookForSpawnPoints()
    {
        var sp = GameObject.FindGameObjectsWithTag("Spawn Point");
        spawnPoints = new Transform[sp.Length];
        for (int i = 0; i < sp.Length; i++)
        {
            spawnPoints[i] = sp[i].GetComponent<Transform>();
        }
    }
    private void LookForRunAwayPoints()
    {
        var rp = GameObject.FindGameObjectsWithTag("Runaway Point");
        runAwayPoints = new Transform[rp.Length];
        for (int i = 0; i < rp.Length; i++)
        {
            runAwayPoints[i] = rp[i].GetComponent<Transform>();
        }
    }
    private void LookForHousePoints()
    {
        var hPoints = GameObject.FindGameObjectsWithTag("Steal Point");
        houseStealPoints = new Transform[hPoints.Length];
        for (int i = 0; i < hPoints.Length; i++)
        {
            houseStealPoints[i] = hPoints[i].GetComponent<Transform>();
        }
    }

    public Transform GetRandomHousePoint()
    {
        int random = Random.Range(0, houseStealPoints.Length);
        return houseStealPoints[random];
    }
    public Transform GetRandomRunawayPoint()
    {
        int random = Random.Range(0, runAwayPoints.Length);
        return runAwayPoints[random];
    }
    public Transform GetRandomSpawnPoint()
    {
        int random = Random.Range(0, runAwayPoints.Length);
        return spawnPoints[random];
    }

    public void GameOver()
    {
        gameIsOver = true;
        // SoundHandler.Instance.PlayClip(gameSounds[0]);
        //loseUI.SetActive(true);
    }
    public void WinLevel()
    {
        gameIsOver = true;
        //winUI.SetActive(true);
    }

    void PlayBackGroundLevelMusic()
    {
        GameManager.Instance.SoundManager.PlayMusic(backgroundMusicClip);
        return;
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

    private void StartLevel()
    {
        if (OnStartLevel != null)
            OnStartLevel();

        PlayBackGroundLevelMusic();
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
        if (CurrentCrop <= 0)
        {
            //GameOver Code
            LevelManager.Instance.GameOver();
        }
    }
}
