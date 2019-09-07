using System.Collections;
using UnityEngine;
using UnityEngine.UI;   

[RequireComponent(typeof(SingleObjectPool), typeof(SingleObjectPool), typeof(AudioSource))]
public class WaveSpawner : Singleton<WaveSpawner>, EventHandler<CharacterEvent>, EventHandler<GameEvent>
{
    public enum SpawnState
    {
        SPAWNING, COUNTING, WAITING
    }

    [SerializeField]
    private const int minWaveRequiredToSpawnEnemy2 = 3;

    [Header("Waves Spawner settings")]
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
    [SerializeField]
    private Text waveCounter;
    [SerializeField]
    private ProgressBar waveProgress;

    [Header("Wave Spawner sounds")]
    [SerializeField]
    private AudioSource waveSoundSource;
    [SerializeField]
    private AudioClip spawnSfx;

    [Header("Game Info (Only for debugging.)")]
    [SerializeField]
    private int currentMaxNumberOfEnemiesLeft = 0;
    [SerializeField]
    private int currentNumberOfEnemiesKilled = 0;
    [SerializeField]
    private int currentWave = 1;

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
        if (waveCounter == null)
            waveCounter = GameObject.Find("WaveBarFrame").transform.Find("WaveCounter").GetComponent<Text>();
        if (waveProgress == null)
            waveProgress = GameObject.Find("WaveBarFrame").transform.Find("WaveProgressBar").GetComponent<ProgressBar>();

        return;
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
        EventManager.AddListener<GameEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<CharacterEvent>(this);
        EventManager.RemoveListener<GameEvent>(this);
        return;
    }

    protected void Update()
    {
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
                WaveCompleted();
                return;
            }
        }

        if (waveSpawnerState == SpawnState.COUNTING)
        {
            // TODO check this statement. Not waiting the required time.
            if (Time.timeSinceLevelLoad > timeBetweenNextWaveSpawn + waveCompletedTimer)
            {
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
        SoundManager.Instance.PlaySfx(waveSoundSource, spawnSfx);

        for(int i = 0; i < enemiesToSpawn.Length; i++)
        {
            enemiesToSpawn[i] = CalculateNumberOfEnemiesToSpawn(enemies[i].CharacterID);
            currentMaxNumberOfEnemiesLeft += enemiesToSpawn[i];
        }

        int numberOfSpawns = currentMaxNumberOfEnemiesLeft;
        EventManager.TriggerEvent<GameEvent>(new GameEvent(GameEventTypes.SpawnStart));

        //TODO spawn per enemy number, not total.
        while (numberOfSpawns > 0)
        {
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] > 0)
                {
                    SpawnEnemy(enemies[i].CharacterID);
                    enemiesToSpawn[i]--;
                    numberOfSpawns--;
                }
                yield return new WaitForSeconds(1f / enemySpawnRatePerSecond);
            }
        }

        waveSpawnerState = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemy(string _enemyID)
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
        tempEnemy.transform.position = LevelManager.Instance.GetRandomSpawnPoint().position;

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
                if(currentWave >= minWaveRequiredToSpawnEnemy2)
                    numberOfEnemiesToSpawn = minSpawnPerEnemy[1] * currentWave;
                break;

            default:
                break;
        }
        return numberOfEnemiesToSpawn;
    }

    private void UpdateWaveSpawnerUI()
    {
        waveCounter.text = currentWave.ToString();

        if (currentMaxNumberOfEnemiesLeft > 0)
            waveProgress.UpdateBar(currentNumberOfEnemiesKilled, currentMaxNumberOfEnemiesLeft);

        return;
    }

    public void OnEvent(CharacterEvent _characterEvent)
    {
        if(!_characterEvent.character.CharacterID.Equals("Spooky"))
        {
            if (_characterEvent.type == CharacterEventType.Death)
            {
                EnemyKilled();
                UpdateWaveSpawnerUI();
            }
            if (_characterEvent.type == CharacterEventType.Release)
            {
                EnemyKilled();
                UpdateWaveSpawnerUI();
            }
        }
        return;
    }

    public void OnEvent(GameEvent _gameManagerEvent)
    {
        if (_gameManagerEvent.type == GameEventTypes.SpawnStart)
            UpdateWaveSpawnerUI();

        return;
    }
}
