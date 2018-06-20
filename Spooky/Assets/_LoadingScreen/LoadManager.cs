using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private static string loadingScreenSceneName = "LoadingScreen";
    private static string sceneToLoad;

    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Image loadingFill;
    private const float loadDelay = 1.0f;
    private const float exitFadeDuration = 1.0f;
    private AsyncOperation loadOperation;

    public static void LoadScene(string _sceneToLoad)
    {
        sceneToLoad = _sceneToLoad;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (loadingScreenSceneName != null)
        {
            SceneManager.LoadScene(loadingScreenSceneName);
        }
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        loadingText.text = "Loading...";
        if (sceneToLoad != "")
        {
            StartCoroutine(LoadLevel());
        }
    }

    public IEnumerator LoadLevel()
    {
        StartLoading();

        while(loadOperation.progress < 0.9f)
        {
            loadingFill.fillAmount = loadOperation.progress;
            yield return null;
        }

        loadingFill.fillAmount = 1f;

        yield return new WaitForSecondsRealtime(loadDelay);

        EventManager.TriggerEvent(new FadeEvent(FadeEventType.FadeOut));
        yield return new WaitForSecondsRealtime(exitFadeDuration);

        loadOperation.allowSceneActivation = true;
    }

    private void StartLoading()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        loadOperation.allowSceneActivation = false;
        return;
    }
}
