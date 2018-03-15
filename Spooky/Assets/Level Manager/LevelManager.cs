﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public LevelUIManager uiManager;

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
    public static bool GameIsOver;
    public GameObject gameOverUI;
    public GameObject completeLvlUI;

    // TODO set up automatic set in script, not in editor.
    public Canvas pauseCanvas;
    public Button botonPause;
    public bool isPaused;

    public AudioClip musicClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        uiManager = new LevelUIManager(
            GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Image>(),
            GameObject.FindGameObjectWithTag("Wave Bar").GetComponent<Image>(),
            GameObject.FindGameObjectWithTag("Money Text").GetComponent<Text>(),
            GameObject.FindGameObjectWithTag("Top UI Info"),
            GameObject.FindGameObjectWithTag("Bottom UI Info")
            );

        spooky = GameObject.FindGameObjectWithTag("Spooky").transform;
        LookForHousePoints();
        LookForRunAwayPoints();
        LookForSpawnPoints();

        pauseCanvas = GameObject.FindGameObjectWithTag("Pause UI").GetComponent<Canvas>();
        botonPause = GameObject.FindGameObjectWithTag("Pause Button").GetComponent<Button>();

        pauseCanvas.enabled = false;

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

    void PlayLevelMusic()
    {
        GameManager.Instance.SoundManager.PlayMusic(musicClip);
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        var CurrentScene = SceneManager.GetActiveScene();
        string SceneName = CurrentScene.name;
        GameManager.Instance.LoadLevel(SceneName);
    }

    public void PauseLevel()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseCanvas.enabled = true;
            Time.timeScale = 0;
        }
        else if (!isPaused)
        {
            pauseCanvas.enabled = false;
            Time.timeScale = 1;
        }

    }
}
