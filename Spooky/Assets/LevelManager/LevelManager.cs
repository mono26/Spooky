using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SceneSingleton<LevelManager>
{
    public string mainMenuScene;

    [SerializeField]
    private Transform[] houseStealPoints;
    [SerializeField]
    private Transform[] runAwayPoints;
    public Transform spooky;
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

        spooky = GameObject.FindWithTag("Spooky").transform;
        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();
    }

    private void Start()
    {
        CurrentCrop = maxCrop;
        CurrentMoney = startingMoney;
        EventManager.TriggerEvent<FadeEvent>(new FadeEvent(FadeEventType.FadeOut));
        SoundManager.Instance.PlayMusic(backgroundMusicClip);
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
    public Transform GetRandomEscapePoint()
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

    public void QuitLevel()
    {
        Time.timeScale = 1;
        LoadManager.LoadScene(mainMenuScene);
        //GameManager.Instance.SoundManager.StopMusic();
    }

    public void RetryLevel()
    {
        /*Time.timeScale = 1;
        GameManager.Instance.StartCoroutine(
            GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().name)
            );*/
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
    }
}
