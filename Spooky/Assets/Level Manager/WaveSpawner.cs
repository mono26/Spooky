using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SingleObjectPool), typeof(SingleObjectPool))]
public class WaveSpawner : SceneSingleton<WaveSpawner>
{
    public enum SpawnState
    {
        SPAWNING, COUNTING, WAITING
    }

    [SerializeField]
    private Character[] enemiesToSpawn;
    [SerializeField]
    private SingleObjectPool[] enemiesPools;
    [SerializeField]
    private int[] minSpawnPerEnemy;
    private const int minWaveRequiredToSpawnEnemy2 = 3;
    
    [SerializeField]
    private int currentMaxNumberOfEnemiesLeft = 0;
    [SerializeField]
    private int currentNumberOfEnemiesKilled = 0;
    [SerializeField]
    private int currentWave = 0;

    [SerializeField]
    private float timeBetweenNextWaveSpawn = 3.0f;
    // Number of enemies to spawn per second
    [SerializeField]
    private float enemySpawnRate = 2f;
    private float waveCompletedTimer;

    [SerializeField]
    private AudioClip SpawnSound;

    //State of the waveSpawn
    private SpawnState waveSpawnerState;

    public delegate void OnSpawnStartDelegate(float _currentWave,float _numberOfEnemies, float _currentKilled);
    public event OnSpawnStartDelegate OnSpawnStart;
    public delegate void OnEnemyKilledDelegate(float _currentWave, float _numberOfEnemies, float _currentKilled);
    public event OnEnemyKilledDelegate OnEnemyKilled;

    protected override void Awake()
    {
        base.Awake();

        enemiesPools = GetComponents<SingleObjectPool>();
    }
    private void OnDisable()
    {
        LevelManager.Instance.OnStartLevel -= PlayEnemySpawnClip;
    }

    // Use this for initialization
    private void Start()
    {
        LevelManager.Instance.OnStartLevel += PlayEnemySpawnClip;

        waveSpawnerState = SpawnState.COUNTING;
    }
    // Update is called once per frame
    private void Update()
    {
        //To see if all waves are finished
        if (currentWave > 100)
        {
            LevelManager.Instance.WinLevel();
            this.enabled = false;
            return;
        }

        if (waveSpawnerState == SpawnState.WAITING)
        {
            if (currentMaxNumberOfEnemiesLeft == 0)
            {
                //Begin a new Round
                WaveCompleted();
                return;
            }
        }

        if (waveSpawnerState == SpawnState.COUNTING)
        {
            if (Time.timeSinceLevelLoad > timeBetweenNextWaveSpawn + waveCompletedTimer && currentMaxNumberOfEnemiesLeft == 0)
            {
                //If the countdown to start spawning the next wave is 0 and is not spawning auotomaticly force it to spawn
                if (waveSpawnerState != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave());
                    return;
                }
            }
        }

        else return;

    }
    private IEnumerator SpawnWave()
    {
        waveSpawnerState = SpawnState.SPAWNING;

        currentMaxNumberOfEnemiesLeft = 0;
        currentNumberOfEnemiesKilled = 0;
        for(int i = 0; i < enemiesToSpawn.Length; i++)
        {
            currentMaxNumberOfEnemiesLeft += CalculateNumberOfEnemiesToSpawn(enemiesToSpawn[i].CharacterID);
        }

        if (OnSpawnStart != null)
        {
            OnSpawnStart(currentWave, currentMaxNumberOfEnemiesLeft, currentNumberOfEnemiesKilled);
        }

        int numberOfSpawns = currentMaxNumberOfEnemiesLeft;
        // TODO in the future think of better architecture
        while (numberOfSpawns > 0)
        {
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                SpawnEnemy(enemiesToSpawn[i].CharacterID);
                numberOfSpawns--;
                yield return new WaitForSeconds(1f / enemySpawnRate);
            }
        }
        waveSpawnerState = SpawnState.WAITING;
        yield break;
    }

    private void SpawnEnemy(string _enemyID)
    {
        //Random between all the spawnpoints
        Enemy tempEnemy = null;
        switch (_enemyID)
        {
            case "Stealer":
                tempEnemy = enemiesPools[0].GetObjectFromPool().GetComponent<Enemy>();
                break;

            case "Attacker":
                tempEnemy = enemiesPools[1].GetObjectFromPool().GetComponent<Enemy>();
                break;

            default:
                break;
        }
        tempEnemy.CharacterTransform.position = LevelManager.Instance.GetRandomSpawnPoint().position;
        /*if(currentWave >= 10)
            tempEnemy.StatsComponent.ScaleStatsByFactor();*/

        PoolableObject poolable = tempEnemy.GetComponent<PoolableObject>();
        if(poolable != null)
            poolable.OnRelease += EnemyKilled;
        currentMaxNumberOfEnemiesLeft++;

        return;
    }

    private void WaveCompleted()
    {
        waveSpawnerState = SpawnState.COUNTING;
        waveCompletedTimer = Time.timeSinceLevelLoad;
        currentWave++;
    }

    private void EnemyKilled()
    {
        currentMaxNumberOfEnemiesLeft--;
        currentMaxNumberOfEnemiesLeft = (int)Mathf.Clamp(currentMaxNumberOfEnemiesLeft, 0, Mathf.Infinity);    // So the number of enemies never goes below 0
        currentNumberOfEnemiesKilled++;
        currentNumberOfEnemiesKilled = (int)Mathf.Clamp(currentNumberOfEnemiesKilled, 0, Mathf.Infinity);

        if (OnEnemyKilled != null)
            OnEnemyKilled(currentWave ,currentMaxNumberOfEnemiesLeft, currentNumberOfEnemiesKilled);
        return;
    }

    private void PlayEnemySpawnClip()
    {
        GameManager.Instance.SoundManager.PlayClip(SpawnSound);
    }

    private int CalculateNumberOfEnemiesToSpawn(string _enemyID)
    {
        int numberOfEnemiesToSpawn = 0;
        switch(_enemyID)
        {
            case "Stealer":
                numberOfEnemiesToSpawn = minSpawnPerEnemy[0] * currentWave;
                break;

            case "Attacker":
                if(currentWave > minWaveRequiredToSpawnEnemy2)
                numberOfEnemiesToSpawn = minSpawnPerEnemy[1] * currentWave;
                break;

            default:
                break;
        }
        return numberOfEnemiesToSpawn;
    }
}
