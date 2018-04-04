using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        //To save the diferent enemy info
        [SerializeField]
        public Enemy[] enemy;
        public float spawnRate;
        //How many of each enemy to spawn
        public int[] count;
    }

    public enum SpawnState
    {
        SPAWNING, COUNTING, WAITING
    }

    //Singleton part
    private static WaveSpawner instance;
    public static WaveSpawner Instance { get { return instance; } }

    //To store different waves
    [SerializeField]
    private Wave[] waves;
    public Wave[] Waves { get { return waves; } }
    [SerializeField]
    private int numberOfEnemiesInCurrentWave = 0;
    public int NumberOfEnemiesInCurrentWave { get { return numberOfEnemiesInCurrentWave; } }
    //Wich wave is next
    [SerializeField]
    private int currentWave = 0;
    public int CurrentWave { get { return currentWave; } }
    //Automatic next wave spawn
    [SerializeField]
    private float timeBetweenNextWaveSpawn = 5.0f;
    //Wave spawn counter
    [SerializeField]
    private float waveTimerCountDown;

    [SerializeField]
    private AudioClip firstSpawnSound;

    //State of the waveSpawn
    private SpawnState waveSpawnerState;

    public delegate void SpawnStartDelegate();
    public event SpawnStartDelegate OnSpawnStart;

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        // TODO mejorar proteccion
        if(GameManager.Instance)
            GameManager.Instance.OnStartGame += PlaySpawnClip;
    }
    // Use this for initialization
    private void Start()
    {
        waveTimerCountDown = timeBetweenNextWaveSpawn;
        waveSpawnerState = SpawnState.COUNTING;
    }
    // Update is called once per frame
    private void Update()
    {
        //To see if all waves are finished
        if (currentWave == waves.Length)
        {
            LevelManager.Instance.WinLevel();
            this.enabled = false;
            return;
        }

        if (waveSpawnerState == SpawnState.WAITING)
        {
            if (numberOfEnemiesInCurrentWave <= 0)
            {
                //Begin a new Round
                WaveCompleted();
                return;
            }
        }

        if (waveTimerCountDown <= 0.0f && numberOfEnemiesInCurrentWave <= 0)
        {
            //If the countdown to start spawning the next wave is 0 and is not spawning auotomaticly force it to spawn
            if (waveSpawnerState != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
            }
        }
        else
        {
            waveTimerCountDown -= Time.deltaTime;
            waveTimerCountDown = Mathf.Clamp(waveTimerCountDown, 0f, Mathf.Infinity);
        }
    }
    private IEnumerator SpawnWave(Wave _wave)
    {
        waveSpawnerState = SpawnState.SPAWNING;
        OnSpawnStart();
        numberOfEnemiesInCurrentWave = 0;
        for (int enemy = 0; enemy < _wave.enemy.Length; enemy++)
        {
            for (int count = 0; count < _wave.count[enemy]; count++)
            {
                SpawnEnemy((ISpawnable<Enemy>)_wave.enemy[enemy]);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        }
        waveSpawnerState = SpawnState.WAITING;
        yield break;
    }

    private void SpawnEnemy(ISpawnable<Enemy> _enemy)
    {
        //Random between all the spawnpoints
        Enemy tempEnemy = _enemy.Spawn(LevelManager.Instance.GetRandomSpawnPoint());
        tempEnemy.OnReleased += DecreaseEnemyNumber;
        tempEnemy.transform.SetParent(GameObject.Find("Enemies").transform);
        numberOfEnemiesInCurrentWave++;
    }

    private void WaveCompleted()
    {
        waveSpawnerState = SpawnState.COUNTING;
        waveTimerCountDown = timeBetweenNextWaveSpawn;
        currentWave++;
    }

    void DecreaseEnemyNumber()
    {
        numberOfEnemiesInCurrentWave--;
        numberOfEnemiesInCurrentWave = (int)Mathf.Clamp(numberOfEnemiesInCurrentWave, 0, Mathf.Infinity);    // So the number of enemies never goes below 0
    }

    void PlaySpawnClip()
    {
        GameManager.Instance.SoundManager.PlayClip(firstSpawnSound);
    }
}
