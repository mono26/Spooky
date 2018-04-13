using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("Sound Settings")]
    [SerializeField]
    private AudioSource efxSource;
    [SerializeField]
    private AudioSource musicSource;
    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } }

    [Header("Load Settings")]
    private LoadManager loadManager;
    public LoadManager LoadManager { get { return loadManager; } }

    [Header("Fade Settings")]
    [SerializeField]
    private AnimationCurve fadeCurve;
    public AnimationCurve FadeCurve { get { return fadeCurve; } }
    private Fader sceneFader;
    public Fader SceneFader { get { return sceneFader; } }

    [Header("Enemy Stats Settings")]
    [SerializeField]
    private EnemyStatsCollection enemyStats;
    public EnemyStatsCollection EnemyStats { get { return enemyStats; } }

    public delegate void StartGame();
    public event StartGame OnStartGame;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            instance = this;
        }
        else if(!instance)
            instance = this;

        sceneFader = new Fader(GameObject.FindGameObjectWithTag("Black Fade").GetComponent<Image>(), FadeCurve);
        soundManager = new SoundManager(efxSource, musicSource);
        loadManager = new LoadManager();
        return;
    }

    private void Start()
    {
        StartCoroutine(SceneFader.FadeInLevel());
        return;
    }

    public IEnumerator LoadLevel(string _sceneName)
    {
        yield return StartCoroutine(loadManager.LoadLevel(_sceneName));

        yield return StartCoroutine(loadManager.FinishLoading());

        if (OnStartGame != null)
            OnStartGame();
    }

}
