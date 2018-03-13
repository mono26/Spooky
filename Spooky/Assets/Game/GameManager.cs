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

    [SerializeField]
    private AudioSource efxSource;
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private float minLoadDuration = 3f;

    public delegate void OnGameStart();
    public event OnGameStart StartGame;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            instance = this;
        }
        else
            instance = this;

        soundManager = new SoundManager(efxSource, musicSource);
    }

    public IEnumerator LoadLevel(string _level)
    {
        Time.timeScale = 0f;

        yield return SceneManager.LoadSceneAsync("LoadScene");

        bool continueToLevel = false;

        // Load level async
        yield return SceneManager.LoadSceneAsync(_level, LoadSceneMode.Additive);

        // TODO loading fade during the minduration.
        yield return new WaitForSecondsRealtime(minLoadDuration - Time.timeSinceLevelLoad);

        var text = GameObject.Find("Load Text");
        text.GetComponent<Text>().text = "Click to continue";

        while (!continueToLevel)
        {
            if (Input.GetMouseButton(0))
            {
                continueToLevel = true;
                yield return 0;
            }
            yield return 0;
        }

        Time.timeScale = 1f;

        yield return SceneManager.UnloadSceneAsync("LoadScene");

        StartGame();
    }

    void FadeObject()
    {

    }
}
