using System.Collections;
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
        [SerializeField]
        public Enemy[] enemy;
        public float spawnRate;
        //How many of each enemy to spawn
        public int[] count;
    }

    //To store different waves
    public Wave[] waves;
    public int numberOfEnemies = 0;
    //Wich wave is next
    public int nextWave = 0;
    //Automatic next wave spawn
    public float timeBetweenWaves = 5.0f;
    //Wave spawn counter
    private float waveCountDown;

    public AudioClip spawnStart;

    //State of the waveSpawn
    public SpawnState state;

    public delegate void SpawnStart();
    public event SpawnStart OnSpawnStart;

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
        waveCountDown = timeBetweenWaves;
        state = SpawnState.COUNTING;
    }
    // Update is called once per frame
    private void Update()
    {
        //To see if all waves are finished
        if (nextWave == waves.Length)
        {
            LevelManager.Instance.WinLevel();
            this.enabled = false;
            return;
        }

        if (state == SpawnState.WAITING)
        {
            if (numberOfEnemies <= 0)
            {
                //Begin a new Round
                WaveCompleted();
                return;
            }
        }

        if (waveCountDown <= 0.0f && numberOfEnemies <= 0)
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
    private IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        OnSpawnStart();
        numberOfEnemies = 0;
        for (int enemy = 0; enemy < _wave.enemy.Length; enemy++)
        {
            for (int count = 0; count < _wave.count[enemy]; count++)
            {
                SpawnEnemy((ISpawnable<Enemy>)_wave.enemy[enemy]);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }
        }
        state = SpawnState.WAITING;
        yield break;
    }

    private void SpawnEnemy(ISpawnable<Enemy> _enemy)
    {
        //Random between all the spawnpoints
        var spawnPoint = LevelManager.Instance.GetRandomSpawnPoint();
        var parentPool = GameObject.Find("Enemies");
        Enemy tempEnemy = _enemy.Spawn(spawnPoint);
        tempEnemy.OnReleased += DecreaseEnemyNumber;
        tempEnemy.transform.SetParent(parentPool.transform);
        numberOfEnemies++;
    }

    private void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;
        nextWave++;
        Debug.Log("Se completo una wave");
    }

    void DecreaseEnemyNumber()
    {
        numberOfEnemies--;
        Mathf.Clamp(numberOfEnemies, 0, Mathf.Infinity);    // So the number of enemies never goes below 0
    }

    void PlaySpawnClip()
    {
        GameManager.Instance.SoundManager.PlayClip(spawnStart);
    }
}
