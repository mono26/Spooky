using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SingleObjectPool), typeof(SingleObjectPool), typeof(AudioSource))]
public class WaveSpawner : SceneSingleton<WaveSpawner>, EventHandler<CharacterEvent>
{
    public enum SpawnState
    {
        SPAWNING, COUNTING, WAITING
    }

    [SerializeField]
    private const int minWaveRequiredToSpawnEnemy2 = 3;

    [Header("Waves settings")]
    [SerializeField]
    private Character[] enemies;
    [SerializeField]
    private SingleObjectPool[] enemiesPools;
    [SerializeField]
    private int[] minSpawnPerEnemy;
    [SerializeField]
    private float timeBetweenNextWaveSpawn = 3.0f;
    [SerializeField]
    private float enemySpawnRatePerSecond = 2f;

    [Header("Wave Spawner sounds")]
    [SerializeField]
    private AudioSource waveSoundSource;
    [SerializeField]
    private AudioClip spawnSfx;

    [Header("Game Info (Only for debugging.)")]
    [SerializeField]
    private int currentMaxNumberOfEnemiesLeft = 0;
    public int CurrentMaxNumberOfEnemiesLeft { get { return currentMaxNumberOfEnemiesLeft; } }
    [SerializeField]
    private int currentNumberOfEnemiesKilled = 0;
    public int CurrentNumberOfEnemiesKilled { get { return currentNumberOfEnemiesKilled; } }
    [SerializeField]
    private int currentWave = 1;
    public int CurrentWave { get { return currentWave; } }

    [SerializeField]
    private int[] enemiesToSpawn;
    private float waveCompletedTimer;
    [SerializeField]
    private SpawnState waveSpawnerState;


    protected override void Awake()
    {
        base.Awake();

        if(enemiesPools == null)
            enemiesPools = GetComponents<SingleObjectPool>();
        if(waveSoundSource == null)
            waveSoundSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    protected void Start()
    {
        waveSpawnerState = SpawnState.COUNTING;
        enemiesToSpawn = new int[enemies.Length];
        return;
    }

    protected void OnEnable()
    {
        EventManager.AddListener<CharacterEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<CharacterEvent>(this);
        return;
    }

    // Update is called once per frame
    protected void Update()
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
            if (currentNumberOfEnemiesKilled == currentMaxNumberOfEnemiesLeft)
            {
                //Begin a new Round
                WaveCompleted();
                return;
            }
        }

        if (waveSpawnerState == SpawnState.COUNTING)
        {
            if (Time.timeSinceLevelLoad > timeBetweenNextWaveSpawn + waveCompletedTimer)
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
        SoundManager.Instance.PlaySfx(waveSoundSource, spawnSfx);

        for(int i = 0; i < enemiesToSpawn.Length; i++)
        {
            enemiesToSpawn[i] = CalculateNumberOfEnemiesToSpawn(enemies[i].CharacterID);
            currentMaxNumberOfEnemiesLeft += CalculateNumberOfEnemiesToSpawn(enemies[i].CharacterID);
        }

        int numberOfSpawns = currentMaxNumberOfEnemiesLeft;
        EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEventTypes.SpawnStart));

        while (numberOfSpawns > 0)
        {
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] > 0)
                {
                    SpawnEnemy(enemies[i].CharacterID, enemiesToSpawn[i]);
                    enemiesToSpawn[i]--;
                    numberOfSpawns--;
                }
                yield return new WaitForSeconds(1f / enemySpawnRatePerSecond);
            }
        }

        waveSpawnerState = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemy(string _enemyID, int _enemiesLeft)
    {
        Character tempEnemy = null;
        switch (_enemyID)
        {
            case "Stealer":
                tempEnemy = enemiesPools[0].GetObjectFromPool().GetComponent<Character>();
                break;

            case "Attacker":
                tempEnemy = enemiesPools[1].GetObjectFromPool().GetComponent<Character>();
                break;

            default:
                break;
        }
        tempEnemy.CharacterTransform.position = LevelManager.Instance.GetRandomSpawnPoint().position;

        return;
    }

    private void WaveCompleted()
    {
        waveSpawnerState = SpawnState.COUNTING;
        waveCompletedTimer = Time.timeSinceLevelLoad;
        currentMaxNumberOfEnemiesLeft = 0;
        currentNumberOfEnemiesKilled = 0;
        currentWave++;
    }

    private void EnemyKilled()
    {
        currentMaxNumberOfEnemiesLeft--;
        currentMaxNumberOfEnemiesLeft = (int)Mathf.Clamp(currentMaxNumberOfEnemiesLeft, 0, Mathf.Infinity);    // So the number of enemies never goes below 0
        currentNumberOfEnemiesKilled++;
        currentNumberOfEnemiesKilled = (int)Mathf.Clamp(currentNumberOfEnemiesKilled, 0, Mathf.Infinity);

        return;
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

    public void OnEvent(CharacterEvent _characterEvent)
    {
        if(!_characterEvent.character.CharacterID.Equals("Spooky"))
        {
            if (_characterEvent.type == CharacterEventType.Death)
                EnemyKilled();
        }
    }
}
