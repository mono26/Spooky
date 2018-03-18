using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } }

    private LoadManager loadManager;
    public LoadManager LoadManager { get { return loadManager; } }

    // TODO use gameObject Find
    public Fader sceneFader;
    public AnimationCurve fadeCurve;

    [SerializeField]
    private AudioSource efxSource;
    [SerializeField]
    private AudioSource musicSource;

    public delegate void StartGame();
    public event StartGame OnStartGame;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            instance = this;
        }
        else
            instance = this;

        sceneFader = new Fader(GameObject.FindGameObjectWithTag("Black Fade").GetComponent<Image>(), fadeCurve);
        soundManager = new SoundManager(efxSource, musicSource);
        loadManager = new LoadManager();
    }

    private void Start()
    {
        StartCoroutine(sceneFader.FadeInLevel());
    }

    public IEnumerator LoadLevel(int _levelIndex)
    {
        yield return StartCoroutine(loadManager.LoadLevel(_levelIndex));

        yield return StartCoroutine(loadManager.FinishLoading());

        OnStartGame();
    }

}
