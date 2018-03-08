using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    //Singleton part
    private static WaveSpawner instance;
    public static WaveSpawner Instance
    {
        get { return instance; }
    }

    public enum SpawnState
    {
        SPAWNING, COUNTING, WAITING
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        //To save the diferent enemy info
        public Enemy[] enemy;
        public float spawnRate;
        //How many of each enemy to spawn
        public int[] count;
    }

    //To store different waves
    public Wave[] waves;
    public Image gameWaveBar;
    public int gameNumberOfEnemies = 0;
    //Wich wave is next
    public int nextWave = 0;
    //Automatic next wave spawn
    public float timeBetweenWaves = 5.0f;
    //Wave spawn counter
    private float waveCountDown;

    public AudioClip spawnStart;

    //State of the waveSpawn
    public SpawnState state;

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    // Use this for initialization
    void Start()
    {
        waveCountDown = timeBetweenWaves;
        state = SpawnState.COUNTING;
        gameWaveBar.fillAmount = (float)(nextWave) / (float)(waves.Length);

        //SoundHandler.Instance.PlayClip(spawnStart);
    }
    // Update is called once per frame
    void Update()
    {
        //To see if all waves are finished
        if (nextWave == waves.Length)
        {
            LevelManager.Instance.WinLevel();
            this.enabled = false;
        }

        if (state == SpawnState.WAITING)
        {
            if (gameNumberOfEnemies <= 0)
            {
                //Begin a new Round
                WaveCompleted();
            }
        }
        if (waveCountDown <= 0.0f && gameNumberOfEnemies <= 0)
        {
            //If the countdown to start spawning the next wave is 0 and is not spawning auotomaticly force it to spawn
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
            waveCountDown = Mathf.Clamp(waveCountDown, 0f, Mathf.Infinity);
        }
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        gameWaveBar.fillAmount = (float)(nextWave + 1) / (float)(waves.Length);
        gameNumberOfEnemies = 0;
        for (int enemy = 0; enemy < _wave.enemy.Length; enemy++)
        {
            for (int count = 0; count < _wave.count[enemy]; count++)
            {
                SpawnEnemy(_wave.enemy[enemy]);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        }
        state = SpawnState.WAITING;
        yield break;
    }
    void SpawnEnemy(Enemy _enemy)
    {
        //Random between all the spawnpoints
        var spawnPoint = LevelManager.Instance.GetRandomSpawnPoint();
        GameObject tempEnemy = Instantiate(_enemy.gameObject, spawnPoint.position, _enemy.gameObject.transform.rotation);
        //PoolsManagerEnemies.Instance.GetEnemy(_enemy.objectIndex, my_SpawnPoint);
        gameNumberOfEnemies++;
    }
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;
        nextWave++;
        Debug.Log("Se completo una wave");
    }
}
