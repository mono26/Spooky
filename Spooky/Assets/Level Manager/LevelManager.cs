using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    private LevelUIManager uiManager;
    public LevelUIManager UiManager { get { return uiManager; } }

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

    // TODO set up automatic set in script, not in editor.
    //Variables relacionadas con el fin del nivel
    private static bool gameIsOver = false;

    [SerializeField]
    private GameObject loseUI;
    [SerializeField]
    private GameObject winUI;

    public AudioClip musicClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        uiManager = new LevelUIManager(
            GameObject.FindGameObjectWithTag("CropBar").GetComponent<Image>(),
            GameObject.FindGameObjectWithTag("WaveBar").GetComponent<Image>(),
            GameObject.FindGameObjectWithTag("Money Text").GetComponent<Text>(),
            GameObject.FindGameObjectWithTag("TopUIInfo"),
            GameObject.FindGameObjectWithTag("BottomUIInfo"),
            GameObject.FindGameObjectWithTag("Pause Button"),
            GameObject.FindGameObjectWithTag("PauseUI"),
            GameObject.FindGameObjectWithTag("Enemies Counter").GetComponent<Text>()
            );

        spooky = GameObject.FindGameObjectWithTag("Spooky").transform;
        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();

        GameManager.Instance.OnStartGame += PlayLevelMusic;
    }

    private void OnDisable()
    {
        uiManager.OnDisable();
    }

    private void Start()
    {
        uiManager.Start();
    }

    private void Update()
    {
        uiManager.Update();
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
        loseUI.SetActive(true);
    }
    public void WinLevel()
    {
        gameIsOver = true;
        winUI.SetActive(true);
    }

    void PlayLevelMusic()
    {
        GameManager.Instance.SoundManager.PlayMusic(musicClip);
    }

    public void QuitLevel()
    {
        uiManager.QuitLevel();
    }

    public void RetryLevel()
    {
        uiManager.RetryLevel();
    }

    public void PauseLevel()
    {
        uiManager.PauseLevel();
    }
}
