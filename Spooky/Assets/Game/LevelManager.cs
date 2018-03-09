using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    // HouseSteal points used by the Stealer to note setting the target to the middle of the house. (out of navmesh)
    [SerializeField]
    private Transform[] houseStealPoints;
    // Used by the waveSpawner for spawning the waves.
    [SerializeField]
    private Transform[] spawnPoints;
    // used by the stealers to run when they have the loot.
    [SerializeField]
    private Transform[] runAwayPoints;

    public Transform spooky;

    public Image cropUIBar;
    public Text gameMoneyText;
    public int startingMoney = 400;
    public int currentMoney;
    public int maxCrop = 800;
    public float currentCrop;

    //Variables relacionadas con el fin del nivel
    public static bool GameIsOver;
    public GameObject gameOverUI;
    public GameObject completeLvlUI;

    public AudioClip[] gameSounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        spooky = GameObject.FindGameObjectWithTag("Spooky").transform;
        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();
        LookForUIElements();
    }

    void Start()
    {
        currentCrop = maxCrop;
        currentMoney = startingMoney;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        gameMoneyText.text = "" + currentMoney;
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
    private void LookForUIElements()
    {
        gameMoneyText = GameObject.FindGameObjectWithTag("Money Text").GetComponent<Text>();
        cropUIBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Image>();
    }

    public void LoseCrop(int _stole)
    {
        currentCrop -= _stole;
        cropUIBar.fillAmount = currentCrop / maxCrop;
        if (currentCrop <= 0)
        {
            //GameOver Code
            GameOver();
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

    void GameOver()
    {
        GameIsOver = true;
        // SoundHandler.Instance.PlayClip(gameSounds[0]);
        gameOverUI.SetActive(true);
    }
    public void WinLevel()
    {
        GameIsOver = true;
        completeLvlUI.SetActive(true);
    }
}
