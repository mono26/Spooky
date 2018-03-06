using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // HouseSteal points used by the Stealer to note setting the target to the middle of the house. (out of navmesh)
    public Transform[] houseStealPoints;
    // Used by the waveSpawner for spawning the waves.
    public Transform[] spawnPoints;
    // used by the stealers to run when they have the loot.
    public Transform[] runAwayPoints;

    public Transform spooky;

    public Image gameHealthBar;
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
        //Si ambas referencias no han sido asignadas por en el editor las debe de encontrar.
        spooky = GameObject.FindGameObjectWithTag("Spooky").transform;
        LookForPlayerAndHousePoints();
        //Para buscar todos los runaway y spawn points
        LookForRunAwayPoints();
        LookForSpawnPoints();
        LookForGameDependencies();
    }
    void Start()
    {
        currentCrop = maxCrop;
        currentMoney = startingMoney;
        gameHealthBar.fillAmount = currentCrop / maxCrop;
        gameMoneyText.text = "" + currentMoney;
    }

    private void LookForSpawnPoints()
    {
        var sp = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[sp.Length];
        for (int i = 0; i < sp.Length; i++)
        {
            spawnPoints[i] = sp[i].GetComponent<Transform>();
        }
    }
    private void LookForRunAwayPoints()
    {
        var rp = GameObject.FindGameObjectsWithTag("RunAwayPoint");
        runAwayPoints = new Transform[rp.Length];
        for (int i = 0; i < rp.Length; i++)
        {
            runAwayPoints[i] = rp[i].GetComponent<Transform>();
        }
    }
    private void LookForPlayerAndHousePoints()
    {
        if (houseStealPoints == null)
        {
            var hPoints = GameObject.FindGameObjectsWithTag("StealPoint");
            houseStealPoints = new Transform[hPoints.Length];
            for (int i = 0; i < hPoints.Length; i++)
            {
                houseStealPoints[i] = hPoints[i].GetComponent<Transform>();
            }
        }
    }
    private void LookForGameDependencies()
    {
        gameMoneyText = GameObject.FindGameObjectWithTag("GameMoneyText").GetComponent<Text>();
        gameHealthBar = GameObject.FindGameObjectWithTag("GameHealthBar").GetComponent<Image>();
    }

    public void LoseHealth(int stole)
    {
        currentCrop -= stole;
        gameHealthBar.fillAmount = currentCrop / maxCrop;
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
