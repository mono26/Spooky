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

    private int currentEnemiesLeft = 0;
    public int CurrentEnemiesLeft { get { return currentEnemiesLeft; } }
    private int currentWave = 0;
    public int CurrentWave { get { return currentWave; } }

    [SerializeField]
    private float timeBetweenNextWaveSpawn = 5.0f;
    // Number of enemies to spawn per second
    [SerializeField]
    private float enemySpawnRate = 4;
    private float waveCompletedTime;

    [SerializeField]
    private AudioClip SpawnSound;

    //State of the waveSpawn
    private SpawnState waveSpawnerState;

    public delegate void OnSpawnStartDelegate();
    public event OnSpawnStartDelegate OnSpawnStart;

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
            if (currentEnemiesLeft == 0)
            {
                //Begin a new Round
                WaveCompleted();
                return;
            }
        }

        if (waveSpawnerState == SpawnState.COUNTING)
        {
            if (Time.timeSinceLevelLoad > timeBetweenNextWaveSpawn + waveCompletedTime && currentEnemiesLeft == 0)
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
        if(OnSpawnStart != null)
        {
            OnSpawnStart();
        }

        currentEnemiesLeft = 0;
        int totalNumberOfEnemiesToSpawn = 0;
        for(int i = 0; i < enemiesToSpawn.Length; i++)
        {
            totalNumberOfEnemiesToSpawn += CalculateNumberOfEnemiesToSpawn(enemiesToSpawn[i].CharacterID);
        }

        // TODO in the future think of better architecture
        while (totalNumberOfEnemiesToSpawn > 0)
        {
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                SpawnEnemy(enemiesToSpawn[i].CharacterID);
                totalNumberOfEnemiesToSpawn--;
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
        PoolableObject poolable = tempEnemy.GetComponent<PoolableObject>();
        if(poolable != null)
            poolable.OnRelease += DecreaseEnemyNumber;
        currentEnemiesLeft++;
    }

    private void WaveCompleted()
    {
        waveSpawnerState = SpawnState.COUNTING;
        waveCompletedTime = Time.timeSinceLevelLoad;
        currentWave++;
    }

    private void DecreaseEnemyNumber()
    {
        currentEnemiesLeft--;
        currentEnemiesLeft = (int)Mathf.Clamp(currentEnemiesLeft, 0, Mathf.Infinity);    // So the number of enemies never goes below 0
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
