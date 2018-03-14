using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } }

    public Fader sceneFader;
    public Image blackBackGround;
    public AnimationCurve fadeCurve;

    [SerializeField]
    private AudioSource efxSource;
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private float minLoadDuration = 3f;

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

        sceneFader = new Fader(blackBackGround, fadeCurve);
        soundManager = new SoundManager(efxSource, musicSource);
    }

    private void Start()
    {
        StartCoroutine(sceneFader.FadeInLevel());
    }

    public IEnumerator LoadLevel(string _level)
    {
        var startLoad = Time.unscaledDeltaTime;
        Time.timeScale = 0f;

        yield return SceneManager.LoadSceneAsync("LoadScene");

        sceneFader.FadeInLevel();

        bool continueToLevel = false;

        // Load level async
        yield return SceneManager.LoadSceneAsync(_level, LoadSceneMode.Additive);

        var loadingText = GameObject.Find("Load Text").GetComponent<Text>();
        loadingText.text = "Loading";

        do
        {
            var time = Time.unscaledTime;
            Debug.Log(time + " fading IN loading text");

            yield return StartCoroutine(sceneFader.FadeInObject(loadingText));

            Debug.Log(time + " fading OUT loading text");

            yield return StartCoroutine(sceneFader.FadeOutObject(loadingText));
        }
        while (minLoadDuration > Time.unscaledTime - startLoad);

        loadingText.text = "Click to continue";

        while (!continueToLevel)
        {
            yield return StartCoroutine(sceneFader.FadeInObject(loadingText));

            if (Input.GetMouseButton(0))
            {
                continueToLevel = true;
                yield return 0;
            }

            yield return StartCoroutine(sceneFader.FadeOutObject(loadingText));
        }

        Time.timeScale = 1f;

        yield return SceneManager.UnloadSceneAsync("LoadScene");

        OnStartGame();
    }
}
